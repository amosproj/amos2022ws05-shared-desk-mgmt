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
  private static string? _emailPassword;
  private static string? _emailHost;
  private static int _emailPort;
  private static string? _emailUsername;
  private static SmtpClient? _smtpClient;

  private const string Footer = "Please note: This is an automatic email notification. Do not reply to this email. If this email contains time information, the used timezone is the ISO Norm Timezone.<br/> " +
                                "<br/> " +
                                "Regards, <br/> " +
                                "<br/>" +
                                "Deskstar Team";

  public static bool SendEmail(ILogger logger, string userEmail, string subject, string message)
  {
    if (_smtpClient == null)
    {
      if (_emailPassword == null || _emailHost == null || _emailUsername == null)
      {
        logger.LogError("Email credentials not set");
        return false;
      }

      _smtpClient = new SmtpClient(_emailHost, _emailPort);
      _smtpClient.Credentials = new System.Net.NetworkCredential(_emailUsername, _emailPassword);
      _smtpClient.EnableSsl = true;
      _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
      _smtpClient.UseDefaultCredentials = false;
    }

    if (userEmail.Contains("@acme.com") || userEmail.Contains("@sola.com"))
    {
      return true;
    }

    var mailMessage = new MailMessage();
    mailMessage.From = new MailAddress(_emailUsername!);
    mailMessage.To.Add(new MailAddress(userEmail));

    mailMessage.Subject = subject;
    mailMessage.IsBodyHtml = true;
    mailMessage.Body = message + Footer;

    try
    {
      _smtpClient.Send(mailMessage);
      return true;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, ex.Message);
    }

    return false;
  }

  public static void SetMailPassword(string? password)
  {
    _emailPassword = password;
  }

  public static void SetMailHost(string host)
  {
    _emailHost = host;
  }

  public static void SetMailPort(int port)
  {
    _emailPort = port;
  }

  public static void SetMailUsername(string username)
  {
    _emailUsername = username;
  }
}
