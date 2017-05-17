using MyBlog.Models.Search;
using MyBlog.Services.Article;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(int art=0,int key=0)
        {
            ViewBag.Title = "Zhangtb_文章";
            ArtSearchModel model = new ArtSearchModel();
            model.PageSize = 6;
            model.PageIndex = 1;
            model.ArtType = art;
            model.KeyID = key;
            model.Order = 1;
            model.Title = "";
            return View(model);
        }

        public ActionResult List(int PageIndex,int PageSize,int ArtType,int KeyID,int Order,string Title)
        {
            ViewBag.ArtType =ArtType;
            ViewBag.KeyID = KeyID;
            ViewBag.Title = Title;
            ViewBag.Order = Order;
            UserArticleServices artServices = new UserArticleServices();
            artServices.WhereIf(ArtType > 0, q => q.type_id == ArtType);
            artServices.WhereIf(KeyID > 0, q => q.key_word.Contains(","+ KeyID.ToString()+"," ));
            artServices.WhereIf(!string.IsNullOrEmpty(Title), q => q.title.Contains(Title));
            if (Order == 1)
            {
                var result = artServices.query.Where(q => q.is_del != true).Where(q => q.is_show == true).OrderByDescending(q=>q.add_time).ToPagedList(PageIndex, PageSize);
                return PartialView(result);
            }
            else
            {
                var result = artServices.query.Where(q => q.is_del != true).Where(q => q.is_show == true).OrderByDescending(q => q.browse_num).ToPagedList(PageIndex, PageSize);
                return PartialView(result);
            }
        }
    }
}