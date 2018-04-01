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
            var view = new MemberRegisterView();
            return View(view);
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
                member.Birthday = memberRegister.Birthday;
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
                    TempData["ErrorMsg"] = message;
                }
                else if (!mailService.SendMail(member.Email, mailBody, "會員註冊信"))
                {
                    TempData["ErrorMsg"] = "寄送驗證信失敗";
                }
                else
                {
                    TempData["SuccessMsg"] = "註冊成功，請去收驗證信";
                }

                return View("Message");
            }
            return View(memberRegister);
        }

        #region 註冊驗證
        [HttpPost]
        public ActionResult CheckEmail(string email)
        {
            return Json(memberService.CheckEmail(email));
        }
        #endregion

        public ActionResult EmailValidate(string memberId = "", string validateCode = "")
        {
            string message = memberService.EmailValidate(memberId, validateCode);
            if (string.IsNullOrEmpty(message))
                TempData["SuccessMsg"] = "驗證成功";
            else
                TempData["ErrorMsg"] = message;

            return View("Message");
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
                TempData["ErrorMsg"] = "Email格式錯誤";
                return View("Message");
            }

            var queryMember = new QueryMember();
            queryMember.Email = email;
            var memberList = memberService.Query(queryMember);
            if (memberList.Count == 0)
            {
                TempData["ErrorMsg"] = "查無此Email";
                return View("Message");
            }

            var member = memberList.FirstOrDefault();
            member.ValidateCode = ValidateHelper.GetValidateCode();
            string updateMsg = memberService.Update(member);
            if (!string.IsNullOrEmpty(updateMsg))
            {
                TempData["ErrorMsg"] = updateMsg;
                return View("Message");
            }

            string template = System.IO.File.ReadAllText(Server.MapPath("~/Views/MailTemplate/RegisterMailTemplate.html"));
            UriBuilder validateUrl = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ResetPassword", "Account", new { memberId = member.Id, validateCode = member.ValidateCode })
            };
            string mailBody = mailService.GetValidateMailBody(template, member.Name, validateUrl.ToString().Replace("%3F", "?"));

            if (!mailService.SendMail(member.Email, mailBody, "重新設定密碼"))
            {
                TempData["ErrorMsg"] = "Service Error";
                return View("Message");
            }

            TempData["SuccessMsg"] = "請去收重設密碼驗證信";
            return View("Message");
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
            string message = memberService.ResetPassword(memberId, validateCode, password);
            if (string.IsNullOrEmpty(message))
                TempData["SuccessMsg"] = "密碼重新設定成功";
            else
                TempData["ErrorMsg"] = message;

            return View("Message");
        }
        #endregion
    }
}