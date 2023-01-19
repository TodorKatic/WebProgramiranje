using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class PosetiociXML
    {
        public List<Posetilac> Posetilacs { get; set; }
        public PosetiociXML()
        {
            Posetilacs = new List<Posetilac>();

        }
    }
}