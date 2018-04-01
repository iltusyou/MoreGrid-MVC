using MoreGrid_MVC.Helpers;
using MoreGrid_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace MoreGrid_MVC.Controllers
{
    public class AuthController : Controller
    {
        private MemberService memberService = new MemberService();

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var view = new ViewModels.LoginView();
            return View(view);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ViewModels.LoginView view)
        {
            if (ModelState.IsValid)
            {
                Models.Member member = memberService.Login(view.Email, view.Password);
                if (member != null)
                {
                    string encryptedTicket = UserHelper.LoginProcess(member, view.RememberMe);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    //登入記錄寫入資料庫
                    memberService.LoginRecord(member.Id);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(view);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }
    }
}