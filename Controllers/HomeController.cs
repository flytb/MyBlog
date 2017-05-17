using MyBlog.Models.Search;
using MyBlog.Services.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MyBlog.Core.Extension;
using MyBlog.Domian;
using MyBlog.Models.Article;
using MyBlog.Extension;
using MyBlog.Services.Public;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        UserArticleServices articleDB = new UserArticleServices();
        public ActionResult Index()
        {
            ViewBag.Title = "Zhangtb_博客";
            SearchModel model = new SearchModel()
            {
                PageIndex = 1,
                PageSize = 7,
            };
            return View(model);
        }
        public ActionResult ArticlList(SearchModel model)
        {
            var result=articleDB.query.Where(q => q.is_show == true).OrderByDescending(q => q.add_time).ToPagedList(model.PageIndex, model.PageSize);
            return PartialView(result);
        }
        public ActionResult GetMoreArticle(int skip,int num)
        {
            JsonResult<List<ArticlListModel>> result = new JsonResult<List<ArticlListModel>>();
            List<ArticlListModel> articls = new List<ArticlListModel>();
            var list = articleDB.query.Where(q => q.is_show == true).OrderByDescending(q => q.add_time).Skip(skip).Take(num).ToList();
            if(list.Count()>0)
            {
                foreach(var item in list)
                {
                    articls.Add(new ArticlListModel() {
                        ID = item.id,
                        AddTime=item.add_time.Value.ToString("yyyy-MM-dd"),
                        Abstract=item.@abstract,
                        BrowseNum=(int)item.browse_num,
                        Title=item.title,
                        TypeName=item.article_type.type_name,
                        CommentNum=item.article_comment.Where(q=>q.is_del!=true).Count(),
                        Image=item.image,
                    });
                }
                result.Success(articls);
            }
            else
            {
                result.Fail("没有更多的文章啦！");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Banner()
        {
            return PartialView();
        }
        public ActionResult Follow()
        {
            IpCity ipcity = ApiHelper.GetCityForIp(UserAddress.GetUserAddress());
            ViewBag.Weather = ApiHelper.GetCityWeather(ipcity!=null?string.IsNullOrEmpty(ipcity.city) ? "福州" : ipcity.city: "福州");
            return PartialView();
        }
        public ActionResult ArticleTab()
        {
            return PartialView();
        }
        public ActionResult Cloud()
        {
            return PartialView();
        }
        public ActionResult TuWen()
        {
            return PartialView();
        }
        public ActionResult Links()
        {
            return PartialView();
        }


    }
}