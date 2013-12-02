﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

using PerformersEval.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PerformersEval.DAL
{
    public class PerformersDB : DbContext
    {
        public PerformersDB()
            : base("name=DefaultConnection")
        {
          //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PerformersDB, PerformersEval.Migrations.Configuration>("DefaultConnection"));
            Database.SetInitializer<PerformersDB>(null);
             // Database.SetInitializer<PerformersDB>(new DropCreateDatabaseAlways<PerformersDB>());
        }

        public DbSet<Performer> Performers  { get; set; } 
        public DbSet<Vendor>    Vendors     { get; set; }
        public DbSet<SubW9>     SubW9s      { get; set; }
        public DbSet<Invoice>   Invoices    { get; set; }
        public DbSet<FormsStatus> AllFormsStatus { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> webpages_Memberships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    //public class UsersContext : DbContext
    //{
    //    public UsersContext()
    //        : base("DefaultConnection")
    //    {
    //    }

    //    public DbSet<UserProfile> UserProfiles { get; set; }       
    //    public DbSet<webpages_Membership> webpages_Memberships { get; set; }
    //}
}