using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.Models
{
    [Table("ForgetPassword")]
    public class ForgetPassword
    {
        [Key]
        public int Id { get; set; }

        public Guid MemberId { get; set; }

        public string ValidateCode { get; set; }
    }
}