﻿using MoreGrid_MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreGrid_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var member = UserHelper.GetUserData();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }        
    }
}