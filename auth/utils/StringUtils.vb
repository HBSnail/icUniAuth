Module StringUtils

    Public Function GetRandomString(length As Integer) As String
        Dim r As Random = New Random()
        Dim s As String = Nothing, str As String = ""
        str += "0123456789"
        str += "abcdefghijklmnopqrstuvwxyz"
        str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        For i As Integer = 0 To length - 1
            s += str.Substring(r.[Next](0, str.Length - 1), 1)
        Next
        Threading.Thread.Sleep(1)
        Return s
    End Function

    Public Function Bytes2String(A As Byte()) As String
        Dim sb As StringBuilder = New StringBuilder()
        For i As Integer = 0 To A.Length - 1
            sb.Append(A(i).ToString("x2"))
        Next

        Return sb.ToString()
    End Function

    Public Function GetBase64FromStringAsUtf8Format(input As String) As String
        Return Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(input))
    End Function

    Public Function GetStringAsUtf8FormatFromBase64(input As String) As String
        Return Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
    End Function

    Public Function GetHexFromStringAsUtf8Format(input As String) As String
        Return Bytes2String(Text.Encoding.UTF8.GetBytes(input))
    End Function

    Public Function VerifyHexString(s As String) As Boolean
        If s Is Nothing Then
            Return False
        End If
        If (s.Length And 1) = 1 Then
            Return False
        End If
        If s.Trim = "" Then
            Return False
        End If
        For i = 0 To s.Length - 1
            If (s(i) >= "0" And s(i) <= "9") OrElse (s(i) >= "A" And s(i) <= "Z") OrElse (s(i) >= "a" And s(i) <= "z") Then
            Else
                Return False
            End If
        Next
        Return True
    End Function
End Module
