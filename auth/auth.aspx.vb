Public Class auth
    Inherits System.Web.UI.Page

    Public uuid As String
    Public redirect As String
    Private apptoken As String
    Public appname As String
    Dim reqinfo As String
    Dim resptoken As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        resptoken = MD5(GetRandomString(50))

        apptoken = Request.Item("app_token")
        appname = GetAppProperty(apptoken, "appname", False)
        If apptoken Is Nothing OrElse appname Is Nothing Then
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "ErrorRedirect(""App令牌不正确"")", True)
            redirect = "default.aspx"
            Exit Sub
        End If
        Try
            redirect = GetAppProperty(apptoken, "redirect", False)

            If InStr(redirect, "?") Then
                redirect &= "&resp_token=" & resptoken
            Else
                redirect &= "?resp_token=" & resptoken
            End If
            Dim additional As String = Request.Item("args")
            If additional IsNot Nothing Then
                redirect &= GetStringAsUtf8FormatFromBase64(additional)
            End If

        Catch
        End Try
        If redirect Is Nothing Then
            redirect = "default.aspx"
        End If



        If Page.Request.Cookies("token") IsNot Nothing Then
            uuid = VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress)
            If uuid IsNot Nothing Then

                Dim mode As String = GetAppProperty(apptoken, "mode", False)

                If mode = 1 Then

                    'private app
                    Dim allowed As Boolean = False

                    Dim usersInfo As String = GetAppProperty(apptoken, "allowuser", False)

                    If IsMyApp(uuid, MD5(apptoken)) OrElse InStr(usersInfo & ";", uuid & ";") Then
                        allowed = True
                    End If

                    If Not allowed Then
                        SetUserConfig(uuid, "authlog", getUserConfig(uuid, "authlog") + createTableDat(appname, "授权请求", "权限不足"， Now.ToString, "danger"))

                        Page.ClientScript.RegisterStartupScript(Page.GetType, "", "ErrorRedirect(""您无权向该APP授权"")", True)
                        Exit Sub
                    End If
                End If

                Dim nickname As String = getUserConfig(uuid, "nickname")
                userid.Text = "允许将" & nickname & "的以下信息授权给:<br>" & appname
                reqinfo = GetAppProperty(apptoken, "required", False)
                Dim authed As String = GetAuthInfo(uuid, apptoken, False)
                If authed IsNot Nothing AndAlso authed = reqinfo Then
                    SetUserConfig(uuid, "authlog", getUserConfig(uuid, "authlog") + createTableDat(appname & "<br>" & MD5(resptoken), "授权请求", "自动授权"， Now.ToString))

                    RegRespToken(uuid, resptoken, apptoken)
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "", "SuccessRedirect(""自动授权成功"")", True)
                    Exit Sub
                End If

                Dim retshowstring As String = ""
                For i = 0 To reqinfo.Length - 1
                    If reqinfo(i) = "1" Then
                        Select Case i
                            Case 0
                                retshowstring &= "昵称、头像<br>"
                            Case 1
                                retshowstring &= "电子邮箱<br>"
                            Case Else

                        End Select
                    End If
                Next
                userid0.Text = retshowstring
                Panel_act.Visible = True
                Exit Sub
            End If
        End If
        Response.Redirect("login.aspx?redirect_url=" & Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(Request.Url.AbsoluteUri)))
    End Sub



    Public Sub act_click()
        Dim authed As String = GetAuthInfo(uuid, apptoken, False)
        If authed IsNot Nothing AndAlso authed = reqinfo Then
            Exit Sub
        End If
        SetUserConfig(uuid, "authlog", getUserConfig(uuid, "authlog") + createTableDat(appname & "<br>" & MD5(resptoken), "授权请求", "手动授权"， Now.ToString))


        AddauthInfo(uuid, apptoken, reqinfo)
        RegRespToken(uuid, resptoken, apptoken)
        Page.ClientScript.RegisterStartupScript(Page.GetType, "", "SuccessRedirect(""授权成功"")", True)
    End Sub
    Public Sub notact_click()
        SetUserConfig(uuid, "authlog", getUserConfig(uuid, "authlog") + createTableDat(appname, "授权请求", "拒绝授权"， Now.ToString, "danger"))

        Page.ClientScript.RegisterStartupScript(Page.GetType, "", "ErrorRedirect(""授权已拒绝"")", True)
    End Sub
End Class