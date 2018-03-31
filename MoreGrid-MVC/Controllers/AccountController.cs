using MoreGrid_MVC.Helpers;
using MoreGrid_MVC.Models;
using MoreGrid_MVC.Services;
using MoreGrid_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreGrid_MVC.Controllers
{
    public class AccountController : Controller
    {
        private MemberService memberService = new MemberService();
        private MailService mailService = new MailService();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        #region 註冊
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(MemberRegisterView memberRegister)
        {
            //TODO: 驗證帳號重複
            if (ModelState.IsValid)
            {
                Member member = new Member();
                member.Email = memberRegister.Email;
                member.Password = memberRegister.Password;
                member.Name = memberRegister.Name;
                member.NickName = memberRegister.NickName;
                member.Phone = memberRegister.Phone;
                member.ValidateCode = ValidateHelper.GetValidateCode();
                member.Id = Guid.NewGuid();

                string template = System.IO.File.ReadAllText(Server.MapPath("~/Views/MailTemplate/RegisterMailTemplate.html"));
                UriBuilder validateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Account", new { memberId = member.Id, validateCode = member.ValidateCode })
                };
                string mailBody = mailService.GetValidateMailBody(template, member.Name, validateUrl.ToString().Replace("%3F", "?"));

                //存入資料庫與寄送驗證信
                string result = string.Empty;

                string message = memberService.Register(member);
                if (message != string.Empty)
                {
                    result = message;
                }
                else if (!mailService.SendMail(member.Email, mailBody, "會員註冊信"))
                {
                    result = "寄送驗證信失敗";
                }
                else
                {
                    result = "註冊成功，請去收驗證信";
                }

                TempData["Result"] = result;
                return View("Result");
            }
            return View(memberRegister);
        }

        public ActionResult EmailValidate(string memberId = "", string validateCode = "")
        {
            string message = memberService.EmailValidate(memberId, validateCode);
            TempData["Result"] = message == "" ? "驗證成功" : message;
            return View("Result");
        }
        #endregion

        #region 個人資料修正

        #endregion

        #region 忘記密碼
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            if (!ValidateHelper.IsValidEmail(email))
            {
                TempData["Result"] = "Email格式錯誤";
                return View("Result");
            }

            var queryMember = new QueryMember();
            queryMember.Email = email;
            var memberList = memberService.Query(queryMember);
            if (memberList.Count == 0)
            {
                TempData["Result"] = "查無此Email";
                return View("Result");
            }

            var member = memberList.FirstOrDefault();
            member.ValidateCode = ValidateHelper.GetValidateCode();
            string updateMsg = memberService.Update(member);
            if (!string.IsNullOrEmpty(updateMsg))
            {
                TempData["Result"] = updateMsg;
                return View("Result");
            }

            string template = System.IO.File.ReadAllText(Server.MapPath("~/Views/MailTemplate/RegisterMailTemplate.html"));
            UriBuilder validateUrl = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ResetPassword", "Account", new { memberId = member.Id, validateCode = member.ValidateCode })
            };
            string mailBody = mailService.GetValidateMailBody(template, member.Name, validateUrl.ToString().Replace("%3F", "?"));

            if (!mailService.SendMail(member.Email, mailBody, "重新設定密碼"))
            {
                TempData["Result"] = "Service Error";
                return View("Result");
            }

            TempData["Result"] = "請去收重設密碼驗證信";
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string password)
        {
            string memberId = Request.QueryString["memberId"] ?? "";
            string validateCode = Request.QueryString["validateCode"] ?? "";
            TempData["Result"] = memberService.ResetPassword(memberId, validateCode, password);

            return View();
        }
        #endregion
    }
}