using EmailTest;
using MailKit.Security;

try
{
  
    string host = "smtp.ethereal.email";
    string username = "brando.johnston@ethereal.email";
    string password = "HyFYHtAeprK7se94bT";
    int port = 587;
    bool ssl = true;
    SecureSocketOptions sso = SecureSocketOptions.StartTls;
    string destEmail = "yogi1002@mailinator.com";
    string logFile = @"C:\Temp\EmailTestLog.txt";

    EmailWithMailKit emailMK = new EmailWithMailKit(host, username, password, port, sso, logFile);

    await emailMK.SendEmailsAsync(destEmail, 9);

    emailMK.SendEmails(destEmail, 9);

    EmailWithSystemNetMail emailSNM = new EmailWithSystemNetMail(host, username, password, port, ssl, logFile);
    
    await emailSNM.SendEmailsAsync(destEmail, 9);

    emailSNM.SendEmails(destEmail, 9);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

