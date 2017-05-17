using MyBlog.Core.Extension;
using MyBlog.Domian;
using MyBlog.Services.Article;
using MyBlog.Services.Public;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class ArticleController : Controller
    {
        public const string SessionArt = "ARTICLE_";
        // GET: Article
        public ActionResult Index(int id=0)
        {
            UserArticleServices artservices = new UserArticleServices();
            var result = artservices.query.Where(q => q.id == id).FirstOrDefault();
            if(result==null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Upper = artservices.query.Where(q => q.id < id).OrderByDescending(q => q.add_time).FirstOrDefault();
            ViewBag.Next = artservices.query.Where(q => q.id > id).OrderBy(q => q.add_time).FirstOrDefault();
            ViewBag.Title = result.title;

            //记住用户浏览记录
            if(Session[SessionArt + id]==null)
            {
                Session[SessionArt + id] = true;
                result.browse_num = result.browse_num != null ? result.browse_num + 1 : 1;
                artservices.Update(result);
            }
            return View(result);
        }

        /// <summary>
        /// 文章评论列表
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult ArtMessage(int artid ,int pageindex,int pagesize)
        {
            ArticleCommentServicese commnetServices = new ArticleCommentServicese();
            var result=commnetServices.query.Where(q => q.article_id == artid).Where(q => q.is_del != true).Where(q => q.is_reply != true).OrderBy(q => q.add_time).ToPagedList(pageindex, pagesize);
            ViewBag.pageindex = pageindex;
            return PartialView(result);
        }

        /// <summary>
        /// 点赞、踩
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="is_up"></param>
        /// <returns></returns>
        public ActionResult LikeMsg(int msgid,bool is_up)
        {
            JsonResult<string> result = new JsonResult<string>();
            CommentLikeServices likeservices = new CommentLikeServices();
            var userAddress=UserAddress.GetUserAddress();
            var lieks = likeservices.query.Where(q => q.comment_id == msgid).ToList();
            var like = likeservices.query.Where(q => q.comment_id == msgid).Where(q => q.user_ip == userAddress).FirstOrDefault();
            if(like!=null)
            {
                if(like.is_like==is_up)
                {
                    result.Fail("");
                }
                else
                {
                    like.is_like = is_up;
                    if (likeservices.Update(like) > 0)
                    {
                        result.Success();
                    }
                    else
                    {
                        result.Fail("");
                    }
                }
            }
            else
            {
                comment_like add = new comment_like();
                add.is_like = is_up;
                //add.user_id
                add.user_ip = userAddress;
                add.add_time = DateTime.Now;
                add.comment_id = msgid;
                if(likeservices.Add(add)>0)
                {
                    result.Success();
                }else
                {
                    result.Fail("");
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="replymsg"></param>
        /// <returns></returns>
        public ActionResult ReplyMsg(int msgid,string replymsg)
        {
            JsonResult<string> result = new JsonResult<string>();
            ArticleCommentServicese commnetServices = new ArticleCommentServicese();
            var msg = commnetServices.query.Where(q => q.id == msgid).Where(q=>q.is_del!=true).FirstOrDefault();
            if(msg!=null)
            {
                article_comment add = new article_comment();
                add.add_time = DateTime.Now;
                add.is_del = false;
                add.is_reply = true;
                add.content = replymsg;
                add.reply_id = msgid;
                add.article_id = msg.article_id;
                if(commnetServices.Add(add)>0)
                {
                    result.Success();
                }
                else
                {
                    result.Fail("无法进行回复");
                }
            }
            else
            {
                result.Fail("该评论不存在，无法回复");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArtReplyMsg(int artreplyid, string artreplymsg)
        {
            JsonResult<string> result = new JsonResult<string>();
            ArticleCommentServicese commnetServices = new ArticleCommentServicese();
            if (artreplyid > 0)
            {
                article_comment add = new article_comment();
                add.add_time = DateTime.Now;
                add.is_del = false;
                add.is_reply = false;
                add.content = artreplymsg;
                add.reply_id = null;
                add.article_id = artreplyid;
                if (commnetServices.Add(add) > 0)
                {
                    result.Success();
                }
                else
                {
                    result.Fail("无法进行回复");
                }
            }
            else
            {
                result.Fail("该文章不存在，无法回复");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}