Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        UserInfoConnection.Open()
        AppInfoConnection.Open()
        AuthInfoConnection.Open()
    End Sub

    Sub Application_End(sender As Object, e As EventArgs)
        UserInfoConnection.Close()
        AppInfoConnection.Close()
        AuthInfoConnection.Close()
    End Sub
End Class