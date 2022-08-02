using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailTest;

public class EmailWithMailKit
{
    private readonly string _host;
    private readonly string _username;
    private readonly string _pwd;
    private readonly int _port;
    private readonly SecureSocketOptions _sso;
    private readonly string _logFile;
    private string _testVersion;

    public EmailWithMailKit(string host, string username, string pwd, int port, SecureSocketOptions sso, string logFile)
    {
        _host = host;
        _username = username;
        _pwd = pwd;
        _port = port;
        _sso = sso;
        _logFile = logFile;
        _testVersion = $".net 6 with MailKit, host: {_host}";
    }

    public async Task SendEmailsAsync(string destEmail, int nbrOfEmails)
    {
        DateTime currTime1 = DateTime.Now;
        using (StreamWriter sw = new StreamWriter(_logFile, true))
        {
            sw.WriteLine($"{currTime1.ToString("HH:mm:ss.ff")} Start ({_testVersion}) ---Async");

            int i = 0;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Connect(_host, _port, _sso);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_pwd))
                    smtp.Authenticate(_username, _pwd);
                smtp.Capabilities &= ~(SmtpCapabilities.Size | SmtpCapabilities.Chunking);

                for (i = 0; i < nbrOfEmails; i++)
                {
                    using (MimeMessage mm = new MimeMessage())
                    {
                        mm.From.Add(MailboxAddress.Parse(_username));
                        mm.To.Add(MailboxAddress.Parse(destEmail));
                        mm.Subject = $"MailKit performance test: {DateTime.Now.ToString("HH:mm:ss.ff")}";
                        mm.Body = new TextPart(TextFormat.Html) { Text = "This is a <b>test</b>", ContentTransferEncoding = ContentEncoding.SevenBit };
                        await smtp.SendAsync(mm);
                    }
                }

                smtp.Disconnect(true);
            }
            DateTime currTime2 = DateTime.Now;
            TimeSpan ts = currTime2 - currTime1;

            sw.WriteLine($"{currTime2.ToString("HH:mm:ss.ff")} End ({_testVersion}) ---Async {i} emails sent. Elapse time: {ts.TotalSeconds} seconds");
        }
    }

    public void SendEmails(string destEmail, int nbrOfEmails)
    {
        DateTime currTime1 = DateTime.Now;
        using (StreamWriter sw = new StreamWriter(_logFile, true))
        {
            sw.WriteLine($"{currTime1.ToString("HH:mm:ss.ff")} Start ({_testVersion}) ---Sync");

            int i = 0;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Connect(_host, _port, _sso);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_pwd))
                    smtp.Authenticate(_username, _pwd);
                smtp.Capabilities &= ~(SmtpCapabilities.Size | SmtpCapabilities.Chunking);

                for (i = 0; i < nbrOfEmails; i++)
                {
                    using (MimeMessage mm = new MimeMessage())
                    {
                        mm.From.Add(MailboxAddress.Parse(_username));
                        mm.To.Add(MailboxAddress.Parse(destEmail));
                        mm.Subject = $"MailKit performance test: {DateTime.Now.ToString("HH:mm:ss.ff")}";
                        mm.Body = new TextPart(TextFormat.Html) { Text = "This is a <b>test</b>", ContentTransferEncoding = ContentEncoding.SevenBit };
                        smtp.Send(mm);
                    }
                }

                smtp.Disconnect(true);
            }
            DateTime currTime2 = DateTime.Now;
            TimeSpan ts = currTime2 - currTime1;

            sw.WriteLine($"{currTime2.ToString("HH:mm:ss.ff")} End ({_testVersion}) ---Sync {i} emails sent. Elapse time: {ts.TotalSeconds} seconds");
        }
    }

}
