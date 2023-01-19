using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class TreneriXML
    {
        public List<Trener> Treners { get; set; }
        public TreneriXML()
        {
            Treners = new List<Trener>();
        }
    }
}