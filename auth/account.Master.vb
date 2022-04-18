Public Class account1
    Inherits System.Web.UI.MasterPage

    Public uuid As String
    Public nackname As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.Request.Cookies("token") IsNot Nothing Then
            uuid = VerifyTokenAndGetUserUUID(Page.Request.Cookies("token").Value, Request.UserHostAddress)
            If uuid IsNot Nothing Then
                nackname = getUserConfig(uuid, "nickname")
                If IO.File.Exists(MapPath("") & "/profileimage/" & uuid & ".png") Then
                    Image1.ImageUrl = "profileimage/" & uuid & ".png"
                Else
                    Image1.ImageUrl = "dashboard_res/avatar-1.png"
                End If
                Exit Sub
            End If
        End If
        Response.Redirect("default.aspx")
    End Sub

End Class