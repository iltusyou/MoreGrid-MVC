using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MoreGrid_MVC.Helpers
{
    public class ValidateHelper
    {       
        /// <summary>
        /// 驗證Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取得n位的驗證碼
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetValidateCode()
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