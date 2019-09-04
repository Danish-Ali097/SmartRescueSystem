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
            return View();
        }
    }
}