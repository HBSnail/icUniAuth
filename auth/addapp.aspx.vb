Public Class addapp
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Protected Sub btn_submit_Click(sender As Object, e As EventArgs) Handles btn_submit.Click
        If input_appname.Text.Trim <> "" AndAlso redirect.Text.Trim <> "" Then
            Dim uuid As String = CType(Master, account1).uuid
            Dim apptoken As String = GetRandomString(32)
            Dim appprivatetoken As String = GetRandomString(32)
            While (appprivatetoken = apptoken)
                appprivatetoken = GetRandomString(32)
            End While
            Dim req As String = ""

            If required.Items(0).Selected Then
                req += "1"
            Else
                req += "0"
            End If
            If required.Items(1).Selected Then
                req += "1"
            Else
                req += "0"
            End If

            If RegisterApp(input_appname.Text, redirect.Text.Trim, apptoken, appprivatetoken, req, app_calss.SelectedIndex, uuid) Then
                Response.Redirect("myapp.aspx")
            Else
                Page.ClientScript.RegisterStartupScript(Page.GetType, "", "alert(""APP注册失败（每个用户最多拥有5个APP且不能与现有APP重名）"")", True)
            End If

        Else
            Page.ClientScript.RegisterStartupScript(Page.GetType, "", "alert(""请完整填写内容"")", True)
        End If
    End Sub

End Class