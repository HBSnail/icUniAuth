Public Class deleteapp
    Inherits System.Web.UI.Page

    Private uuid As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.Request.Cookies("token") IsNot Nothing Then
            uuid = VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress)
            If uuid IsNot Nothing Then
                DeleteMyApp(uuid, Request.Item("apptoken"))
            End If
        End If
        Response.Redirect("myapp.aspx")
    End Sub

End Class