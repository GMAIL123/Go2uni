using Go2uniApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Go2uniApi
{
    public static class BaseClass
    {
        public static string Mail(string emailTo, EmailTemplate Template)
        {
            string Res = string.Empty;
            try
            {
                MailMessage mail = new MailMessage();
                if (emailTo.Contains(','))
                {
                    var emailList = emailTo.Split(',');
                    foreach (var item in emailList)
                    {
                        mail.To.Add(item);
                    }
                }
                else
                {
                    mail.To.Add(emailTo);
                }
                mail.From = new MailAddress("Go2UniTestMail@esolzdev.com");
                mail.Subject = Template.Header="Please provide Subject here";             // Set Mail's Subject here
                mail.Body = Template.TemplateBody="Please provide Body here";             // Set Mail's Body here
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.esolzdev.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("Go2UniTestMail@esolzdev.com", "Pyc8a1!6");
                smtp.EnableSsl = false;
                smtp.Send(mail);
                Res = "Success! Mail has been sent";
            }
            catch (Exception ex)
            {
                Res = "Failed!" + ex.Message;
            }
            return Res;
        }
    }
}