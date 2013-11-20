using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using PerformersEval.Models;
using PerformersEval.DAL;

using NLog;

namespace PerformersEval.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        //Logger logger = LogManager.GetLogger("InitializeSimpleMembershipAttribute");
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {

                Database.SetInitializer<PerformersDB>(null);
                //Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new PerformersDB())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    if (!WebSecurity.Initialized)
                    {
                        logger.Info("InitializeSimpleMembershipAttribute::SimpleMembershipInitializer calls WebSecurity.InitializeDatabaseConnection");
                        WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                            "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
