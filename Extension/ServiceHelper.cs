using MyBlog.Domian;
using MyBlog.Services.Article;
using MyBlog.Services.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static  class ServiceHelper
    {
        /// <summary>
        /// 获取浏览量最多的top文章
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<user_article> GetFireArtilce(int top)
        {
            UserArticleServices articleServices = new UserArticleServices();
            return articleServices.query.Where(q => q.is_show == true).OrderByDescending(q => q.browse_num).Take(top).ToList();
        }
        /// <summary>
        /// 获取最新发布的top文章
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<user_article> GetNewArtilce(int top)
        {
            UserArticleServices articleServices = new UserArticleServices();
            return articleServices.query.Where(q => q.is_show == true).OrderByDescending(q => q.add_time).Take(top).ToList();
        }
        /// <summary>
        /// 获取站长推荐的top文章
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<user_article> GetRecommendArtilce(int top)
        {
            UserArticleServices articleServices = new UserArticleServices();
            return articleServices.query.Where(q => q.is_show == true).Where(q=>q.is_recommend==true).OrderByDescending(q => q.add_time).Take(top).ToList();
        }

        /// <summary>
        /// 获取评论的回复列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static List<article_comment> GetRelpyMsg(int parentid)
        {
            ArticleCommentServicese commentservices = new ArticleCommentServicese();
            return commentservices.query.Where(q => q.is_del != true).Where(q => q.reply_id == parentid).OrderBy(q=>q.add_time).ToList();
        }

        /// <summary>
        /// 获取现在时间和发布时间 文字距离
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeDistance(DateTime time)
        {
            DateTime now = DateTime.Now;
            string result = "";
            TimeSpan dis = now - time;
            if(dis.Days>0)
            {
                if(dis.Days>30)
                {
                    if(dis.Days > 365)
                    {
                        result = dis.Days / 365 + "年前";
                    }
                    else {
                        result = dis.Days / 30 + "个月前";
                    }
                }
                else
                {
                    result = dis.Days + "天前";
                }
            }
            else
            {
                if(dis.Hours>0)
                {
                    result = dis.Hours + "小时前";
                }
                else
                {
                    if(dis.Minutes>0)
                    {
                        result = dis.Minutes + "分钟前";
                    }
                    else
                    {
                        if (dis.Seconds > 10)
                            result = dis.Seconds + "秒前";
                        else
                            result = "刚刚";
                    }
                }
            }
            return result;
        }
        public static article_type GetArtType(string typeName)
        {
            ArticleTypeServices typeServices = new ArticleTypeServices();
            return typeServices.query.Where(q => q.type_name == typeName).FirstOrDefault();
        }
        public static List<article_type> GetArtTypeList()
        {
            ArticleTypeServices typeServices = new ArticleTypeServices();
            return typeServices.query.Where(q=>q.is_del!=true).ToList();
        }
        public static List<key_word> GetKeyWordList()
        {
            KeyWordServices typeServices = new KeyWordServices();
            return typeServices.query.ToList();
        }
        public static List<key_word> GetKeyWordList(List<int> ids)
        {
            KeyWordServices typeServices = new KeyWordServices();
            return typeServices.query.Where(q=>ids.Contains(q.id)).ToList();
        }

        public static string GetWeatherImg(string type)
        {
            string result = "";
            switch(type)
            {
                case "晴":
                    if (DateTime.Now.Hour < 18)
                        result = "sunny";
                    else result = "night";
                    break;
                case "多云":
                    if (DateTime.Now.Hour < 18)
                        result = "sundy-cloudy";
                    else result = "night-cloudy";
                    break;
                case "阵雨":
                    result = "shower";
                    break;
                case "小雨":
                    result = "rain";
                    break;
                case "中雨":
                    result = "rain";
                    break;
                case "大雨":
                    result = "hearvy-rain";
                    break;
                case "雷阵雨":
                    result = "thunder-shower";
                    break;
                case "阴":
                    result = "day-cloudy";
                    break;
                case "雪":
                    result = "snow";
                    break;
                case "大雪":
                    result = "heavy-snow";
                    break;
                case "雾霾":
                    result = "haze";
                    break;
                case "大雾":
                    result = "fog";
                    break;
                case "雾":
                    result = "fog";
                    break;
                default: result = "null"; break;
            }
            return result;
        }
    }
}