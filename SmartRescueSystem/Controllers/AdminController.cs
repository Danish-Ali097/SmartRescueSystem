using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using SmartRescueSystem.Models;

namespace SmartRescueSystem.Controllers
{
    public class AdminController : Controller
    {
        SmartRescueSystemEntities db = new SmartRescueSystemEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                Admin model = new Admin();
                model.Catagorylist = db.Catagories.ToList<Catagory>();
                model.SPlist = db.ServiceProviders.ToList<ServiceProvider>();
                return View(model);
            }
            return RedirectToAction("Login");
        }

        // Admin Register
        public ActionResult Register()
        {
            return View();
        }

        // Admin Register
        [HttpPost]
        public ActionResult Register(Admin admin)
        {
            if (ModelState.IsValid && admin.Name != null)
            {
                Admin temp = new Admin();
                var res = from x in db.Admins where x.Email == admin.Email select x;
                foreach (var x in res)
                {
                    temp = db.Admins.Find(x.Id);
                }
                //first some validation
                if (temp == null)
                {
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    ViewBag.Message = "Admin Registered";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "This Email is already registered");
                    return View(admin);
                }
            }
            ModelState.AddModelError("Name", "The Name field is required");
            return View(admin);
        }

        // Admin Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin Login
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                Admin temp = new Admin();
                var res = from x in db.Admins where x.Email == admin.Email && x.Password == admin.Password select x;
                foreach (var x in res)
                {
                    temp = db.Admins.Find(x.Id);
                }
                if (temp.Id > 0)
                {
                    Session["AdminLgnFlag"] = temp;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Email or Password may incorrect";
                    return View();
                }
            }
            return View();
        }

        // Admin Logout
        public ActionResult Logout()
        {
            Session.Remove("AdminLgnFlag");
            return RedirectToAction("Login");
        }

        // Add Catagory
        [HttpPost]
        public ActionResult AddCatagory(Catagory catagory)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                db.Catagories.Add(catagory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        // AcceptProvider
        public ActionResult AcceptProvider(int id)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var temp = db.ServiceProviders.Find(id);
                temp.Sgn_Status = "accepted";
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        // RejectProvider
        public ActionResult RejectProvider(int id)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var temp = db.ServiceProviders.Find(id);
                temp.Sgn_Status = "rejected";
                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }
    }
}