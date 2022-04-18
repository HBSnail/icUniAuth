Public Class account
    Inherits System.Web.UI.Page

    Public tabledata As String
    Public uuid As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        uuid = CType(Master, account1).uuid
        Dim authinfos() As String = GetAllAuthInfo(uuid)
        tabledata = ""
        For i = 0 To authinfos.Length - 2
            Dim apptoken As String = authinfos(i).Split(",")(0)
            Dim authinfo As String = authinfos(i).Split(",")(1)
            Dim authtime As String = authinfos(i).Split(",")(2)
            tabledata &= createTableDat(GetAppProperty(apptoken, "appname", True), apptoken, authinfo, authtime)
        Next
    End Sub

    Function createTableDat(appname As String, token As String, info As String, time As String) As String

        Return "<tr><td><a href=""#"">" & appname & "</a></td><td class=""font-weight-600"">" & token & "</td><td><div class=""badge badge-success"">" & info & "</div></td><td>" & time & "</td><td><a href=""/deauth.aspx?apptoken=" & Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(token)) & """ class=""btn btn-danger"">撤销授权</a></td></tr>"
    End Function


End Class