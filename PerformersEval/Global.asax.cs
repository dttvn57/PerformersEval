using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PerformersEval.DAL;
//using PerformersEval.Migrations;
using System.Data.Entity;
using NLog.Common;
using System.Data.Entity.Migrations;
using WebMatrix.WebData;

//using NLog;
//using PerformersEval.Migrations;

namespace PerformersEval
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            //string nlogPath = Server.MapPath("nlog-app.log");
            //InternalLogger.LogFile = nlogPath;
            //InternalLogger.LogLevel = NLog.LogLevel.Trace;

             //logger.Trace("Enter ::Application_Start");
           //var migrator = new DbMigrator(new Configuration());
           //migrator.Update();

            if (!WebSecurity.Initialized)
            {
                //logger.Trace("::Application_Start calls WebSecurity.InitializeDatabaseConnection");
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                    "FIN_PE_UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            //logger.Trace("::Application_Start no calls to InitializeDatabaseConnection");

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //GlobalFilters.Filters.Add(new RequireSSLAttribute());
          //GlobalFilters.Filters.Add(new NotRequireSSLAttribute());

            //===========>>>> comment out if test locally AND comment all [Authorize], [RequiredHttps] from the Controllers
        // GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            /***
                Database.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());

                //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
                //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
                //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
             ***/
            //System.Data.Entity.Database.SetInitializer<PerformersEval.DAL.PerformersDB>(null);// null: turn off DB initializer (don't want to lose data)
            //System.Data.Entity.Database.SetInitializer<PerformersEval.DAL.PerformersDB>(new DropCreateDatabaseAlways<PerformersEval.DAL.PerformersDB>());
                            
// version 13            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<PerformersEval.DAL.PerformersDB, Configuration>());
        }
    }
}