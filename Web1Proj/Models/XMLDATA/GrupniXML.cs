using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class GrupniXML
    {
        public List<GrupniTrening> GrupniTrenings { get; set; }
        public GrupniXML()
        {
            GrupniTrenings = new List<GrupniTrening>();
        }
    }
}