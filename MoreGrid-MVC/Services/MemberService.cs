using Microsoft.Ajax.Utilities;
using MoreGrid_MVC.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MoreGrid_MVC.Services
{
    public class MemberService
    {
        MoreGridDBContext db = new MoreGridDBContext();

        /// <summary>
        /// 註冊資料寫入資料庫
        /// </summary>
        /// <param name="member"></param>
        public void Register(Models.Member member)
        {
            member.Password = HashPassword(member.Password);
            member.Status = false;
            //TODO:
            member.Birthday = DateTime.Now;
            member.RegisterTime = DateTime.Now;
            member.UpdateTime = DateTime.Now;
            db.Members.Add(member);
            db.SaveChanges();
        }

        #region Hash密碼
        private string HashPassword(string password)
        {
            string saltKey = "asghidsahgliahgioag";
            string salyAndPassword = String.Concat(password, saltKey);
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] passwordData = Encoding.Default.GetBytes(salyAndPassword);
            byte[] hashData = sha1Hasher.ComputeHash(passwordData);
            string hashResult = "";
            //TODO: Hash密碼後的長度？
            for (int i = 0; i < hashData.Length; i++)
            {
                hashResult += hashData[i].ToString("x2");
            }
            return hashResult;
        }
        #endregion
    }
}