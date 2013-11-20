using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PerformersEval.DAL;
using PerformersEval.Models;
using System.IO;
using System.Net.Mail;
//using System.Net.Mail;
//using System.Web.Mail;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using WebMatrix.WebData;
using NLog;
using System.Web.Security;
using System.Configuration;

namespace PerformersEval.Controllers
{
    [Authorize]
    [RequireHttps]
    public class FormsStatusController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        PerformersDB _db = new PerformersDB();

        public ActionResult UploadDocument(string s = null)
        {
            FormsStatus f = new FormsStatus();
            if (s != null)
                f.TIN = s;

            return View(f);
        }

        //public ActionResult Upload()
        //{
        //    //return PartialView("_FormsStatusView");   // View();
        //    return RedirectToAction("Index", "FormsStatus", new { PerformerName = "trung", PerformerEmail = "trung@abc.com" });
        //}

        public static void WriteFileFromStream(Stream stream, string toFile)
        {
            using (FileStream fileToSave = new FileStream(toFile, FileMode.Create))
            {
                stream.CopyTo(fileToSave);
            }
        }

        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file, string s = null)
        {
            if (file != null && file.ContentLength > 0 && s != null)
            {             
                //var bytes = new byte[file.ContentLength];
                //file.InputStream.Read(bytes, 0, bytes.Length);C:\Users\tdang\Documents\Visual Studio 2012\Projects\PerformersEval\deploy_FS

                string ext;
                string fileName = Path.GetFileName(file.FileName);
                int extInd = fileName.LastIndexOf('.');
                if (extInd != -1)
                {
                    ext = fileName.Substring(extInd, fileName.Length - extInd);
                    fileName = "Ins_" + s + ext;

                }

                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

               // WriteFileFromStream(file.InputStream, path);
                file.SaveAs(path);

//TempData["STATUS"] = "Status: " + path;

                var formsStatus = _db.AllFormsStatus.Where(w => w.TIN.Equals(s));  // any record that had been create by Vendor (check by using TIN)
                try
                {
                    FormsStatus f = formsStatus.First();     // any record in the DB? throw exception if 

                    if (f.TIN.Equals(s)) // already there's a record with the same TIN
                    {
                        f.AutoInsuranceFileName = "Images/" + fileName;
                        _db.Entry(f).State = System.Data.EntityState.Modified;
                        _db.SaveChanges();

                        return RedirectToAction("Index", "FormsStatus");//, new { TIN = f.TIN });
                    }
                }
                catch (InvalidOperationException)   // it means no TIN exists so we can modify
                {
                    FormsStatus f = new FormsStatus();
                    f.TIN = s;
                    f.AutoInsuranceFileName = path;
                    _db.AllFormsStatus.Add(f);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = f.TIN });
                }

            }

            //return PartialView("_FormsStatusView"); 
            TempData["STATUS"] = "No Auto Insurance Uploaded";
            return RedirectToAction("Index", "FormsStatus");//, new { TIN = TIN });
        }

        //
        // GET: /FormsStatus/

        public ActionResult Index()//string TIN = "")
        {
            //UserProfile profile = _db.UserProfiles.Where(p => (p.UserName.Equals(user, StringComparison.OrdinalIgnoreCase)));
            //UserProfile profile = _db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == user);
            //if (profile != null)
            //{
            //    ViewBag.VendorCreated = profile.VendorCreate;
            //}
            //var formsStatus = _db.AllFormsStatus.Where(p => (p.TIN.Equals(TIN, StringComparison.OrdinalIgnoreCase)));
            logger.Info("Enter FormsStatusController::Index");

            // Taking care of the Admin/Manager who has been given the backdoor Name & email
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;
            bool isFUAdmin = User.IsInRole("fu_admin");
            if (isFUAdmin)
            {
                return View(_db.AllFormsStatus.ToList());
            }
            else
            {
                var formsStatus = _db.AllFormsStatus.Where(p => (p.CreatedByUser.Equals(WebSecurity.CurrentUserName, StringComparison.OrdinalIgnoreCase)));
                int cnt = formsStatus.Count();
                return View(formsStatus.ToList());//_db.AllFormsStatus.ToList());
            }
        }

        public ActionResult Status(string s)//TIN)
        {
            // Taking care of the Admin/Manager who has been given the backdoor Name & email
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;
            bool isFUAdmin = User.IsInRole("fu_admin");
            if (isFUAdmin && (s != null && s.Length == 0 || s == null))
            {
                if (_db.AllFormsStatus.Count() == 0)
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No TIN Found";
                   // return RedirectToAction("Index", "FormsStatus", new { TIN = TIN });
                }
                return PartialView("_FormsStatusView", _db.AllFormsStatus.ToList());
            }

            var formsStatus = isFUAdmin ? _db.AllFormsStatus.Where(p => (p.TIN.Equals(s, StringComparison.OrdinalIgnoreCase)))
                                        : _db.AllFormsStatus.Where(p => (p.TIN.Equals(s, StringComparison.OrdinalIgnoreCase)) && 
                                                            (p.CreatedByUser.Equals(WebSecurity.CurrentUserName, StringComparison.OrdinalIgnoreCase)));
            int cnt = formsStatus.Count();
            if (cnt == 0)
            {
                //TempData["OBJECT"] = null;
                TempData["STATUS"] = "No TIN Found";
                return PartialView("_FormsStatusView", formsStatus);//.First());
            }
            else 
            if (cnt > 1)
            {
                //TempData["OBJECT"] = null;
                TempData["STATUS"] = "Error: more than one Vendor. Please see the Admin.";
                return PartialView("_FormsStatusView", formsStatus);//.First());
           }
            else
                return PartialView("_FormsStatusView", formsStatus);//.First());
        }

        public ActionResult SubmitAllForms(string spec = null, string name = null) // Your model is passed in here
        {
            var formsStatus = _db.AllFormsStatus.Where(p => (p.TIN.Equals(spec, StringComparison.OrdinalIgnoreCase)));
            int cnt = formsStatus.Count();
            if (cnt == 0)
            {
                TempData["OBJECT"] = null;
                TempData["STATUS"] = "Vendor Not Found";
                return RedirectToAction("Index", "FormsStatus");//, new { TIN = spec });
            }
            else
            if (cnt > 1)
            {
                TempData["OBJECT"] = null;
                TempData["STATUS"] = "Error: more than one Vendor. Please see the Admin.";
                return RedirectToAction("Index", "FormsStatus");//, new { TIN = spec });
            }

            try
            {
                FormsStatus f = formsStatus.First();
                if (f.TIN.Equals(spec)) // already there's a record with the same TIN
                {
                    f.FormsSent = DateTime.Now; // true;
                    _db.Entry(f).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();

                    /*************
                    //Outlook exchange Service 
                    ExchangeService service = new ExchangeService();
                    service.AutodiscoverUrl("tdang@aclibrary.org");

                    EmailMessage message = new EmailMessage(service);
                    message.Subject = "Test EWS";
                    message.Body = "Test EWS";
                    message.ToRecipients.Add("tdang@aclibrary.org");
                    message.Save();

                    try
                    {
                        message.SendAndSaveCopy();
                    }
                    catch (Exception e)
                    {

                        TempData["OBJECT"] = null;
                        TempData["STATUS"] = e.Message;
                        //return RedirectToAction("Index");
                        //return PartialView("_FormsStatusView", formsStatus);
                        return RedirectToAction("Index", "FormsStatus", new { TIN = f.TIN });
                    } 

                    //System.Windows.MessageBox.Show("Message sent!");

                    //recipientTextbox.Text = "";
                    //subjectTextbox.Text = "";
                    //bodyTextbox.Text = "";  
                    ***************/

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("Performers-Notification@aclibrary.org", "Performers Notification");

                    string[] emails = ConfigurationManager.AppSettings.AllKeys
                                                 .Where(key => key.StartsWith("fuemail"))
                                                 .Select(key => ConfigurationManager.AppSettings[key])
                                                 .ToArray();
                    foreach (string email in emails)
                    {
                        message.To.Add(new MailAddress(email));
                    }
                    //message.To.Add(new MailAddress("dttvn57@gmail.com"));

                    string subject = "A performer has submitted all forms";
                    message.Subject = subject;
                    string body = (name != null ? name : "") + " has completed all required forms";
//                    string body = (name != null ? name : "") + (spec != null ? spec : "") + " has completed all required forms";
                    message.Body = body;
                    ////msg.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient("bender.aclibrary.org", 25);

                    //client.UseDefaultCredentials = false;
                    client.EnableSsl = false;
                    //client.Credentials = new NetworkCredential("TestLogin", "TestPassword");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.EnableSsl = true;

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception e)
                    {

                        TempData["OBJECT"] = null;
                        TempData["STATUS"] = e.Message;
                        return RedirectToAction("Index", "FormsStatus");//, new { TIN = spec });
                    } 
                }
            }
            catch (InvalidOperationException)   // it means no TIN exists so we can modify
            {
                TempData["OBJECT"] = null;
                TempData["STATUS"] = "Performer Not Found";
                return RedirectToAction("Index", "FormsStatus");//, new { TIN = spec });
            }

            return RedirectToAction("Index", "FormsStatus");//, new { TIN = spec });
        }

        //private void SendEmail()
        //{
        //    ExchangeService service = CreateConnection("https://autodiscover.test.com/EWS/Exchange.asmx");

        //    EmailMessage msg = new EmailMessage(service);
        //    msg.ToRecipients.Add(new EmailAddress("tdang@aclibrary.org"));
        //    msg.Subject = "Test email";
        //    msg.Body = new MessageBody(BodyType.HTML, "<p>Hello Email!</p>");
        //    try
        //    {
        //        msg.Send();
        //    }
        //    catch (Exception e)
        //    {

        //        TempData["OBJECT"] = null;
        //        TempData["STATUS"] = e.Message;
        //        //return RedirectToAction("Index");
        //        //return PartialView("_FormsStatusView", formsStatus);
        //        //return RedirectToAction("Index", "FormsStatus", new { TIN = f.TIN });
        //    }
        //}

        //public static ExchangeService CreateConnection(String url)
        //{
        //    // Hook up the cert callback to prevent error if Microsoft.NET doesn't trust the server
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        delegate(
        //            Object obj,
        //            X509Certificate certificate,
        //            X509Chain chain,
        //            SslPolicyErrors errors)
        //        {
        //            return true;
        //        };

        //    ExchangeService service = new ExchangeService();
        //    service.Url = new Uri(url);

        //    service.Credentials = new NetworkCredential("username", "password", "domain");

        //    return service;
        //}

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
