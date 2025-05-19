using MailKit.Net.Smtp;
using MimeKit;
using Models;

public class EmailService() {
    public async Task SendAsync(Email email)
    {
        // create message
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("PotPal", Config.SMTP_ADDRESS));
        message.To.Add(new MailboxAddress("", email.Recipient));
        message.Subject = email.Subject;
        message.Body = new TextPart("HTML") { Text = email.Body };

        // send message
        using (var smtpClient = new SmtpClient())
        {
            try
            {
                await smtpClient.ConnectAsync(Config.SMTP_ADDRESS, int.Parse(Config.SMTP_PORT), true);
                await smtpClient.AuthenticateAsync(Config.SMTP_USER, Config.SMTP_PASSWORD);

                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email " + ex.Message);
            }
        }
    }
}
