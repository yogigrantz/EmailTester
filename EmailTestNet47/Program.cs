using System;
using System.Threading.Tasks;

namespace EmailTestNet47
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Waiting a minute to let the .net 6 project finish up");
            System.Threading.Thread.Sleep(60000); // Allowing the other project to finish up

            string host = "smtp.ethereal.email";
            string username = "brando.johnston@ethereal.email";
            string password = "HyFYHtAeprK7se94bT";
            int port = 587;
            bool ssl = true;

            string destEmail = "yogi1002@mailinator.com";
            string logFile = @"C:\Temp\EmailTestLog.txt";

            EmailWithSystemNetMail emailSNM = new EmailWithSystemNetMail(host, username, password, port, ssl, logFile);

            await emailSNM.SendEmailsAsync(destEmail, 9);

            emailSNM.SendEmails(destEmail, 9);

        }
    }
}
