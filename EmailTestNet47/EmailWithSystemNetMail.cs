using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailTestNet47
{
    public class EmailWithSystemNetMail
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _pwd;
        private readonly int _port;
        private readonly bool _ssl;
        private readonly string _logFile;
        private string _testVersion;


        public EmailWithSystemNetMail(string host, string username, string pwd, int port, bool ssl, string logFile)
        {
            _host = host;
            _username = username;
            _pwd = pwd;
            _port = port;
            _ssl = ssl;
            _logFile = logFile;
            _testVersion = $".net 4.7.2 with System.Net.Mail, host: {_host}";
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
                    smtp.Port = _port;
                    smtp.Host = _host;
                    if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_pwd))
                        smtp.Credentials = new System.Net.NetworkCredential(_username, _pwd);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = _ssl;

                    for (i = 0; i < nbrOfEmails; i++)
                    {
                        using (MailMessage mm = new MailMessage(_username, destEmail))
                        {
                            mm.Subject = $"System.Net.Mail .net 4 performance test: {DateTime.Now.ToString("HH:mm:ss.ff")}";
                            mm.Body = "This is a <b>test</b>";
                            mm.IsBodyHtml = true;
                            await smtp.SendMailAsync(mm);
                        }
                    }
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
                    smtp.Port = _port;
                    smtp.Host = _host; 
                    if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_pwd))
                        smtp.Credentials = new System.Net.NetworkCredential(_username, _pwd);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = _ssl;

                    for (i = 0; i < nbrOfEmails; i++)
                    {
                        using (MailMessage mm = new MailMessage(_username, destEmail))
                        {
                            mm.Subject = $"System.Net.Mail performance test: {DateTime.Now.ToString("HH:mm:ss.ff")}";
                            mm.Body = "This is a <b>test</b>";
                            mm.IsBodyHtml = true;
                            smtp.Send(mm);
                        }
                    }
                }

                DateTime currTime2 = DateTime.Now;
                TimeSpan ts = currTime2 - currTime1;

                sw.WriteLine($"{currTime2.ToString("HH:mm:ss.ff")} End ({_testVersion}) ---Sync {i} emails sent. Elapse time: {ts.TotalSeconds} seconds");

            }

        }

    }
}
