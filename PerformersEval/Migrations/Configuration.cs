namespace PerformersEval.Migrations
{
    using NLog;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<PerformersEval.DAL.PerformersDB>
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        public Configuration()
        {
            // these 2 are for Automatic Migration.
            //AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;

            // set this to false to use Code First Migration
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PerformersEval.DAL.PerformersDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedMembership();
        }


        private void SeedMembership()
        {
            //logger.Info("Enter Configuration::SeedMembership calls WebSecurity.InitializeDatabaseConnection");
            if (!WebSecurity.Initialized)
            {
                //logger.Info("Configuration::SeedMembership calls WebSecurity.InitializeDatabaseConnection");
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("user"))
            {
                roles.CreateRole("user");
            }
            if (!roles.RoleExists("fu_admin"))
            {
                roles.CreateRole("fu_admin");
            }

            if (membership.GetUser("tdang", false) == null)
            {
                membership.CreateUserAndAccount("tdang", "tdangpwd");
            }
            if (!roles.GetRolesForUser("tdang").Contains("fu_admin"))
            {
                roles.AddUsersToRoles(new[] { "tdang" }, new[] { "fu_admin" });
            }

            if (membership.GetUser("icarrasco", false) == null)
            {
                membership.CreateUserAndAccount("icarrasco", "icarrascopwd");
            }
            if (!roles.GetRolesForUser("icarrasco").Contains("fu_admin"))
            {
                roles.AddUsersToRoles(new[] { "icarrasco" }, new[] { "fu_admin" });
            }

            if (membership.GetUser("dchattha", false) == null)
            {
                membership.CreateUserAndAccount("dchattha", "dchatthapwd");
            }
            if (!roles.GetRolesForUser("dchattha").Contains("fu_admin"))
            {
                roles.AddUsersToRoles(new[] { "dchattha" }, new[] { "fu_admin" });
            }

            if (membership.GetUser("rdizon", false) == null)
            {
                membership.CreateUserAndAccount("rdizon", "rdizonpwd");
            }
            if (!roles.GetRolesForUser("rdizon").Contains("fu_admin"))
            {
                roles.AddUsersToRoles(new[] { "rdizon" }, new[] { "fu_admin" });
            }


            // a generic admin role
            if (!roles.RoleExists("aclibadmin"))
            {
                roles.CreateRole("aclibadmin");
            }

            if (membership.GetUser("aclibadmin", false) == null)
            {
                membership.CreateUserAndAccount("aclibadmin", "aclibadminpwd");
            }
            if (!roles.GetRolesForUser("aclibadmin").Contains("aclibadmin"))
            {
                roles.AddUsersToRoles(new[] { "aclibadmin" }, new[] { "aclibadmin" });
            }

        }
    }    
}
