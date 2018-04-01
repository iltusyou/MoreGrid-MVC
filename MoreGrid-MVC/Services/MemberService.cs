using Microsoft.Ajax.Utilities;
using MoreGrid_MVC.DataContext;
using MoreGrid_MVC.Helpers;
using MoreGrid_MVC.Models;
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
        public string Register(Models.Member member)
        {
            try
            {
                var query = db.Members.Where(p => p.Email == member.Email);
                if (query.Count() > 0)
                    return "此Email已註冊過";

                member.Password = HashPassword(member.Password);
                member.Status = false;
                //TODO:
                member.Birthday = DateTime.Now;
                member.RegisterTime = DateTime.Now;
                member.UpdateTime = DateTime.Now;
                db.Members.Add(member);
                db.SaveChanges();
                return string.Empty;
            }
            catch(Exception ex)
            {
                return "Service Error";
            }
        }

        public bool CheckEmail(string email)
        {
            return !(db.Members.Where(p => p.Email == email).Count() > 0);
        }

        public List<Models.Member> Query(QueryMember queryMember)
        {
            IEnumerable<Models.Member> query = db.Members;
            var result = new List<Models.Member>();
            if (queryMember.Email != "")
            {
                query = query.Where(p => p.Email == queryMember.Email);
            }

            return query.ToList();
        }

        /// <summary>
        /// 驗證註冊信
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public string EmailValidate(string memberId, string validateCode)
        {
            string message = "驗證失敗";

            Guid id;
            if (!Guid.TryParse(memberId, out id))
                return message;
                
            try
            {
                var member = db.Members.Find(id);
                if (member != null)
                {
                    if (member.ValidateCode == string.Empty)
                        message = "此Email以驗證過";

                    else if (member.ValidateCode == validateCode)
                    {
                        member.ValidateCode = string.Empty;
                        member.Status = true;
                        db.SaveChanges();
                        message = string.Empty;
                    }
                }
            }
            catch(Exception ex)
            {
                return message;
            }
            return message;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="validateCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string ResetPassword(string memberId, string validateCode, string password)
        {
            string message = "驗證失敗";

            Guid id;
            if (!Guid.TryParse(memberId, out id))
                return message;

            try
            {
                var member = db.Members.Find(id);
                if (member != null)
                {
                    if (member.ValidateCode == validateCode)
                    {
                        member.ValidateCode = string.Empty;
                        member.Password = HashPassword(password);
                        db.SaveChanges();
                        message = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return message;
            }
            return message;
        }

        /// <summary>
        /// 登入驗證
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Models.Member Login(string email, string password)
        {
            password = HashPassword(password);
            Models.Member member = null;

            try
            {
                var query = db.Members.Where(p => p.Email == email && p.Password == password && p.Status == true);
                if (query.Count() == 1)
                {
                    member = query.FirstOrDefault();
                    return member;
                }
                    
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }       

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public string UpdatePassword(Guid memberId, string oldPassword, string newPassword)
        {           
            try
            {
                oldPassword = HashPassword(oldPassword);
                var query = db.Members.Where(p => p.Id == memberId && p.Password == oldPassword);
                if (query.Count() == 0)
                    return "密碼錯誤";

                var member = query.FirstOrDefault();
                member.Password = HashPassword(newPassword);
                db.SaveChanges();
                return string.Empty;
            }
            catch(Exception ex)
            {
                return "Service Error";
            }
        }

        public string Update(Models.Member newMember)
        {
            try
            {
                var oldMember = db.Members.Find(newMember.Id);
                oldMember.Name = newMember.Name;

                db.SaveChanges();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Service Error";
            }
        }

        public string ForgetPassword(string email)
        {
            try
            {
                var query = db.Members.Where(p => p.Email == email && p.Status == true);
                if (query.Count() == 0)
                    return "查無此Email或此Email尚未通過認證";

                var member = query.FirstOrDefault();
                member.ValidateCode = ValidateHelper.GetValidateCode();
                db.SaveChanges();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Service Error";
            }
        }

        /// <summary>
        /// 登入紀錄
        /// </summary>
        /// <param name="memberId"></param>
        public void LoginRecord(Guid memberId)
        {
            try
            {
                var recode = new MemberLoginRecord();
                recode.MemberId = memberId;
                recode.LoginTIme = DateTime.Now;
                db.MemberLoginRecords.Add(recode);
                db.SaveChanges();
  
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool registerEmailCheck(string email)
        {
            var member = db.Members.Where(p => p.Email == email);
            return member.Count() == 0;
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