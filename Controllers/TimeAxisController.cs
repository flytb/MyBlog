using MyBlog.Services.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class TimeAxisController : Controller
    {
        TimeAxixServices services = new TimeAxixServices();
        // GET: TimeAxis
        public ActionResult Index()
        {
            ViewBag.Title = "Zhangtb_历程";
            var result = services.query.OrderByDescending(q => q.add_time).ToList();
            return View(result);
        }
    }
}