using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading;
using HP.ScalableTest;
using HP.ScalableTest.FileAnalysis;

namespace CorruptFileNotifier
{
    class Program
    {
        private const string _smtpHost = "smtp3.hp.com";
        private const string _doNotReply = "donotreply@hp.com";

        static void Main(string[] args)
        {
            var monitorLocations = GetAppSettingList("monitorLocations");
            string fileMask = ConfigurationManager.AppSettings["fileMask"];

            // Set up directory file watchers
            foreach (string location in monitorLocations)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(location);
                watcher.Filter = fileMask;
                watcher.Created += new FileSystemEventHandler(watcher_FoundFile);
                watcher.Renamed += new RenamedEventHandler(watcher_FoundFile);
                watcher.EnableRaisingEvents = true;
            }

            // Hold the program open
            Console.ReadKey();
        }

        private static void watcher_FoundFile(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Console.WriteLine("Found file: " + e.FullPath);
            ValidationResult result = FileAnalyzerFactory.Create(e.FullPath).Validate();
            if (result.Success == false)
            {
                DoNotifications(e.FullPath, result.Result);
            }
        }

        private static void DoNotifications(string path, string error)
        {
            Console.WriteLine("Corrupt file: " + error);
            SendEmailNotifications(path, error);
        }

        private static void SendEmailNotifications(string path, string error)
        {
            IEnumerable<string> emails = GetAppSettingList("emails");
            
            using (SmtpClient client = new SmtpClient(_smtpHost))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(_doNotReply, "Corrupt File Notifier");
                    foreach (string email in emails)
                    {
                        message.To.Add(new MailAddress(email));
                    }
                    message.Subject = "Corrupt File Detected";
                    message.Body = GetMessage(path, error);
                    client.Send(message);
                }
            }
        }

        private static string GetMessage(string path, string error)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("A corrupt file has been found on {0}.".FormatWith(Environment.MachineName));
            builder.AppendLine();
            builder.AppendLine("Name:  " + Path.GetFileName(path));
            builder.AppendLine("Path:  " + path);
            builder.AppendLine("Error:  " + error);
            return builder.ToString();
        }

        private static IEnumerable<string> GetAppSettingList(string key)
        {
            List<string> results = new List<string>();
            foreach (string result in ConfigurationManager.AppSettings[key].Split(';', ',', '\n', '\r'))
            {
                if (!string.IsNullOrWhiteSpace(result))
                {
                    results.Add(result.Trim());
                }
            }
            return results;
        }
    }
}
