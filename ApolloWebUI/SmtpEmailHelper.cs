using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWebUI
{
    public class SmtpEmailHelper
    {
        SmtpClient smtpServer;
        string userName;
        public SmtpEmailHelper(string host, string userName, string password)
        {
            smtpServer = new SmtpClient(host);
            this.userName = userName;
            smtpServer.Port = 25;
            smtpServer.Credentials = new System.Net.NetworkCredential(userName, password);
            smtpServer.EnableSsl = true;
        }

        public async Task SendEmailAsync(string title, string body, IEnumerable<string> toAddresses)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(userName);
            AddRecieveAddress(mail, toAddresses);
            mail.Subject = title;//标题
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.Priority = System.Net.Mail.MailPriority.High;//邮件优先级
            mail.IsBodyHtml = true;
            await smtpServer.SendMailAsync(mail);//发送
        }

        public async Task SendEmailAsync(string title, string body, IEnumerable<MailAddress> toAddresses)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(userName);
            AddRecieveAddress(mail, toAddresses);
            mail.Subject = title;//标题
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.Priority = System.Net.Mail.MailPriority.High;//邮件优先级
            mail.IsBodyHtml = true;
            await smtpServer.SendMailAsync(mail);//发送
        }

        private void AddRecieveAddress(MailMessage mail, IEnumerable<string> toAddresses)
        {
            foreach (var item in toAddresses)
            {
                mail.To.Add(item);
            }
        }

        private void AddRecieveAddress(MailMessage mail, IEnumerable<MailAddress> toAddresses)
        {
            foreach (var item in toAddresses)
            {
                mail.To.Add(item);
            }
        }
    }
}
