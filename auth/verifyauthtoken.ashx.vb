Imports System.IO
Imports System.Runtime.Serialization.Json
Imports System.Xml

Public Class verifyauthtoken
    Implements System.Web.IHttpHandler

    Public Class VerifyState
        Public AuthState As String
        Public UserInfo As New Dictionary(Of String, String)
    End Class
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest


        Try
            context.Response.ContentType = "application/json"
            Dim l As New VerifyState
            Dim vresp As String = VerifyAuthRequestToken(context.Request.Item("auth_token"), context.Request.Item("private_token"))
            l.AuthState = vresp.Split("@")(0)
            If vresp Like "OK@*" Then
                Dim uinfo As String = vresp.Split("@")(1)
                SetUserConfig(vresp.Split("@")(2), "authlog", getUserConfig(vresp.Split("@")(2), "authlog") + createTableDat(MD5(context.Request.Item("auth_token")), "令牌验证", "验证成功"， Now.ToString))


                For i = 0 To uinfo.Length - 1
                    If uinfo(i) = "1" Then
                        Select Case i
                            Case 0
                                l.UserInfo.Add("nickname", getUserConfig(vresp.Split("@")(2), "nickname")) '&= "昵称、头像<br>"
                            Case 1
                                l.UserInfo.Add("email", getUserConfig(vresp.Split("@")(2), "email")) ' &= "电子邮箱<br>"
                            Case Else

                        End Select
                    End If
                Next
            Else
            End If
            Dim dcjson As New DataContractJsonSerializer(l.GetType)
            Dim stream As New MemoryStream
            dcjson.WriteObject(stream, l)
            stream.Position = 0
            Dim sr As New StreamReader(stream)
            context.Response.Write(sr.ReadToEnd())
        Catch ex As Exception
            context.Response.ContentType = "application/text"
            context.Response.Write(ex.Message)
        End Try

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class