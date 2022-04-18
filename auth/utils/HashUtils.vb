Imports System.Security.Cryptography

Public Module HashUtils

    Public Function MD5(data As String) As String
        Dim SHA As MD5 = System.Security.Cryptography.MD5.Create()
        Dim bytHash() As Byte
        bytHash = SHA.ComputeHash(Text.Encoding.UTF8.GetBytes(data))
        SHA.Clear()
        Return Bytes2String(bytHash).ToUpper
    End Function
    Public Function SHA512(data As String) As String
        Dim SHA As SHA512 = System.Security.Cryptography.SHA512.Create()
        Dim bytHash() As Byte
        bytHash = SHA.ComputeHash(Text.Encoding.UTF8.GetBytes(data))
        SHA.Clear()
        Return Bytes2String(bytHash).ToUpper
    End Function


End Module
