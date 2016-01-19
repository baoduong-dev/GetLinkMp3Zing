using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GetLinkMp3.Models
{
    public class InfoMusic
    {
        public int song_id { get; set; }
        public string title { get; set; }
        //public string Link { get; set; }
        public string artist { get; set; }
        public Dictionary<string, string> source { get; set; }
    }

    [Serializable]
    [XmlRoot("data"), XmlType("item")]
    public class Song
    {
        public string title { get; set; }
        public string backimage { get; set; }
    }
}