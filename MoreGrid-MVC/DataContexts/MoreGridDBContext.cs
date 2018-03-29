using Microsoft.Ajax.Utilities;
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
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Models.Member> Members { get; set; }
    }
}