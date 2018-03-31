using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.Helpers
{
    public class MailTemplateHelper
    {
        /// <summary>
        /// 取得驗證信(通用)
        /// </summary>
        /// <param name="template"></param>
        /// <param name="userName"></param>
        /// <param name="validateUrl"></param>
        /// <returns></returns>
        public static string GetValidateMailBody(string template, string userName, string validateUrl)
        {
            template = template.Replace("{{userName}}", userName);
            template = template.Replace("{{validateUrl}}", validateUrl);
            return template;
        }
    }
}