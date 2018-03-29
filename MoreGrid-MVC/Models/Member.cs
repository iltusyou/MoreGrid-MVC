using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        public string Phone { get; set; }

        public string ValidateCode { get; set; }

        public bool Status { get; set; }

        public DateTime RegisterTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}