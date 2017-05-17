using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.App_Data
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}