using System;
using System.IO;
using System.Net.Mail;

namespace BirthdayGreetings
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = args[0];
            int port = int.Parse(args[1]);
            string dataPath = args[2];

            using (FileStream file = File.Open(dataPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(file))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(
                        new string[] { ", " },
                        StringSplitOptions.None);
                    string firstName = tokens[1];                    
                    string email = tokens[3];
                    using (SmtpClient smtpClient = new SmtpClient(hostname, port))
                    {
                        var subject = "Happy birthday!";
                        var body = string.Format("Happy birthday, dear {0}!", firstName);
                        var message = new MailMessage("noreply@noname", email, subject, body);
                        smtpClient.Send(message);
                    }
                }
            }
        }
    }
}
