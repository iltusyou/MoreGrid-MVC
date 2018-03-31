using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.ViewModels
{
    public class UpdatePasswordView
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "請輸入舊密碼")]
        public string OldPassword { get; set; }

        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入新密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請再次確認密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        public string PasswordCheck { get; set; }
    }
}