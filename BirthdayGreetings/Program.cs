﻿using System;
using System.Globalization;
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

            DateTime now = DateTime.Now;

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
                    var birthday =
                        DateTime.ParseExact(
                            tokens[2],
                            "yyyy/MM/dd",
                            CultureInfo.InvariantCulture);
                    if (birthday.Month == now.Month && birthday.Day == now.Day)
                    {
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
}
