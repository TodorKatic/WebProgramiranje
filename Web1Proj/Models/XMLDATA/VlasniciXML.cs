using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class VlasniciXML
    {
        public List<Vlasnik> Vlasniks { get; set; }
        public VlasniciXML()
        {
            Vlasniks = new List<Vlasnik>();
        }
    }
}