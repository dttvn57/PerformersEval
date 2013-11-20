using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI;
using PerformersEval.Models;
using PerformersEval.Controllers;
using PerformersEval.DAL;
using System.Linq;

namespace PerformersEval.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller {

        PerformersDB _db = new PerformersDB();
        //IUserDB _repository;
//#if  InMemDB
//          public ValidationController() : this(InMemoryDB.Instance) { }
//#else
//        public ValidationController() : this(new EF_UserRepository()) { }
//#endif


        public ValidationController(PerformersDB repository)
        {
            _db = repository;
        }

        //public static string GetAltName(string Username, IUserDB repository) {

        //    string suggestedUID = String.Format(CultureInfo.InvariantCulture,
        //        "{0} is not available.", Username);

        //    for (int i = 1; i < 100; i++) {
        //        string altCandidate = Username + i.ToString();
        //        if (!repository.UserExists(altCandidate)) {
        //            suggestedUID = String.Format(CultureInfo.InvariantCulture,
        //           "{0} is not available. Try {1}.", Username, altCandidate);
        //            break;
        //        }
        //    }

        //    return suggestedUID;
        //}

        [HttpGet]
        //public virtual JsonResult IsUserNameAvailable(string contactEmailPrimary, int userAccountId)
        public JsonResult IsUserNameAvailable(string contactEmailPrimary)//, int userAccountId)
        {
            //if (userAccountId == 0)
            {
                //int results = 0;
                //var searchResults = _userServiceGateway.SearchUserLogin(contactEmailPrimary);
                var performerInDataBase = _db.Performers.Where(p => p.PerformerEmail.Equals(contactEmailPrimary));
                int cnt = performerInDataBase.Count();
                Performer p2 = performerInDataBase.First();


                bool result = true;
                if (performerInDataBase == null)
                    result = false;

                //We need to check for an exact match here as the search only does a "like"
                //foreach (var userAccountDto in searchResults)
                //{
                //    if (userAccountDto.UserLogin == contactEmailPrimary)
                //    {
                //        result = false;
                //        break;
                //    }
                //}
                if (!result)
                    return Json("This Email Address has already been registered", JsonRequestBehavior.AllowGet);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        /*public JsonResult IsUID_Available(string Username) {

            if (!_repository.UserExists(Username))
                return Json(true, JsonRequestBehavior.AllowGet);

            string suggestedUID = GetAltName(Username, _repository);
            return Json(suggestedUID, JsonRequestBehavior.AllowGet);
        }*/

    }
}
