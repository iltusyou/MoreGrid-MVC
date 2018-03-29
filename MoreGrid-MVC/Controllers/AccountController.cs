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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(MemberRegisterView memberRegister)
        {
            //to do: 驗證帳號重複
            if (ModelState.IsValid)
            {

                string authCode = mailService.GetValidateCode();

                string template = System.IO.File.ReadAllText(Server.MapPath("~/Views/MailTemplate/RegisterMailTemplate.html"));
                Member member = new Member();
                member.Email = memberRegister.Email;
                member.Password = memberRegister.Password;
                member.Name = memberRegister.Name;
                member.NickName = memberRegister.NickName;
                member.Phone = memberRegister.Phone;
                member.ValidateCode = mailService.GetValidateCode();
                member.Id = Guid.NewGuid();

                UriBuilder validateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Member", new { userId = member.Id, validateCode = member.ValidateCode })
                };
                string mailBody = mailService.GetRegisterMailBody(template, member.Name, validateUrl.ToString().Replace("%3F", "?"));

                //存入資料庫與寄送驗證信
                memberService.Register(member);
                mailService.SendMail(member.Email, mailBody, "會員註冊信");
                TempData["Result"] = "註冊成功，請去收驗證信";
                return View("Result");
            }
            return View(memberRegister);
        }
    }
}