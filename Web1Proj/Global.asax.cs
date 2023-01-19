using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web1Proj.Models;
using Web1Proj.Models.XMLDATA;

namespace Web1Proj
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //List<Posetilac> posetioci = IOXML.IzvlaciPosetioce().Posetilacs;
            HttpContext.Current.Application["Posetioci"] = IOXML.IzvlaciPosetioce().Posetilacs;
            HttpContext.Current.Application["Treneri"] = IOXML.IzvlaciTrenere().Treners;
            HttpContext.Current.Application["Vlasnici"] = IOXML.IzvlaciVlasnike().Vlasniks;
            HttpContext.Current.Application["FitnessCentri"] = IOXML.IzvlaciFitnese().FitnessCentars;
            HttpContext.Current.Application["Komentari"] = IOXML.IzvlaciKomentare().Komentars;
            HttpContext.Current.Application["Treninzi"] = IOXML.IzvlaciTreninge().GrupniTrenings;

        }
    }
}
