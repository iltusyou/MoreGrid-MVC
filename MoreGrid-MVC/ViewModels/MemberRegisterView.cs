using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreGrid_MVC.ViewModels
{
    public class MemberRegisterView
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage = "這不是Email格式")]
        [Remote("CheckEmail", "Account", HttpMethod = "POST", ErrorMessage = "此Email已申請過會員，如忘記密碼可按忘記密碼取回密碼")]
        public string Email { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(20, ErrorMessage = "姓名密碼長度對多20字元")]
        public string Name { get; set; }

        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "密碼長度需為6~12字元")]
        [RegularExpression(@"[a-zA-Z0-9]*$", ErrorMessage = "密碼僅能有英文或數字")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請再次確認密碼")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        public string PasswordCheck { get; set; }

        [DisplayName("暱稱")]
        [Required(ErrorMessage = "請輸入暱稱")]
        public string NickName { get; set; }

        [DisplayName("生日")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "請輸入生日")]
        public DateTime Birthday { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請輸入性別")]
        public bool Gender { get; set; }

        [DisplayName("電話")]
        [Required(ErrorMessage = "請輸入電話")]
        [Phone(ErrorMessage = "不是電話格式")]
        public string Phone { get; set; }

    }
}