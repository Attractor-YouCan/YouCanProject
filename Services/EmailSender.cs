
public class EmailSender
{
    public void SendEmail(string email, string subject, string text)

    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        string server = "smtp.gmail.com"; 
        int port = 465; 
        bool enableSsl = true; 

        string from = "satoshinaemail@gmail.com"; 
        string password = "rwdb saoe gqfv gpvi"; 
        string to = email; 

        var message = new MimeKit.MimeMessage();
        message.From.Add(new MimeKit.MailboxAddress("YouCan", from)); 
        message.To.Add(new MimeKit.MailboxAddress("satoshi", to));
        message.Subject = subject;
        message.Body = new MimeKit.TextPart("html")
        {
            Text = text
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect(server, port, enableSsl);
            client.Authenticate(from, password);
            client.Send(message);
            client.Disconnect(true);
        }

    }
}