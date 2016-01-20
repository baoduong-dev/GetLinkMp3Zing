using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GetLinkMp3.Models;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.IO.Compression;
using System.Data;
using System.Xml;

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
            string artist = "";
            string linkdown320 = "";
            string linkdownloss = "";
            string imagessong = ReadHtml(url).backimage;
            var url1 = url.LastIndexOf("/");
            var url2 = url.IndexOf(".html");
            var id = url.Substring(url1 + 1, url2 - url1 - 1);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.mp3.zing.vn/api/mobile/song/getsonginfo?requestdata={\"id\":\"" + id + "\"}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            //string text = "";
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
                        //LinkDown dc = js.Deserialize<LinkDown>(data.link_download.ToString());
                        songname = data.title;
                        artist = data.artist;
                        Dictionary<string, string> LocalGroup = data.source;
                        foreach (var item in LocalGroup)
                        {
                            if (item.Key == "320")
                            {
                                linkdown320 = item.Value;
                            }
                            if (item.Key == "lossless")
                            {
                                linkdownloss = item.Value;
                            }
                        }
                        //string[] nameParts = link.ToString().Split(',');
                        //object id1 = link.GetType().GetProperty("320").GetValue(link, null);
                        //string key = data.link_download.GetType().GetProperty(nameOfProperty).GetValue(link).ToString();

                        //linkdown = nameParts[2];
                        //linkdown = data.link_download[3].ToString();
                        //text = readStream.ReadToEnd();
                    }
                }
                ViewBag.name = songname;
                ViewBag.artist = artist;
                ViewBag.linkdown320 = linkdown320;
                ViewBag.linkdownloss = linkdownloss;
                ViewBag.imagessong = imagessong;
                return PartialView();
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected Song ReadHtml(string link)
        {
            string result = "";

            string sContents = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            GZipStream gzstream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            StreamReader reader = new StreamReader(gzstream);
            sContents = reader.ReadToEnd();
            //return sContents;
            Match m = Regex.Match(sContents, @"xmlURL=\s*(.+?)\s*&amp;textad=");
            if (m.Success)
            {
                result = m.Groups[1].Value;
            }
            else
            {
                result = "";
            }


            string sContentsXML = string.Empty;
            HttpWebRequest requestXML = (HttpWebRequest)WebRequest.Create(result);
            HttpWebResponse responseXML = (HttpWebResponse)requestXML.GetResponse();
            GZipStream gzstreamXML = new GZipStream(responseXML.GetResponseStream(), CompressionMode.Decompress);
            //using (XmlReader xmlReader = XmlReader.Create(gzstreamXML, new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Fragment }))
            //{
            //    xmlReader.MoveToContent();
            //    XmlDocument xmlDocument = new XmlDocument();
            //    xmlDocument.LoadXml(xmlReader.ReadContentAsString());
            //}
            StreamReader readerXML = new StreamReader(gzstreamXML);
            //sContentsXML = readerXML.ReadToEnd();

            DataSet ds = new DataSet();//Using dataset to read xml file
            ds.ReadXml(readerXML);
            var song = new Song();
            song = (from rows in ds.Tables[1].AsEnumerable()
                    select new Song
                    {
                        title = rows[0].ToString(),
                        backimage = rows[9].ToString()
                    }).SingleOrDefault();

            return song;
        }


    }
}