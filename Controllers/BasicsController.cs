using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class BasicsController : Controller
    {
        // GET: Basics
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}