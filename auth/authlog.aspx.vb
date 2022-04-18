Public Class authlog
    Inherits System.Web.UI.Page

    Public tabledata As String
    Public uuid As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        uuid = CType(Master, account1).uuid

        tabledata = getUserConfig(uuid, "authlog")
    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        uuid = CType(Master, account1).uuid
        SetUserConfig(uuid, "authlog", "")
    End Sub
End Class