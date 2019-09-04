using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRescueSystem.Models;

namespace SmartRescueSystem.Controllers
{
    public class SPController : Controller
    {
        SmartRescueSystemEntities db = new SmartRescueSystemEntities();   
        // GET: SP
        public ActionResult Index()
        {
            if (Session["SPLgnFlag"] != null)
            {
                ServiceProvider temp = (ServiceProvider)Session["SPLgnFlag"];
                ViewData["List"] = db.Emergencies.Where(x => x.catagory_id == temp.Catagory_Id).ToList<Emergency>();
                return View(); 
            }
            return RedirectToAction("Login");
        }
        // Login
        public ActionResult Login()
        {
            return View();
        }

        // Login
        [HttpPost]
        public ActionResult Login(ServiceProvider serviceProvider)
        {
            if (serviceProvider.UserName != null && serviceProvider.Password != null )
            {
                var res = db.ServiceProviders.ToList<ServiceProvider>().Where<ServiceProvider>(x=> x.UserName == serviceProvider.UserName && x.Password == serviceProvider.Password).FirstOrDefault();
                if (res != null)
                {
                    if (res.Sgn_Status == "pending")
                    {
                        ViewData["Message"] = "Your account is under review by the admin";
                    }
                    if (res.Sgn_Status == "rejected")
                    {
                        ViewData["Message"] = "Your account hasbeen Rejected by the admin";
                    }
                    if (res.Sgn_Status == "accepted")
                    {
                        Session.Add("SPLgnFlag", res);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewData["Error"] = "Email/Password may be incorrect";
                }
            }
            if (serviceProvider.UserName == null)
            {
                ModelState.AddModelError("UserName", "UserName field is required"); 
            }
            if (serviceProvider.Password == null)
            {
                ModelState.AddModelError("Password", "Password field is required");
            }
            return View();
        }

        // Register
        public ActionResult Register()
        {
            ViewData["List"] = db.Catagories.ToList<Catagory>();
            return View();
        }

        //POST: Register
        [HttpPost]
        public ActionResult Register(ServiceProvider serviceProvider)
        {
            if (ModelState.IsValid)
            {
                ServiceProvider temp = new ServiceProvider();
                 var res = db.ServiceProviders.Where(x => x.UserName == serviceProvider.UserName);
                foreach (var x in res)
                {
                    temp = db.ServiceProviders.Find(x.Id);
                }
                if (temp.Id == 0)
                {
                    serviceProvider.Sgn_Status = "pending";
                    db.ServiceProviders.Add(serviceProvider);
                    db.SaveChanges();
                    ViewData["Message"] = "Register Success waiting for admin to approve";
                }
                else
                {
                    ViewData["Error"] = "User Name Already taken try another one";
                }
            }
            ViewData["List"] = db.Catagories.ToList<Catagory>();
            return View();
        }

        // AcceptEmergency
        public ActionResult AcceptEmergency(int id)
        {
            if (Session["SPLgnFlag"] != null)
            {
                var temp = db.Emergencies.Find(id);
                return View(temp);
            }
            return RedirectToAction("Login");
        }

        // AcceptEmergency
        [HttpPost]
        public ActionResult AcceptEmergency(Emergency emergency)
        {
            if (Session["SPLgnFlag"] != null)
            {
                ServiceProvider sp = (ServiceProvider)Session["SPLgnFlag"];
                var temp = db.Emergencies.Find(emergency.Id);
                temp.sp_remarks = emergency.sp_remarks;
                temp.status = "accepted";
                temp.sp_id = sp.Id;
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Remove("SPLgnFlag");
            return RedirectToAction("Login");
        }
    }
}