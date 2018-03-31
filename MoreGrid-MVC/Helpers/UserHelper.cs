using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MoreGrid_MVC.Helpers
{
    public class UserHelper
    {
        /// <summary>
        /// 儲存登入資訊
        /// </summary>
        /// <param name="member"></param>
        /// <param name="isRemeber"></param>
        /// <returns></returns>
        public static string LoginProcess(Models.Member member, bool isRemeber)
        {
            var now = DateTime.Now;
            string userData = string.Format("{0},{1},{2},{3},{4}", 
                member.Id.ToString(), 
                member.Name, 
                member.NickName, 
                member.Phone, 
                member.Email);

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: member.Id.ToString(),
                issueDate: now,
                expiration: now.AddMinutes(30),
                isPersistent: isRemeber,
                userData: userData,
                cookiePath: FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            return encryptedTicket;
        }

        public static Models.Member GetUserData()
        {
            Models.Member member = new Models.Member();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                char[] delimiterChars = { ',' };

                // 先取得該使用者的 FormsIdentity
                FormsIdentity id = HttpContext.Current.User.Identity as FormsIdentity;
                // 再取出使用者的 FormsAuthenticationTicket
                FormsAuthenticationTicket ticket = id.Ticket;
                string[] userInfo = id.Ticket.UserData.Split(delimiterChars);

                member.Id = new Guid(userInfo[0]);
                member.Name = userInfo[1];
                member.NickName = userInfo[2];
                member.Phone = userInfo[3];
                member.Email = userInfo[4];
                return member;
            }
            return null;
        }
    }
}