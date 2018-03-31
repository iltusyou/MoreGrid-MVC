using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.Models
{
    /// <summary>
    /// 會員登入紀錄
    /// </summary>
    [Table("MemberLoginRecord")]
    public class MemberLoginRecord
    {
        [Key]
        public int Id { get; set; }

        public Guid MemberId { get; set; }

        public DateTime LoginTIme { get; set; }
    }
}