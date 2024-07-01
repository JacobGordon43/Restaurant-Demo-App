using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using MailKit.Security;
using NuGet.Protocol;
using Restaurant_Demo_App.Models;

namespace Restaurant_Demo_App.Services
{
    public class EmailService
    {
        public async Task<bool> SendEmail(EmailModel email)
        {
            bool success = false;
            //Takes the environment variables defined in the .env file and loads them into the apps environment
            DotNetEnv.Env.Load();

            var message = new MimeMessage();
            //The email address that is sending the email
            message.From.Add(new MailboxAddress("Restaurant Demo", "jacobdev43@outlook.com"));
            //The recipient
            message.To.Add(new MailboxAddress("Jacob Gordon", Environment.GetEnvironmentVariable("EMAIL")));
            message.Subject = email.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = @"<html><body><h1>Message From " + email.Name + "</h1><br/><p>" + email.Message + "<br/><span>Respond to the email address of " + email.EmailAddress + "</span></body></html>"

            };
            message.Body = builder.ToMessageBody();

            SmtpClient client = new SmtpClient();

            try
            {
                Debug.WriteLine(Environment.GetEnvironmentVariable("EMAIL_PASS"));
                client.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("jacobdev43@outlook.com", Environment.GetEnvironmentVariable("EMAIL_PASS"));
                client.Send(message);

                success = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                success = false;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            return success;

        }
    }
}
