using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MoreGrid_MVC.Services
{
    public class MailService
    {
        private const string smtpServer = "smtp.gmail.com";
        private const int smtpPort = 587;
        private readonly string smtpAccount = ConfigurationManager.AppSettings["SmtpAccount"];
        private readonly string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        private readonly string mailFrom = ConfigurationManager.AppSettings["MailFrom"];

        /// <summary>
        /// 取得驗證信(通用)
        /// </summary>
        /// <param name="template"></param>
        /// <param name="userName"></param>
        /// <param name="validateUrl"></param>
        /// <returns></returns>
        public string GetValidateMailBody(string template, string userName, string validateUrl)
        {
            template = template.Replace("{{userName}}", userName);
            template = template.Replace("{{validateUrl}}", validateUrl);
            return template;
        }

        /// <summary>
        /// 共通的寄信方法
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailBody"></param>
        /// <param name="mailSubject"></param>
        /// <returns></returns>
        public bool SendMail(string mailTo, string mailBody,  string mailSubject)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpAccount, smtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(mailFrom);
                mail.To.Add(mailTo);
                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                smtpClient.Send(mail);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 產生註冊信用驗證碼
        /// </summary>
        /// <returns>
        /// 10個隨機的字母
        /// </returns>
        public string GetValidateCode()
        {
            string[] code = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
                "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
            "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z"};

            string validateCode = string.Empty;
            Random r = new Random();
            for (var i = 0; i < 10; i++)
            {
                validateCode += code[r.Next(code.Count())];
            }
            return validateCode;
        }
    }
}