using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class KomentariXML
    {
        public List<Komentar> Komentars { get; set; }
        public KomentariXML()
        {
            Komentars = new List<Komentar>();
        }
    }
}