using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAES_Test.Models
{
    public class ResultDiff
    {
        public bool AreEqual { get; set; }
        public bool SameSize { get; set; }
        public int LetfSize { get; set; }
        public int RightSize { get; set; }
        public JObject Diffs { get; set; }
    }
}