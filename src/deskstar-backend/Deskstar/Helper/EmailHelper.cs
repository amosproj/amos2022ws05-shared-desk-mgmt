/**
 * Program
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */
using System.Net.Mail;

namespace Deskstar.Helper;

public class EmailHelper
{
    public static bool SendEmail(ILogger logger, string userEmail, string subject, string message)
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("noreply@deskstar.de");
        mailMessage.To.Add(new MailAddress(userEmail));

        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = message;

        var client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential("login", "psw");
        client.Host = "host";
        client.Port = int.Parse("port");
        client.EnableSsl = true;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;


        try
        {
            client.Send(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        return false;
    }

}