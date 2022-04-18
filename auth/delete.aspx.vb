Imports System.Data.OleDb

Public Class delete
    Inherits System.Web.UI.Page

    Private uuid As String
    Public redirect As String = "default.aspx"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.Request.Cookies("token") IsNot Nothing Then
            uuid = VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress)
            If uuid IsNot Nothing Then
                Exit Sub
            End If
        End If
        Response.Redirect("default.aspx")
    End Sub

    Public Sub delete_click()
        If SHA512(input_email.Text) = "" And input_username.Text = uuid.Substring(5, 10) Then
            Try
                DeleteUserAccount(MapPath("") + "/userdata/" + uuid, uuid, input_email.Text)
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "SuccessRedirect(""销户完成"")", True)
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""" & ex.Message & """)", True)
            End Try
        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "Error(""邮箱与UID不匹配"")", True)
        End If
    End Sub

End Class