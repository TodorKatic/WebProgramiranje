using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1Proj.Models;
using Web1Proj.Models.XMLDATA;

namespace Web1Proj.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OdrzaniTreninzi()
        {
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<GrupniTrening> trenings = new List<GrupniTrening>();
            foreach(GrupniTrening g in grupniTrenings.ToList())
            {
                if(g.Trener == korisnik.KorisnickoIme && g.Obrisan == false)
                {
                    trenings.Add(g);
                }
            }
            foreach(GrupniTrening gt in trenings.ToList())
            {
                var parsedDate = DateTime.ParseExact(gt.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null);
                if (parsedDate > DateTime.Now)
                {
                    trenings.Remove(gt);
                }
            }
            HttpContext.Application["ProsliTreninzi"] = trenings;
            HttpContext.Application["ProsliTreninziRez"] = trenings;
            return View();
        }
        public ActionResult Sort(string vrsta, string vrednost)
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["ProsliTreninziRez"];
            if (vrednost == "opadajuce")
            {
                if (vrsta == "naziv")
                {
                    grupniTrenings = grupniTrenings.OrderByDescending(gt => gt.Naziv).ToList();
                }
                if (vrsta == "tip")
                    grupniTrenings = grupniTrenings.OrderByDescending(gt => gt.TipTreninga).ToList();
                if (vrsta == "vreme")
                {
                    grupniTrenings = grupniTrenings.OrderByDescending(gt => gt.VremeOdrzavanja).ToList();
                }
            }
            if (vrednost == "rastuce")
            {
                if (vrsta == "naziv")
                {
                    grupniTrenings = grupniTrenings.OrderBy(gt => gt.Naziv).ToList();
                }
                if (vrsta == "tip")
                    grupniTrenings = grupniTrenings.OrderBy(gt => gt.TipTreninga).ToList();
                if (vrsta == "vreme")
                {
                    grupniTrenings = grupniTrenings.OrderBy(gt => gt.VremeOdrzavanja).ToList();
                }
            }
            HttpContext.Application["ProsliTreninzi"] = grupniTrenings;
            return View("OdrzaniTreninzi");
        }
        public ActionResult Search(string naziv, string tip, FormCollection form)
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["ProsliTreninziRez"];
            //grupniTrenings = grupniTrenings.OrderBy(g => g.Naziv).ToList();
            string od=form.Get("od");
            string Do = form.Get("do");

            if (naziv != "")
            {
                grupniTrenings=VratiPoNazivu(grupniTrenings, naziv);
            }
            if (tip != "")
                grupniTrenings = VratiPoTipu(grupniTrenings, tip);
            if (od!= "")
            {
                DateTime.TryParse(od, out DateTime donja);
                grupniTrenings=VratiPoDonjoj(grupniTrenings, donja);
            }
            if (Do != "")
            {
                DateTime.TryParse(Do, out DateTime gornja);
                grupniTrenings=VratiPoGornjoj(grupniTrenings, gornja);
            }

            HttpContext.Application["ProsliTreninzi"] = grupniTrenings;


            return View("OdrzaniTreninzi");
        }
        public List<GrupniTrening> VratiPoNazivu(List<GrupniTrening> treninzi, string naziv)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.Naziv.Contains(naziv))
                    tempTreninzi.Add(f);
            }

            return tempTreninzi;
        }
        #region Pomocne
        public List<GrupniTrening> VratiPoTipu(List<GrupniTrening> treninzi, string tip)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if (f.TipTreninga.ToString() == tip)
                    tempTreninzi.Add(f);
            }

            return tempTreninzi;
        }

        public List<GrupniTrening> VratiPoDonjoj(List<GrupniTrening> treninzi, DateTime date)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if ((DateTime.ParseExact(f.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null))  >= date)
                    tempTreninzi.Add(f);
            }

            return tempTreninzi;
        }

        public List<GrupniTrening> VratiPoGornjoj(List<GrupniTrening> treninzi, DateTime date)
        {
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();
            foreach (var f in treninzi)
            {
                if ((DateTime.ParseExact(f.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null)) < date)
                    tempTreninzi.Add(f);
            }

            return tempTreninzi;
        }
        #endregion

        public ActionResult PrikaziPosetioce(string naziv)
        {
            List<GrupniTrening> trenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            GrupniTrening trening = new GrupniTrening();
            List<Posetilac> posetiociPrikaz = new List<Posetilac>();
            foreach(GrupniTrening gt in trenings.ToList())
            {
                if (gt.Naziv == naziv && gt.Obrisan == false){
                    trening = gt;
                    break;
                }
            }
            List<string> posetioci = trening.Posetioci;
            foreach(string s in posetioci)
            {
                foreach(Posetilac p in posetilacs)
                {
                    if (s == p.KorisnickoIme)
                    {
                        posetiociPrikaz.Add(p);
                    }
                }
            }
            HttpContext.Application["PosetiociPrikaz"] = posetiociPrikaz;
            return View();
        }

        public ActionResult PredstojeciTreninzi()
        {
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<GrupniTrening> temp = new List<GrupniTrening>();
            foreach(GrupniTrening g in grupniTrenings.ToList())
            {
                if(g.Trener == korisnik.KorisnickoIme && g.Obrisan==false)
                {
                    temp.Add(g);
                }
            }
            foreach(GrupniTrening g in temp.ToList())
            {
                var parsedDate = DateTime.ParseExact(g.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null);
                DateTime datum = Convert.ToDateTime(parsedDate);
                if (datum < DateTime.Now)
                    temp.Remove(g);
            }
            HttpContext.Application["BuduciTreninzi"] = temp;
            return View();

        }
        public ActionResult ObrisiTrening(string naziv)
        {
            List<GrupniTrening> buduci = (List<GrupniTrening>)HttpContext.Application["BuduciTreninzi"];
            GrupniTrening trening = new GrupniTrening();
            foreach(GrupniTrening g in buduci)
            {
                if(g.Naziv==naziv && g.Obrisan == false)
                {
                    trening = g;
                    break;
                }
            }
            if (trening.Posetioci.Count() > 0)
            {
                ViewBag.Message = "Postoje prijavljeni korisnici za trening, nemoguce ga je izbrisati!";
                return View("PredstojeciTreninzi");
            }
            trening.Obrisan = true;
            foreach(GrupniTrening g in buduci.ToList())
            {
                if (g.Naziv == trening.Naziv)
                {
                    buduci.Remove(g);
                }
            }
            buduci.Add(trening);
            IOXML.AzurirajTreninge(trening);
            HttpContext.Application["Treninzi"] = IOXML.IzvlaciTreninge().GrupniTrenings;

            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            return RedirectToAction("PredstojeciTreninzi");
        }
        public ActionResult NoviTrening()
        {
            return View("KreirajTrening");
        }
        public ActionResult KreirajTrening(string naziv, string tip, string trajanje, DateTime datum, string ucesnika)
        {
            List<GrupniTrening> trenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<Trener> treneri = (List<Trener>)HttpContext.Application["Treneri"];
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Trener trener = new Trener();
            foreach(Trener t in treneri)
            {
                if (t.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    trener = t;
                    break;
                }
            }
            ViewBag.Message = "";
            if((!(Int32.TryParse(trajanje, out int trajanjeRes)) || trajanjeRes < 0)|| (!(Int32.TryParse(ucesnika, out int maxRes)) && maxRes <= 0))
            {
                ViewBag.Message = "Broj posetilaca i vreme trajanja moraju biti pozitivni brojevi";
                return View("KreirajTrening");
            }
            if (naziv == "" )
            {
                ViewBag.Message = "Nepravilan unos podataka";
                return View("KreirajTrening");
            }
            if (datum < DateTime.Now.AddDays(3))
            {
                ViewBag.Message = "Trening mora biti kreiran minimalno 3 dana unapred";
                return View("KreirajTrening");
            }
            foreach(GrupniTrening g in trenings)
            {
                if (g.Naziv == naziv)
                {
                    ViewBag.Message = "Trening s tim imenom vec postoji";
                    return View("KreirajTrening");
                }
            }
           
            Enums.TipTreninga TipTr = (Enums.TipTreninga)Enum.Parse(typeof(Enums.TipTreninga), tip);
            foreach (Trener t in treneri)
            {
                if (t.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    trener = t;
                    break;
                }
            }

            GrupniTrening grupniTrening = new GrupniTrening(naziv, TipTr, trener.FitnessCentar, trajanjeRes, datum.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), maxRes, trener.KorisnickoIme);
            trener.Treninzi.Add(naziv);
            treneri.RemoveAll(x => x.KorisnickoIme == trener.KorisnickoIme);
            treneri.Add(trener);
            HttpContext.Application["Treneri"] = treneri;
            trenings.Add(grupniTrening);
            HttpContext.Application["Treninzi"] = trenings;
            IOXML.AzurirajTrenere(trener);
            IOXML.AzurirajTreninge(grupniTrening);

            return RedirectToAction("Index", "Home");

        }
        public ActionResult Modifikuj(string naziv)
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["BuduciTreninzi"];
            GrupniTrening grupni = new GrupniTrening();
            foreach (GrupniTrening gt in grupniTrenings)
            {
                if (gt.Naziv == naziv)
                {
                     grupni = gt;
                    break;
                }
            }
            HttpContext.Application["Trening"] = grupni;
            return View();
        }
        public ActionResult Modifikacija(string tip, string trajanje, string ucesnika, DateTime datum)
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];

            GrupniTrening trening = (GrupniTrening)HttpContext.Application["Trening"];
            if(Enum.TryParse(tip, out Enums.TipTreninga tipTr))
                trening.TipTreninga = tipTr;
            if(trajanje=="" || ucesnika == "")
            {
                ViewBag.Message = "Vreme trajanja i broj ucesnika moraju biti popunjeni";
                return View("Modifikuj");
            }
            else
            {
                int trajanjeTr = int.Parse(trajanje);
                int brUc = int.Parse(ucesnika);
                if(trajanjeTr<=0 || brUc<= 0)
                {
                    ViewBag.Message = "Vreme trajanja i broj ucesnika moraju biti pozitivni brojevi!";
                    return View("Modifikuj");
                }
                if(brUc< trening.Posetioci.Count())
                {
                    ViewBag.Message = "Ne moze se smanjiti kapacitet zbog broja prijavljenih!";
                    return View("Modifikuj");
                }
                trening.Trajanje = trajanjeTr;
                trening.MaxBroj = brUc;
            }
            if (datum < DateTime.Now.AddDays(3))
            {
                ViewBag.Message = "Trening mora biti kreiran minimalno 3 dana unapred";
                return View("Modifikuj");
            }
            trening.VremeOdrzavanja = datum.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            grupniTrenings.RemoveAll(x => x.Naziv == trening.Naziv);
            grupniTrenings.Add(trening);
         
            
           
            IOXML.AzurirajTreninge(trening);
            return RedirectToAction("PredstojeciTreninzi");
        }
    }
}