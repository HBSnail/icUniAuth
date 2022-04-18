Imports System.Data.OleDb

Public Class Login
    Inherits System.Web.UI.Page

    Public redirect As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            redirect = Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request.Item("redirect_url")))
        Catch
        End Try
        If redirect Is Nothing Then
            redirect = "account.aspx"
        End If
        If Page.Request.Cookies("token") IsNot Nothing Then
            If VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress) IsNot Nothing Then
                Response.Redirect(redirect)
            End If
        End If
    End Sub

    Public Sub login_click()

        If input_username.Text <> "" AndAlso input_password.Text <> "" Then
            Try
                Dim cookie As String = CheckLoginAndGetToken(input_username.Text, input_password.Text, Request.UserHostAddress)
                Dim respCookie As New HttpCookie("")
                respCookie.Item("token") = cookie
                respCookie.Expires = Now.AddHours(24)
                Response.Cookies.Add(respCookie)
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Success(""登录成功"")", True)
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""" & ex.Message.Replace("""", "").Replace(vbCr, " ").Replace(vbLf, " ") & """)", True)
            End Try
        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""请输入用户名和密码"")", True)
        End If


    End Sub
End Class