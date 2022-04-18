Imports System.Data.OleDb
Imports System.IO

Public Class active
    Inherits System.Web.UI.Page

    Public redirect As String = "default.aspx"
    Private useruuid As String
    Private useremail As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.Request.Cookies("token") IsNot Nothing Then
            If VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress) IsNot Nothing Then
                Response.Redirect("default.aspx")
                Return
            End If
        End If

        Dim key As String = Page.Request.Item("key")
        If key Is Nothing Then
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "ErrorRedirect(""激活码无效"")", True)
        Else
            Dim result As (Boolean, String, String) = VerifyOpKey(key)
            If result.Item1 = False Then
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "ErrorRedirect(""激活码无效"")", True)
            Else
                useremail = GetStringAsUtf8FormatFromBase64(result.Item2)
                useruuid = result.Item3
                Panel_act.Visible = True
                userid.Text = "ID:" & useruuid.Substring(5, 10)
            End If
        End If
    End Sub

    Public Sub active_click()
        If input_email.Text = useremail Then
            Try
                ActiveUserAccount(useruuid)
                setUserConfig(useruuid, "nickname", input_username.Text)
                setUserConfig(useruuid, "email", input_email.Text)
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "SuccessRedirect(""激活成功"")", True)
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""" & ex.Message & """)", True)
            End Try
        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""邮箱不匹配"")", True)
        End If
    End Sub

End Class