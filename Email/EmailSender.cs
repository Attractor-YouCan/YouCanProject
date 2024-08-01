public class EmailSender
{
    public bool SendEmail(string email, string subject, string text)
    {
        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            
            System.Net.ServicePointManager.ServerCertificateValidationCallback = 
                delegate { return true; };
    
            string server = "smtp.gmail.com";
            int port = 587; 
            bool enableSsl = true;
    
            string from = "satoshinaemail@gmail.com";
            string password = "rwdb saoe gqfv gpvi";
            string to = email;
    
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("YouCan", from));
            message.To.Add(new MimeKit.MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new MimeKit.TextPart("html")
            {
                Text = text
            };
    
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(server, port, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
    
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }


}
