Public Class logoff
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim redirect As String
        Try
            redirect = Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request.Item("redirect_url")))
        Catch
        End Try
        Dim respCookie As New HttpCookie("")
        respCookie.Item("token") = ""
        Response.Cookies.Add(respCookie)

        If Page.Request.Cookies("token") IsNot Nothing AndAlso
            Page.Request.Cookies("token").Value IsNot Nothing AndAlso
            Page.Request.Cookies("token").Value.Trim <> "" Then

            UserLogOff(Page.Request.Cookies("token").Value)
        End If

        If redirect Is Nothing Then
            Response.Redirect("default.aspx")
        Else
            Response.Redirect(redirect)
        End If
    End Sub

End Class