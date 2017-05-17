using MyBlog.Models.Image;
using MyBlog.Services.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class ImageController : Controller
    {
        AlbumServices albumServices = new AlbumServices();
        AlbumImgServices imgservices = new AlbumImgServices();
        public ActionResult Index()
        {
            ViewBag.Title = "Zhangtb_相册";
            return View();
        }

        public ActionResult List()
        {
            var result = albumServices.query.Where(q=>q.is_del!=true).OrderByDescending(q=>q.add_time).ToList();
            return PartialView(result);
        }
        public ActionResult PhotosList(int album=0)
        {
            if(album<=0)
            {
                return RedirectToAction("Index", "Image");
            }
            var result = albumServices.query.Where(q => q.is_del != true).Where(q=>q.id== album).FirstOrDefault();
            ViewBag.Album = album;
            ViewBag.Title = result.title;
            return View(result);
        }
        public ActionResult GetPhotos(int num,int album)
        {
            var Album=albumServices.query.Where(q => q.id == album).FirstOrDefault();
            IamgesModel model = new IamgesModel();
            if (Album != null)
            {
                model.id = 1;
                model.start = num;
                model.title = "相册集";
                List<ImageData> data = new List<ImageData>();
                foreach(var item in Album.album_img.Where(q=>q.is_del!=true))
                {
                    data.Add(new ImageData { alt = item.img_name, pid = item.id, src = item.src, thumb = item.sub_src });
                }
                model.data = data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetThunmbPhotos(int album,int skip,int num)
        {
            List<ImageData> data = new List<ImageData>();
            var imgs=imgservices.query.Where(q => q.alb_id == album).Where(q => q.is_del != true).OrderBy(q=>q.id).Skip(skip).Take(num);
            foreach (var item in imgs)
            {
                data.Add(new ImageData { alt = item.img_name, pid = item.id, src = item.src, thumb = item.sub_src });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}