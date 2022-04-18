Imports MySql.Data.MySqlClient

Module DatabaseConnections

    Public UserInfoConnection As New MySqlConnection(My.Settings.UserInfoConnectStr)

    Public AuthInfoConnection As New MySqlConnection(My.Settings.AuthInfoConnectStr)

    Public AppInfoConnection As New MySqlConnection(My.Settings.AppInfoConnectStr)
End Module
