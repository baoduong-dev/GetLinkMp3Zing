using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GetLinkMp3.Models;
using System.Net;
using System.IO;

namespace GetLinkMp3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult GetLink(string url)
        {
            string songname = "";
            string linkdown = "";
            var url1 = url.LastIndexOf("/");
            var url2 = url.IndexOf(".html");
            var id = url.Substring(url1 + 1, url2 - url1 - 1);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.mp3.zing.vn/api/mobile/song/getsonginfo?requestdata={\"id\":\"" + id + "\"}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            string text = "";
            try
            {
                //Google recaptcha Responce
                using (WebResponse wResponse = httpWebRequest.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        InfoMusic data = js.Deserialize<InfoMusic>(jsonResponse);// Deserialize Json

                        songname = data.title;
                        //Object link = data.link_download;
                        //string[] nameParts = link.ToString().Split(',');

                        //string key = data.link_download.GetType().GetProperty(nameOfProperty).GetValue(link).ToString();

                        //linkdown = nameParts[2];
                        //linkdown = data.link_download[3].ToString();
                        //text = readStream.ReadToEnd();
                    }
                }
                ViewBag.name = songname;
                //ViewBag.linkdown = linkdown;
                return PartialView(); ;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}