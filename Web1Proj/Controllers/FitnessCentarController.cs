using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1Proj.Models;

namespace Web1Proj.Controllers
{
    public class FitnessCentarController : Controller
    {
        // GET: FitnessCentar
        public ActionResult DetaljanPrikaz(FormCollection form)
        {
            string imeFC = form.Get("nazivCentra");
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<GrupniTrening> tempTrening = new List<GrupniTrening>();
            List<Komentar> komentars = (List<Komentar>)HttpContext.Application["Komentari"];
            List<Komentar> temp = new List<Komentar>();
            FitnessCentar fc = new FitnessCentar();
            
            tempTrening = grupniTrenings.FindAll(g => g.FitnesCentar == imeFC && g.Obrisan == false);
            foreach(FitnessCentar f in fitnessCentars)
            {
                if(f.Naziv == imeFC)
                {
                    fc = f;
                    break;
                }
            }
            HttpContext.Application["FitnessCentar"] = fc;
            

            foreach (GrupniTrening g in tempTrening.ToList())
            {
                var parsedDate = DateTime.ParseExact(g.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null);
                if (parsedDate< DateTime.Now)
                {
                    tempTrening.Remove(g);
                }
            }
            foreach(Komentar k in komentars)
            {
                if(k.FitnessCentar == imeFC && k.Odobren==true)
                {
                    temp.Add(k);
                }
            }
            HttpContext.Application["IzabraniKomentari"] = temp;
            HttpContext.Application["IzabraniTreninzi"] = tempTrening;
            return View();
        }
        
    }
}