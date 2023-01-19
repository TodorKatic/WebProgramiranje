using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models.XMLDATA
{
    public class FitnessXML
    {
        public List<FitnessCentar> FitnessCentars { get; set; }

        public FitnessXML()
        {
            FitnessCentars = new List<FitnessCentar>();
        }
    }
}