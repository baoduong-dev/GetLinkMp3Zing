using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace GetLinkMp3.Models
{
    public class InfoMusic
    {
        public int song_id { get; set; }
        public string title { get; set; }
        //public string Link { get; set; }
        //public string Artist { get; set; }
        public Dictionary<string, string> source { get; set; }
    }
}