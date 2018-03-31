using Microsoft.Ajax.Utilities;
using MoreGrid_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MoreGrid_MVC.DataContext
{
    public class MoreGridDBContext: DbContext
    {
        public MoreGridDBContext() : base("name=MoreGridEntities") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Member>();
            modelBuilder.Entity<MemberLoginRecord>();
            modelBuilder.Entity<ForgetPassword>();
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Models.Member> Members { get; set; }
        public virtual DbSet<MemberLoginRecord> MemberLoginRecords { get; set; }
        public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }
    }
}