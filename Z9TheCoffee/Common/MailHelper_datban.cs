using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace Common
{
    public class MailHelper_datban
    {
        public void SendMail_datban(string toEmailAddress_datban, string subject_datban, string content_datban)
        {
            var fromEmailAddress_datban = ConfigurationManager.AppSettings["FromEmailAddress_datban"].ToString();
            var fromEmailDisplayName_datban = ConfigurationManager.AppSettings["FromEmailDisplayName_datban"].ToString();
            var fromEmailPassword_datban = ConfigurationManager.AppSettings["FromEmailPassword_datban"].ToString();
            var smtpHost_datban = ConfigurationManager.AppSettings["SMTPHost_datban"].ToString();
            var smtpPort_datban = ConfigurationManager.AppSettings["SMTPPort_datban"].ToString();

            bool enabledSsl_datban = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL_datban"].ToString());

            string body_datban = content_datban;
            MailMessage message_datban = new MailMessage(new MailAddress(fromEmailAddress_datban, fromEmailDisplayName_datban), new MailAddress(toEmailAddress_datban));
            message_datban.Subject = subject_datban;
            message_datban.IsBodyHtml = true;
            message_datban.Body = body_datban;

            var client_datban = new SmtpClient();
            client_datban.Credentials = new NetworkCredential(fromEmailAddress_datban, fromEmailPassword_datban);
            client_datban.Host = smtpHost_datban;
            client_datban.EnableSsl = enabledSsl_datban;
            client_datban.Port = !string.IsNullOrEmpty(smtpPort_datban) ? Convert.ToInt32(smtpPort_datban) : 0;
            client_datban.Send(message_datban);
        }
    }
}
