Public Class deauth
    Inherits System.Web.UI.Page
    Public tabledata As String
    Public uuid As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.Request.Cookies("token") IsNot Nothing Then
                uuid = VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress)
                If uuid IsNot Nothing Then
                    Dim apptoken As String = Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request.Item("apptoken")))
                    DeleteAuthInfo(uuid, apptoken)

                    SetUserConfig(uuid, "authlog", getUserConfig(uuid, "authlog") + createTableDat(GetAppProperty(apptoken, "appname", True), "授权撤销", "撤销授权", Now.ToString, "warning"))


                End If
            End If
        Catch
        End Try
        Response.Redirect("account.aspx")
    End Sub

End Class