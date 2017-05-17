using MyBlog.Core.Extension;
using MyBlog.Domian;
using MyBlog.Services.Message;
using MyBlog.Services.Public;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            ViewBag.Title = "Zhangtb_留言";
            return View();
        }

        public ActionResult List(int pageindex, int pagesize)
        {
            MessageServices msgservices = new MessageServices();
            var result=msgservices.query.Where(q => q.is_del != true).OrderByDescending(q => q.msg_time).ToPagedList(pageindex, pagesize);
            return PartialView(result);
        }

        public ActionResult MessageIng(string msg)
        {
            MessageServices msgServices = new MessageServices();
            JsonResult<string> result = new JsonResult<string>();
            string userip=UserAddress.GetUserAddress();
            //10分钟内，同一ip只可留言一次
            var newMsg = msgServices.query.Where(q => q.user_ip == userip).OrderByDescending(q => q.msg_time).FirstOrDefault();
            if(newMsg!=null&&((TimeSpan)(DateTime.Now-newMsg.msg_time)).TotalMinutes<10)
            {
                result.Fail("已留言，"+ (10-((TimeSpan)(DateTime.Now - newMsg.msg_time)).TotalMinutes).ToString("F0") + "分钟内无法再次留言");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            message add = new message();
            add.is_del = false;
            add.msg_time = DateTime.Now;
            add.content = msg;
            add.user_ip = userip;
            if(msgServices.Add(add)>0)
            {
                result.Success();
            }
            else
            {
                result.Fail("留言失败");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}