using Authentication_Identity.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Service
{
    public class EmailService : IEmailService
    {

        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendAccountConfirmationMail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdaetPlaceHolders("Registration confirmaion {{UserName}}",userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdaetPlaceHolders(GetEmailBody("RegistraionConfirmation"),userEmailOptions.PlaceHolders);
            //userEmailOptions.ActivateionLink = UpdaetPlaceHolders("Link", userEmailOptions.PlaceHolders);          

            await SendMail(userEmailOptions);
        }

        private async Task SendMail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAdress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML,
            };

            foreach (var toEmail in userEmailOptions.ToMails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCrenditials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdaetPlaceHolders(string text, List<KeyValuePair<string,string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholer in keyValuePairs)
                {
                    if (text.Contains(placeholer.Key))
                    {
                        text = text.Replace(placeholer.Key, placeholer.Value);
                    }
                }
            }
            return text;
        }
    }
}
