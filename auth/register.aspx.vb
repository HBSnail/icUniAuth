Imports System.Data.OleDb

Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.Request.Cookies("token") IsNot Nothing Then
            If VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress) IsNot Nothing Then
                Response.Redirect("default.aspx")
            End If
        End If
    End Sub
    Public Sub reg_click()
        If input_username.Text <> "" AndAlso input_password.Text <> "" AndAlso input_repassword.Text <> "" Then
            If input_password.Text = input_repassword.Text Then
                Try
                    Dim activeKey As String = RegisterUser(input_username.Text, input_password.Text)
                    SendEmail(input_username.Text,
                          "<a href=""" & Request.Url.Scheme & "://" & Request.Url.Authority & "/active.aspx?key=" & activekey & """>激活</a>")
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Success(""处理完成，请前往邮箱确认"",""default.aspx"")", True)
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""" & ex.Message.Replace("""", "").Replace(vbCr, " ").Replace(vbLf, " ") & """)", True)
                End Try
            Else
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""两次输入的密码不匹配"")", True)
            End If
        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""请填写注册信息"")", True)
        End If

    End Sub
End Class