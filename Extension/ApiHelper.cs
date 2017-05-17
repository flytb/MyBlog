using MyBlog.Core.Extension;
using MyBlog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MyBlog.Extension
{
    public static class ApiHelper
    {
        static string Url = ConfigurationManager.AppSettings["Weather"].ToString();
        public static Weather GetCityWeather(string city)
        {
            string sendURL = Url + city;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendURL);
            HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
            GZipStream gzip = new GZipStream(respon.GetResponseStream(), CompressionMode.Decompress);
            StreamReader sw = new StreamReader(gzip);
            string value = sw.ReadToEnd();
            return JsonConvert.DeserializeObject<Weather>((value));
        }

        public static IpCity GetCityForIp(string ip)
        {
            string cityurl = ConfigurationManager.AppSettings["IpCity"].ToString()+"&ip="+ip;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cityurl);
            HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
            StreamReader stream = new StreamReader(respon.GetResponseStream(), Encoding.UTF8);
            try
            {
                return JsonConvert.DeserializeObject<IpCity>(stream.ReadToEnd());
            }
            catch
            {
                return null;
            }
        }
    }
}