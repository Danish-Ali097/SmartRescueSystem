using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRescueSystem.Models;

namespace SmartRescueSystem.Controllers
{
    public class RescueController : Controller
    {
        SmartRescueSystemEntities db = new SmartRescueSystemEntities();
        // GET: Rescue
        public ActionResult Index()
        {
            return View();
        }
        //FIle Report
        public ActionResult FileReport()
        {
            ViewData["catlist"] = db.Catagories.ToList<Catagory>();
             HttpCookie cookie = Request.Cookies["EmergencyRequest"];
            if(cookie != null && cookie["status"] == "pending")
            {
                int id = Convert.ToInt32(cookie["id"]);
                Emergency temp = db.Emergencies.Find(id);
                if(temp.status == "accepted")
                {
                    ViewData["Title"] = "Title: " + temp.emergency_title;
                    ViewData["Message"] = "\n Your Request hasbeen Accepted! \n"+temp.sp_remarks;
                    Response.Cookies.Remove("EmergencyRequest");
                }
                else
                {
                    ViewData["Title"] = temp.emergency_title;
                    ViewData["Message"] = "Your Request hasbeen Send to the Authorities! Waiting for Response";
                }
            }
            return View();
        }
        //POST:File Report
        [HttpPost]
        public ActionResult FileReport(Emergency emergency)
        {
            if (ModelState.IsValid)
            {
                emergency.status = "pending";
                db.Emergencies.Add(emergency);
                db.SaveChanges();
                HttpCookie cookie = new HttpCookie("EmergencyRequest");
                cookie["id"] = emergency.Id.ToString();
                cookie["status"] = emergency.status;
                cookie.Expires = System.DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookie);
            }
            ViewData["catlist"] = db.Catagories.ToList<Catagory>();
            ViewData["Title"] = emergency.emergency_title;
            ViewData["Message"] = "Your Request hasbeen Send to the Authorities!";
            
            return View();
        }

        //GET:View Report
        public ActionResult ViewReport()
        {
            ViewData["emerlist"] = db.Emergencies.ToList<Emergency>();
            return View();
        }
    }
}