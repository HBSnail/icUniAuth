Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Xml

Module Utils


    Public Function CheckLoginAndGetToken(username As String, password As String, ipaddr As String) As String
        Dim usernameBase64 As String = GetBase64FromStringAsUtf8Format(username)
        Dim token As String = Nothing
        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
        oledbCommand.CommandText = "select passwordhash,salt,state,loginfailedtimes from userinfo where username='" & usernameBase64 & "'"
        Dim dbReader As MySqlDataReader = oledbCommand.ExecuteReader()
        If dbReader.HasRows Then
            If dbReader.Read() Then
                If IsDBNull(dbReader("loginfailedtimes")) OrElse CInt(dbReader("loginfailedtimes") >= 10) Then
                    dbReader.Close()
                    Throw New Exception("请重置密码以解锁账号")
                Else
                    Dim passwordHashwithSalt As String = SHA512(SHA512(password).ToUpper & dbReader("salt").ToString).ToUpper
                    If passwordHashwithSalt = dbReader("passwordhash").ToString.ToUpper Then
                        Dim accountstate As Integer = CInt(If(IsDBNull(dbReader("state")), -2, dbReader("state")))
                        dbReader.Close()
                        oledbCommand.CommandText = "update userinfo Set loginfailedtimes=0 where username='" & usernameBase64 & "'"
                        oledbCommand.ExecuteNonQuery()
                        If accountstate = 1 Then

                            token = SHA512(Now.ToString & username & password & GetRandomString(10)).ToUpper
                            oledbCommand.CommandText = "delete from tokens where username='" & usernameBase64 & "'"
                            oledbCommand.ExecuteNonQuery()
                            oledbCommand.CommandText = "insert into tokens (token,username,createtime,loginip) values ('" & SHA512(token) & "','" & usernameBase64 & "','" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "','" & ipaddr & "')"
                            oledbCommand.ExecuteNonQuery()

                        ElseIf accountstate = 0 Then
                            dbReader.Close()
                            Throw New Exception("该账号未激活")
                        ElseIf accountstate = -1 Then
                            dbReader.Close()
                            Throw New Exception("该账号被锁定")
                        Else
                            dbReader.Close()
                            Throw New Exception("该账号状态错误")
                        End If
                    Else
                        dbReader.Close()
                        oledbCommand.CommandText = "update userinfo set loginfailedtimes=loginfailedtimes+1 where username='" & usernameBase64 & "'"
                        oledbCommand.ExecuteNonQuery()
                        Throw New Exception("用户名或密码错误")
                    End If
                End If
            Else
                dbReader.Close()
                Throw New Exception("数据读取失败")
            End If
        Else
            dbReader.Close()
            Throw New Exception("用户名或密码错误")
        End If
        If token Is Nothing Then
            dbReader.Close()
            Throw New Exception("系统错误")
        End If
        Return token
    End Function

    Public Sub ActiveUserAccount(uuid As String)

        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
        oledbCommand.CommandText = "Select state from userinfo where useruuid='" & uuid & "'"
        Dim r As MySqlDataReader = oledbCommand.ExecuteReader()
        r.Read()
        Dim x As Integer = CInt(r("state"))
        r.Close()
        If x = 0 Then
            oledbCommand.CommandText = "update userinfo set loginfailedtimes=0,opkey='',state=1 where useruuid='" & uuid & "'"
            oledbCommand.ExecuteNonQuery()

            oledbCommand.CommandText = "create table " & uuid & "(name longtext,val longtext)"
            oledbCommand.ExecuteNonQuery()
        Else
            Throw New Exception("账号已激活")
        End If
    End Sub

    'Register a new user and return the active_key
    Public Function RegisterUser(email As String, password As String) As String

        Dim usernameBase64 As String = GetBase64FromStringAsUtf8Format(email)
        Dim activekey As String = Nothing

        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
        oledbCommand.CommandText = "select state from userinfo where username='" & usernameBase64 & "'"
        Dim dbReader As MySqlDataReader = oledbCommand.ExecuteReader()
        If dbReader.HasRows AndAlso dbReader.Read() AndAlso CInt(dbReader("state")) <> 0 Then
            dbReader.Close()
            Throw New Exception("该用户已注册")
        Else
            dbReader.Close()
            Dim salt As String = GetRandomString(16)
            activekey = SHA512(GetRandomString(32) & New Random().Next(10000, Integer.MaxValue).ToString)
            Dim passwordHashwithSalt As String = SHA512(SHA512(password).ToUpper & salt).ToUpper
            oledbCommand.CommandText = "delete from userinfo where username='" & usernameBase64 & "'"
            oledbCommand.ExecuteNonQuery()

            'Notice the activekey saved in the db is in its sha512 form
            oledbCommand.CommandText = "insert into userinfo (username,passwordhash,salt,state,useruuid,loginfailedtimes,opkey) values ('" &
                    usernameBase64 & "','" & passwordHashwithSalt & "','" & salt & "',0,'" & Guid.NewGuid().ToString("N") & "',0,'" & SHA512(activekey) & "')"
            oledbCommand.ExecuteNonQuery()

        End If

        If activekey Is Nothing Then
            dbReader.Close()
            Throw New Exception("系统错误")
        End If
        Return activekey

    End Function

    Public Sub SetUserConfig(uuid As String, key As String, value As String)

        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()

        oledbCommand.CommandText = "delete from " & uuid & " where name='" & GetBase64FromStringAsUtf8Format(key) & "'"
        oledbCommand.ExecuteNonQuery()

        oledbCommand.CommandText = "insert into " & uuid & " (name,val) values('" & GetBase64FromStringAsUtf8Format(key) & "','" & GetBase64FromStringAsUtf8Format(value) & "')"
        oledbCommand.ExecuteNonQuery()

    End Sub

    Public Function getUserConfig(uuid As String, key As String) As String
        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()

        oledbCommand.CommandText = "select val from " & uuid & " where name='" & GetBase64FromStringAsUtf8Format(key) & "'"
        Dim dbReader As MySqlDataReader = oledbCommand.ExecuteReader()
        If dbReader.HasRows AndAlso dbReader.Read() Then
            Dim ret As String = dbReader("val")
            dbReader.Close()
            Return GetStringAsUtf8FormatFromBase64(ret)
        End If
        dbReader.Close()
        Return Nothing
    End Function

    Public Function GetAppProperty(apptoken As String, prop As String, md5ed As Boolean) As String
        Dim reader As MySqlDataReader
        Try
            If md5ed Then
                If Not VerifyHexString(apptoken) Then
                    Return Nothing
                End If
            End If
            If apptoken IsNot Nothing AndAlso
             apptoken.Trim <> "" Then

                Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()
                oledbCommand.CommandText = "select val from " & If(md5ed, apptoken, MD5(apptoken).ToUpper) & " where name='" & GetBase64FromStringAsUtf8Format(prop) & "'"
                reader = oledbCommand.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    If Not IsDBNull(reader("val")) Then
                        Dim retStr As String = GetStringAsUtf8FormatFromBase64(reader("val"))
                        reader.Close()
                        Return retStr
                    End If
                End If
                reader.Close()
            End If
        Catch
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Sub SetAppProperty(apptoken As String, name As String, value As String, md5ed As Boolean)
        Dim insertbase64 As String = GetBase64FromStringAsUtf8Format(value)

        Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()
        oledbCommand.CommandText = "insert into " & If(md5ed, apptoken, MD5(apptoken).ToUpper) & " (name,val) values ('" & GetBase64FromStringAsUtf8Format(name) & "','" & GetBase64FromStringAsUtf8Format(value) & "')"
        oledbCommand.ExecuteNonQuery()

    End Sub


    Function createTableDat(appname As String, op As String, state As String, time As String, Optional cstate As String = "success") As String
        Return "<tr><td><a>" & appname & "</a></td><td class=""font-weight-600"">" & op & "</td><td><div class=""badge badge-" & cstate & """>" & state & "</div></td><td>" & time & "</td></tr>"
    End Function

    Public Sub AddauthInfo(uuid As String, apptoken As String, authinfo As String)
        Try
            If apptoken IsNot Nothing AndAlso
             apptoken.Trim <> "" Then

                Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
                oledbCommand.CommandText = "insert into authinfo (uuid,apptoken,authinfo,authtime) values ('" & uuid & "','" & MD5(apptoken) & "','" & authinfo & "','" & Now.ToString & "')"
                oledbCommand.ExecuteNonQuery()
            End If
        Catch
        End Try
    End Sub

    Public Sub RegRespToken(uuid As String, resptoken As String, apptoken As String)
        Try


            Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
            oledbCommand.CommandText = "delete from resptoken where ((timestampdiff(SECOND,authedtime,now())) > 60)"
            oledbCommand.ExecuteNonQuery()
            oledbCommand.CommandText = "insert into resptoken (uuid,resptoken,apptoken,authedtime) values ('" & uuid & "','" & MD5(resptoken) & "','" & MD5(apptoken) & "','" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')"
            oledbCommand.ExecuteNonQuery()

        Catch
        End Try
    End Sub

    Public Sub DeleteAuthInfo(uuid As String, apptoken As String)
        Try
            If Not VerifyHexString(apptoken) Then
                Return
            End If

            Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
            oledbCommand.CommandText = "delete from authinfo where uuid='" & uuid & "' and apptoken='" & apptoken & "'"
            oledbCommand.ExecuteNonQuery()
        Catch

        End Try
    End Sub

    Public Function GetAuthInfo(uuid As String, apptoken As String, md5ed As Boolean) As String
        Dim reader As MySqlDataReader
        Try
            If md5ed Then
                If Not VerifyHexString(apptoken) Then
                    Return Nothing
                End If
            End If
            If apptoken IsNot Nothing AndAlso
             apptoken.Trim <> "" Then

                Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
                oledbCommand.CommandText = "select authinfo from authinfo where uuid='" & uuid & "' and apptoken='" & If(md5ed, apptoken, MD5(apptoken).ToUpper) & "'"
                reader = oledbCommand.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    If Not IsDBNull(reader("authinfo")) Then
                        Dim retdat As String = reader("authinfo")
                        reader.Close()
                        Return retdat
                    End If
                End If
                reader.Close()
            End If
        Catch

            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Function GetAllAuthInfo(uuid As String) As String()
        Dim reader As MySqlDataReader
        Dim stringret As String = ""
        Try


            Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
            oledbCommand.CommandText = "select apptoken,authinfo,authtime from authinfo where uuid='" & uuid & "'"
            reader = oledbCommand.ExecuteReader()
            If reader.HasRows Then
                While reader.Read()
                    If Not IsDBNull(reader("apptoken")) AndAlso Not IsDBNull(reader("authinfo")) AndAlso Not IsDBNull(reader("authtime")) Then
                        stringret &= reader("apptoken") & "," & reader("authinfo") & "," & reader("authtime") & ";"
                    End If
                End While
            End If
            reader.Close()
        Catch

            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return stringret.Split(";")
    End Function

    Public Function VerifyAuthRequestToken(authtoken As String, appprivatekey As String) As String
        If authtoken Is Nothing OrElse appprivatekey Is Nothing Then
            Return "BAD_TOKEN"
        End If
        Dim reader As MySqlDataReader
        Try
            Dim q_authtoken As String = MD5(authtoken)

            Dim oledbCommand As MySqlCommand = AuthInfoConnection.CreateCommand()
            oledbCommand.CommandText = "delete from resptoken where ((timestampdiff(SECOND,authedtime,now())) > 60)"
            oledbCommand.ExecuteNonQuery()
            oledbCommand.CommandText = "select uuid,apptoken from resptoken where resptoken='" & q_authtoken & "'"
            reader = oledbCommand.ExecuteReader()
            If reader.HasRows Then

                Dim list As New List(Of (String, String))
                While reader.Read()
                    If Not IsDBNull(reader("apptoken")) AndAlso Not IsDBNull(reader("uuid")) Then
                        list.Add((reader("apptoken"), reader("uuid")))
                    End If
                End While
                reader.Close()
                For Each dat As (String, String) In list
                    Dim apptoken As String = dat.Item1
                    Dim uuid As String = dat.Item2
                    Dim privatetoken As String = GetAppProperty(apptoken, "privatetoken", True)
                    If SHA512(appprivatekey) <> SHA512(privatetoken) Then
                        oledbCommand.CommandText = "delete from resptoken where resptoken='" & q_authtoken & "'"
                        oledbCommand.ExecuteNonQuery()
                        list.Clear()
                        Return "PRIVATE_TOKEN_INVALID"
                    End If
                    Dim getdata As String = GetAuthInfo(uuid, apptoken, True)
                    If getdata IsNot Nothing Then

                        oledbCommand.CommandText = "delete from resptoken where resptoken='" & q_authtoken & "'"
                        oledbCommand.ExecuteNonQuery()
                        list.Clear()
                        Return "OK@" & getdata & "@" & uuid
                    End If
                Next
                list.Clear()

            Else
                reader.Close()
                Return "AUTH_TOKEN_INVALID"
            End If
            reader.Close()
            oledbCommand.CommandText = "delete from resptoken where resptoken='" & q_authtoken & "'"
            oledbCommand.ExecuteNonQuery()

        Catch
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return "ERROR_DETECTED"
    End Function

    Public Function RegisterApp(appname As String, redurl As String, apptoken As String, appprivatetoken As String, req As String, selectedIndex As Integer, uuid As String) As Boolean
        Dim reader As MySqlDataReader
        Try
            Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()

            oledbCommand.CommandText = "select * from appindex where appname='" & GetBase64FromStringAsUtf8Format(appname) & "'"
            reader = oledbCommand.ExecuteReader()
            If reader.HasRows Then

                reader.Close()
                Return False
            End If
            reader.Close()
            oledbCommand.CommandText = "select apptoken from appindex where uuid='" & uuid & "'"
            reader = oledbCommand.ExecuteReader()
            Dim cnt As Integer = 0
            If reader.HasRows Then
                While reader.Read()
                    cnt += 1
                End While
            End If
            If cnt >= 5 Then

                reader.Close()
                Return False
            End If
            reader.Close()

            oledbCommand.CommandText = "insert into appindex (uuid,apptoken,appname) values ('" & uuid & "','" & MD5(apptoken) & "','" & GetBase64FromStringAsUtf8Format(appname) & "')"
            oledbCommand.ExecuteNonQuery()
            oledbCommand.CommandText = "create table " & MD5(apptoken) & "(name longtext,val longtext)"
            oledbCommand.ExecuteNonQuery()

            SetAppProperty(MD5(apptoken), "appname", (appname), True)
            SetAppProperty(MD5(apptoken), "redirect", (redurl), True)
            SetAppProperty(MD5(apptoken), "publictoken", (apptoken), True)
            SetAppProperty(MD5(apptoken), "privatetoken", (appprivatetoken), True)
            SetAppProperty(MD5(apptoken), "required", (req), True)
            SetAppProperty(MD5(apptoken), "mode", (selectedIndex), True)
            Return True

        Catch
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return False
    End Function



    Public Function addMyAPP(appname As String, publictoken As String, privatetoken As String, require As String, mode As String) As String
        Return "<tr><td><div>" & appname & "</div></td><td><div>" & publictoken & "</div></td><td><div>" & privatetoken & "</div></td><td><div>" & require & "</div></td><td>" & mode & "</div></td><td><div><a  href=""manageapp?apptoken=" & publictoken & """ class=""btn btn-success"" style=""color:#ffffff"">管理</a><a href=""deleteapp?apptoken=" & publictoken & """ class=""btn btn-danger"" style=""color:#ffffff"">删除</a></div></td></tr>"
    End Function

    Public Sub DeleteMyApp(uuid As String, appToken As String)
        appToken = MD5(appToken)
        If IsMyApp(uuid, appToken) Then
            Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()
            oledbCommand.CommandText = "delete from appindex where uuid='" & uuid & "' and apptoken='" & appToken & "'"
            oledbCommand.ExecuteNonQuery()
            oledbCommand.CommandText = "drop table " & appToken
            oledbCommand.ExecuteNonQuery()
        End If

    End Sub

    Public Function IsMyApp(uuid As String, appToken As String) As Boolean
        Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()

        oledbCommand.CommandText = "select * from appindex where uuid='" & uuid & "' and apptoken='" & appToken & "'"
        Dim reader As MySqlDataReader = oledbCommand.ExecuteReader()
        If reader.HasRows Then
            reader.Close()
            Return True
        End If
        reader.Close()
        Return False
    End Function

    Public Function GetAppHtmlListbyUUID(uuid As String) As List(Of String)
        Dim l As New List(Of String)
        Dim apptokens As New List(Of String)
        Dim oledbCommand As MySqlCommand = AppInfoConnection.CreateCommand()
        oledbCommand.CommandText = "select apptoken from appindex where uuid='" & uuid & "'"
        Dim reader As MySqlDataReader = oledbCommand.ExecuteReader()
        If reader.HasRows Then
            While reader.Read
                If Not IsDBNull(reader("apptoken")) Then
                    apptokens.Add(reader("apptoken"))
                End If
            End While
        End If

        reader.Close()
        For Each apptoken As String In apptokens
            l.Add(addMyAPP(GetAppProperty(apptoken, "appname", True), GetAppProperty(apptoken, "publictoken", True), GetAppProperty(apptoken, "privatetoken", True), GetAppProperty(apptoken, "required", True), If(GetAppProperty(apptoken, "mode", True) = "0", "<div class=""badge badge-success"">公有", "<div class=""badge badge-warning"">私有")))
        Next
        apptokens.Clear()

        Return l
    End Function

    Public Function VerifyTokenAndGetUserUUID(token As String, rip As String) As String
        Dim reader As MySqlDataReader
        Try
            If token IsNot Nothing AndAlso token.Trim <> "" AndAlso
                rip IsNot Nothing AndAlso rip.Trim <> "" Then

                Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
                oledbCommand.CommandText = "Select username,createtime,loginip from tokens where token='" & SHA512(token) & "'"
                reader = oledbCommand.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    Dim un As String = reader("username")
                    Dim ctime As Date = reader("createtime")
                    If rip <> reader("loginip") Then
                        reader.Close()
                        Return Nothing
                    End If
                    If Math.Abs((Now - ctime).Hours) < 24 Then
                        oledbCommand.CommandText = "select useruuid,state from userinfo where username='" & un & "'"
                        reader.Close()
                        reader = Nothing
                        reader = oledbCommand.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()
                            If Not IsDBNull(reader("state")) AndAlso CInt(reader("state")) = 1 Then
                                Dim retuuid As String = reader("useruuid")
                                reader.Close()
                                Return retuuid
                            End If
                        End If
                    End If
                End If
                reader.Close()

            End If
        Catch
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Function VerifyOpKey(key As String) As (Boolean, String, String)
        Dim reader As MySqlDataReader
        Try
            If key IsNot Nothing AndAlso
                key.Trim <> "" Then
                Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
                oledbCommand.CommandText = "select username,useruuid from userinfo where opkey='" & SHA512(key) & "'"
                reader = oledbCommand.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    Dim uemail As String = reader("username")
                    Dim uuuid As String = reader("useruuid")
                    reader.Close()
                    If uemail IsNot Nothing AndAlso uuuid IsNot Nothing Then
                        Return (True, uemail, uuuid)
                    End If
                End If

                reader.Close()
            End If
        Catch
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
        Return (False, Nothing, Nothing)
    End Function

    Public Sub DeleteUserAccount(dirPath As String, uuid As String, email As String)


        Try
            FileIO.FileSystem.DeleteDirectory(dirPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch
        End Try
        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()
        oledbCommand.CommandText = "delete from userinfo where useruuid='" & uuid & "'"
        oledbCommand.ExecuteNonQuery()
        oledbCommand.CommandText = "delete from tokens where username='" & GetBase64FromStringAsUtf8Format(email) & "'"
        oledbCommand.ExecuteNonQuery()
    End Sub

    Public Sub UserLogOff(token As String)

        Dim oledbCommand As MySqlCommand = UserInfoConnection.CreateCommand()

        oledbCommand.CommandText = "delete from tokens where token='" & token & "'"
        oledbCommand.ExecuteNonQuery()

    End Sub
End Module
