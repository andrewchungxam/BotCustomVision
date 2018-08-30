using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisAndQandA1.Services
{
    public class OCRResponseClass
    {
    }

    public class Word
    {
        public string boundingBox { get; set; }
        public string text { get; set; }
    }

    public class Line
    {
        public string boundingBox { get; set; }
        public List<Word> words { get; set; }
    }

    public class Region
    {
        public string boundingBox { get; set; }
        public List<Line> lines { get; set; }
    }

    //public class RootObject
    public class OCRRootObject

    {
        public string language { get; set; }
        public string orientation { get; set; }
        public double textAngle { get; set; }
        public List<Region> regions { get; set; }
    }

}