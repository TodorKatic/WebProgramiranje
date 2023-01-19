using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1Proj.Models;
using Web1Proj.Models.XMLDATA;

namespace Web1Proj.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PregledTreninga()
        {
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Posetilac posetilac = new Posetilac();
            foreach(Posetilac p in posetilacs)
            {
                if(p.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    posetilac = p;
                    break;
                }
            }
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<GrupniTrening> grupniTrenings = new List<GrupniTrening>();
            foreach(string t in posetilac.GrupniTreninzi)
            {
                foreach(GrupniTrening gt in treninzi)
                {
                    if(t==gt.Naziv && gt.Obrisan == false)
                    {
                        grupniTrenings.Add(gt);
                    }
                }
            }
            
          
            foreach (GrupniTrening g in grupniTrenings.ToList())
            {
                var parsedDate = DateTime.ParseExact(g.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null);
                if (parsedDate > DateTime.Now)
                {
                    grupniTrenings.Remove(g);
                }
            }
            HttpContext.Application["OdrzaniTreninzi"] = grupniTrenings;
            HttpContext.Application["OdrzaniTreninziRez"] = grupniTrenings;
            return View();
        }

        public ActionResult Search(string naziv, string tip, string fitnessCentar)
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["OdrzaniTreninziRez"];
            List<GrupniTrening> temp = new List<GrupniTrening>();
            bool imalNes = false;
           

            if(naziv != "")
            {
                imalNes = true;
                foreach (GrupniTrening fc in grupniTrenings.ToList())
                {
                    if (fc.Naziv.Contains(naziv))
                    {
                        temp.Add(fc);

                    }
                }
            }
            if (tip != "")
            {

                if (imalNes)
                {
                    foreach (GrupniTrening gt in temp.ToList())
                    {
                        if (gt.TipTreninga.ToString() !=tip)
                        {
                            temp.Remove(gt);
                        }
                    }
                }
                else
                {
                    foreach (GrupniTrening gt in grupniTrenings.ToList())
                    {
                        if (gt.TipTreninga.ToString() == tip)
                        {
                            temp.Add(gt);
                        }
                    }
                }
                imalNes = true;
            }
            if (fitnessCentar != "")
            {

                if (imalNes)
                {
                    foreach (GrupniTrening gt in temp.ToList())
                    {
                        if (!gt.FitnesCentar.Contains(fitnessCentar))
                        {
                            temp.Remove(gt);
                        }
                    }
                }
                else
                {
                    foreach (GrupniTrening gt in grupniTrenings.ToList())
                    {
                        if (gt.FitnesCentar.Contains(fitnessCentar))
                        {
                            temp.Add(gt);
                        }
                    }
                }
                imalNes = true;
            }

            if (imalNes)
                HttpContext.Application["OdrzaniTreninzi"] = temp;
            else
                HttpContext.Application["OdrzaniTreninzi"] = grupniTrenings;
            return View("PregledTreninga");
        }

        public ActionResult Sort(string parametar, string vrednost) 
        {
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["OdrzaniTreninzi"];
            List<GrupniTrening> tempTreninzi = new List<GrupniTrening>();

            if (vrednost == "rastuce")
            {
                if(parametar == "naziv")
                {
                    tempTreninzi = grupniTrenings.OrderBy(t => t.Naziv).ToList();
                }
                if (parametar == "tip")
                {
                    tempTreninzi = grupniTrenings.OrderBy(t => t.TipTreninga).ToList();
                }
                if (parametar == "vreme")
                {
                    tempTreninzi = grupniTrenings.OrderBy(t => t.VremeOdrzavanja).ToList();
                }
            }
            else
            {
                if (parametar == "naziv")
                {
                    tempTreninzi = grupniTrenings.OrderByDescending(t => t.Naziv).ToList();
                }
                if (parametar == "tip")
                {
                    tempTreninzi = grupniTrenings.OrderByDescending(t => t.TipTreninga).ToList();
                }
                if (parametar == "vreme")
                {
                    tempTreninzi = grupniTrenings.OrderByDescending(t => t.VremeOdrzavanja).ToList();
                }
            }
            HttpContext.Application["OdrzaniTreninzi"] = tempTreninzi;
            return View("PregledTreninga");
        }
        public ActionResult Prijava(string naziv)
        {
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];

            GrupniTrening grupniTrening = new GrupniTrening();
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Posetilac posetilac = new Posetilac();

            List < Trener > treners = (List<Trener>)HttpContext.Application["Treneri"];
            Trener trener = new Trener();
            foreach(Posetilac p in posetilacs)
            {
                if (p.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    posetilac = p;
                    break;
                }
                    

            }
            foreach(GrupniTrening g in grupniTrenings)
            {
                if(g.Naziv == naziv)
                {
                    grupniTrening = g;
                    break;
                }
            }

            if(posetilac.GrupniTreninzi.Contains(naziv) || grupniTrening.Posetioci.Count >= grupniTrening.MaxBroj)
            {
                ViewBag.Message = "Nije moguce prijaviti se na trening!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                posetilac.GrupniTreninzi.Add(naziv);
                foreach(Posetilac p in posetilacs.ToList())
                {
                    if(p.KorisnickoIme == posetilac.KorisnickoIme)
                    {
                        posetilacs.Remove(p);
                    }
                }
                posetilacs.Add(posetilac);
                IOXML.AzurirajPosetioce(posetilac);
                grupniTrening.Posetioci.Add(posetilac.KorisnickoIme);
                foreach(GrupniTrening g in grupniTrenings.ToList())
                {
                    if (g.Naziv == grupniTrening.Naziv)
                    {
                        grupniTrenings.Remove(g);
                    }
                }
                grupniTrenings.Add(grupniTrening);
                IOXML.AzurirajTreninge(grupniTrening);

                HttpContext.Application["Posetioci"] = posetilacs;
                HttpContext.Application["Treninzi"] = grupniTrenings;


              /*  foreach (var t in treners)
                {
                    foreach (var trening in t.Treninzi)
                    {
                        if (trening.Naziv == treningNaz)
                        {
                            trener = t;
                            break;
                        }
                    }
                }*/
                
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Komentar()
        {
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Posetilac posetilac = new Posetilac();
            foreach(Posetilac p in posetilacs)
            {
                if(p.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    posetilac = p;
                    break;
                }
            }
            List<GrupniTrening> trenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<string> fitnessCentars = new List<string>();
            GrupniTrening grupniTrening = new GrupniTrening();
            List<Komentar> komentars = (List<Komentar>)HttpContext.Application["Komentari"];

            foreach(string t in posetilac.GrupniTreninzi)
            {
                foreach(GrupniTrening g in trenings)
                {
                    if(g.Naziv == t)
                    {
                        grupniTrening = g;
                        break;
                    }
                }
                var parsedDate = DateTime.ParseExact(grupniTrening.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null);
                if (parsedDate < DateTime.Now)
                {
                    if (fitnessCentars.Contains(grupniTrening.FitnesCentar))
                        continue;
                    fitnessCentars.Add(grupniTrening.FitnesCentar);
                }
            }
            foreach (string fc in fitnessCentars)
            {
                foreach(Komentar k in komentars.ToList())
                {
                    if(k.FitnessCentar== fc && posetilac.KorisnickoIme == k.Posetilac)
                    {
                        komentars.Remove(k);
                    }
                }
            }
            if (fitnessCentars.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Application["CentriZaKomentarisanje"] = fitnessCentars;
            return View(fitnessCentars);
        }
        public ActionResult KomentarPostavi(string ocena, string tekst, string fitnessCentar)
        {
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Komentar komentar = new Komentar();
            List<Komentar> komentars = (List<Komentar>)HttpContext.Application["Komentari"];
            if (tekst == "")
            {
                ViewBag.Message = "Morate uneti tekst komentara";
                return View("Komentar");
            }
            komentar = new Komentar(korisnik.KorisnickoIme, fitnessCentar, tekst, int.Parse(ocena));
            komentars.Add(komentar);
            IOXML.AzurirajKomentare(komentar);
            return RedirectToAction("Index", "Home");
        }
    }
}