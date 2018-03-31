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
    [Authorize]
    public class ManageController : Controller
    {
        private MemberService memberService = new MemberService();
        private MailService mailService = new MailService();

        // GET: Manage
        public ActionResult Index()
        {
            var member = UserHelper.GetUserData();

            return View(member);
        }

        [AllowAnonymous]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult UpdatePassword(UpdatePasswordView view)
        {
            if (ModelState.IsValid)
            {
                string message = memberService.UpdatePassword(UserHelper.GetUserData().Id, view.OldPassword, view.Password);
                TempData["Result"] = message;
                return View("Result");
            }

            return View(view);
        }

       

        
    }
}