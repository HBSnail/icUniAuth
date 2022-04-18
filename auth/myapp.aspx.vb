Public Class myapp
    Inherits System.Web.UI.Page

    Public x As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Dim uuid As String = CType(Master, account1).uuid
        x = ""
        Dim applist As List(Of String) = GetAppHtmlListbyUUID(uuid)
        For i = 0 To applist.Count - 1
            x &= applist(i)
        Next
    End Sub


End Class