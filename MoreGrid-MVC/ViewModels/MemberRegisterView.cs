using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.ViewModels
{
    public class MemberRegisterView
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage = "這不是Email格式")]
        public string Email { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        public string Name { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        public string PasswordCheck { get; set; }

        [DisplayName("暱稱")]
        [Required(ErrorMessage = "請輸入暱稱")]
        public string NickName { get; set; }

        [DisplayName("生日")]
        [Required(ErrorMessage = "請輸入生日")]
        public DateTime Birthday { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請輸入性別")]
        public bool Gender { get; set; }

        [DisplayName("電話")]
        [Required(ErrorMessage = "請輸入電話")]
        public string Phone { get; set; }

    }
}