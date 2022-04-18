Imports DnsClient

Module NetUtils
    Public Sub SendEmail(toAddr As String, bodyhtml As String)
        Dim addr As New Net.Mail.MailAddress(toAddr)
        Dim dnsc As New LookupClient()
        Dim SmtpClient As New Net.Mail.SmtpClient(DirectCast(dnsc.Query(addr.Host, QueryType.MX, QueryClass.IN).Answers.Item(0), DnsClient.Protocol.MxRecord).Exchange.Value, 25)
        Dim mail As New Net.Mail.MailMessage()
        mail.Subject = "icUniAuth注册确认邮件"
        mail.From = New Net.Mail.MailAddress(My.Settings.MailAddress, "icUniAuth账号事务局")
        mail.Priority = Net.Mail.MailPriority.Normal
        mail.IsBodyHtml = True
        mail.Body = bodyhtml
        mail.To.Add(addr)
        SmtpClient.Send(mail)
    End Sub

End Module
