using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1Proj.Models;
using Web1Proj.Models.XMLDATA;

namespace Web1Proj.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult Index()
        {
            return View();
        }
        #region Registruj
        public ActionResult Registruj()
        {
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];

            Korisnik korisnik = (Korisnik)HttpContext.Session["UlogovaniKorisnik"];
            Vlasnik vlasnik = new Vlasnik();
            foreach (Vlasnik v in vlasniks)
            {
                if (v.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    vlasnik = v;
                    break;
                }
            }
            HttpContext.Application["Vlasnik"] = vlasnik;
            return View();
        }
        public ActionResult RegistrujKor(string korisnickoIme, string ime, string prezime, string pol, string email, DateTime datumRodjenja, string centar)
        {
            ViewBag.Message = "";
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];

            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];

            if (korisnickoIme == "" || ime == "" || prezime == "" || email == "")
            {
                ViewBag.Message = "Sva polja se moraju popuniti";
                return View("Registruj");
            }
            if (datumRodjenja.AddYears(21) > DateTime.Now)
            {
                ViewBag.Message = "Trener suvise mlad";
                return View("Registruj");
            }
            foreach (Posetilac p in posetilacs)
            {
                if (p.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.Message = "Korisnik sa navedenim korisnickim imenom vec postoji!";
                    return View("Registruj");
                }
            }
            foreach (Trener t in treners)
            {
                if (t.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.Message = "Korisnik sa navedenim korisnickim imenom vec postoji!";
                    return View("Registruj");
                }
            }
            foreach (Vlasnik v in vlasniks)
            {
                if (v.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.Message = "Korisnik sa navedenim korisnickim imenom vec postoji!";
                    return View("Registruj");
                }
            }
            Enum.TryParse(pol, out Enums.Pol poll);

            Trener trener = new Trener(korisnickoIme, "JMGB", ime, prezime, poll, email, datumRodjenja.ToString("dd/MM/yyyy"), centar);
            treners.Add(trener);
            IOXML.AzurirajTrenere(trener);
            HttpContext.Application["Treneri"] = treners;
            return RedirectToAction("Index", "Home");

        }
        #endregion
        public ActionResult Pregled()
        {
            Korisnik korisnik = (Korisnik)HttpContext.Session["UlogovaniKorisnik"];
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<FitnessCentar> tempCentri = new List<FitnessCentar>();
            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.VlasnikCen == korisnik.KorisnickoIme && fc.Obrisan == false)
                {
                    tempCentri.Add(fc);
                }
            }
            HttpContext.Application["VlasnikCentri"] = tempCentri;
            return View();
        }
        public ActionResult Obrisi(string imeFC)
        {
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<GrupniTrening> grupniTrenings = (List<GrupniTrening>)HttpContext.Application["Treninzi"];
            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];
            FitnessCentar temp = new FitnessCentar();
            List<GrupniTrening> tempTrening = new List<GrupniTrening>();
            List < Vlasnik > vlasniks= (List<Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            Vlasnik vlasnik = new Vlasnik();
            foreach(Vlasnik v in vlasniks)
            {
                if(v.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    vlasnik = v;
                    break;
                }
            }

            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.Naziv == imeFC)
                {
                    temp = fc;
                    break;
                }
            }
            foreach (GrupniTrening gt in grupniTrenings)
            {
                if (gt.FitnesCentar == temp.Naziv)
                {
                    tempTrening.Add(gt);
                }
            }

            foreach (GrupniTrening gt in tempTrening)
            {
                if (DateTime.ParseExact(gt.VremeOdrzavanja, "dd/MM/yyyy HH:mm", null) > DateTime.Now && gt.Posetioci.Count > 0)
                {
                    ViewBag.Message = "Nije moguce izbrisati centar zbog zakazanog treninga!";
                    return View("Pregled");
                }
            }

            temp.Obrisan = true;
            foreach (string s in vlasnik.FitnessCentri.ToList())
            {
                if (s == temp.Naziv)
                {
                    vlasnik.FitnessCentri.Remove(s);
                }
            }
            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.Naziv == temp.Naziv)
                {
                    fitnessCentars.Remove(fc);
                    break;
                }
            }
            fitnessCentars.Add(temp);
            vlasniks.Remove(vlasnik);
            vlasniks.Add(vlasnik);
            IOXML.AzurirajVlasnike(vlasnik);
            IOXML.AzurirajFitnes(temp);
            List<Trener> tempTrener = new List<Trener>();
            foreach (Trener t in treners.ToList())
            {
                if (t.FitnessCentar == temp.Naziv)
                {

                    tempTrener.Add(t);
                    treners.Remove(t);
                }
            }
            foreach (Trener t in tempTrener)
            {
                t.Blokiran = true;
                IOXML.AzurirajTrenere(t);
                treners.Add(t);
            }
            HttpContext.Application["Treneri"] = treners;
            HttpContext.Application["FitnessCentri"] = fitnessCentars;
            return RedirectToAction("Pregled");
        }
        #region Modifikacija
        public ActionResult Modifikuj(string naziv)
        {
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["UlogovaniKorisnik"];
            FitnessCentar fitness = new FitnessCentar();
            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.VlasnikCen == korisnik.KorisnickoIme)
                {
                    fitness = fc;
                    break;
                }
            }
            ViewBag.Message = "";
            HttpContext.Application["Centar"] = fitness;
            return View();
        }
        public ActionResult ModifikujCentar(string adresa, string godina, string cenaMesec, string cenaGodina, string cenaJedan, string cenaGrupni, string cenaPersonalni)
        {


            if (adresa == "")
            {
                ViewBag.Message = "Morate popuniti adresu";
                return View("Modifikuj");

            }
            if ((!Int32.TryParse(godina, out int godinaC)) || godinaC < 1900 || godinaC > 2022)
            {
                ViewBag.Message = "Nepravilno uneta godina";
                return View("Modifikuj");
            }
            if (!(Double.TryParse(cenaMesec, out double cenaMes)) || !(Double.TryParse(cenaGodina, out double cenaGod)) || cenaMes < 0 || cenaGod < 0)
            {
                ViewBag.Message = "Nepravilno unete clanarine";
                return View("Modifikuj");
            }
            if (!(Double.TryParse(cenaJedan, out double cenaJed)) || !(Double.TryParse(cenaGrupni, out double cenaGrup)) || (!(Double.TryParse(cenaPersonalni, out double cenaPers))) || cenaJed < 0 || cenaGrup < 0 || cenaPers < 0)
            {
                ViewBag.Message = "Nepravilno unete cene treninga";
                return View("Modifikuj");
            }

            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            FitnessCentar centar = (FitnessCentar)HttpContext.Application["Centar"];

            centar.Adresa = adresa;
            centar.GodinaOtvaranja = godinaC;
            centar.CenaMesec = cenaMes;
            centar.CenaGodina = cenaGod;
            centar.CenaJedan = cenaJed;
            centar.CenaGrupni = cenaGrup;
            centar.CenaPersonalniTrener = cenaPers;

            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.Naziv == centar.Naziv)
                {
                    fitnessCentars.Remove(fc);
                    break;
                }
            }
            fitnessCentars.Add(centar);
            IOXML.AzurirajFitnes(centar);
            HttpContext.Application["FitnessCentri"] = fitnessCentars;

            return RedirectToAction("Pregled");
        }
        #endregion  
        public ActionResult Kreiraj()
        {
            return View();
        }
        public ActionResult KreirajCentar(string naziv, string adresa, string godina, string cenaMesec, string cenaGodina, string cenaJedan, string cenaGrupni, string cenaPersonalni)
        {
            if (naziv == "" || adresa == "")
            {
                ViewBag.Message = "Morate popuniti naziv i adresu centra!";
                return View("Kreiraj");
            }
            if ((!Int32.TryParse(godina, out int godinaC)) || godinaC < 1900 || godinaC > 2022)
            {
                ViewBag.Message = "Nepravilno uneta godina";
                return View("Modifikuj");
            }
            if (!(Double.TryParse(cenaMesec, out double cenaMes)) || !(Double.TryParse(cenaGodina, out double cenaGod)) || cenaMes < 0 || cenaGod < 0)
            {
                ViewBag.Message = "Nepravilno unete clanarine";
                return View("Modifikuj");
            }
            if (!(Double.TryParse(cenaJedan, out double cenaJed)) || !(Double.TryParse(cenaGrupni, out double cenaGrup)) || (!(Double.TryParse(cenaPersonalni, out double cenaPers))) || cenaJed < 0 || cenaGrup < 0 || cenaPers < 0)
            {
                ViewBag.Message = "Nepravilno unete cene treninga";
                return View("Modifikuj");
            }

            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];

            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            FitnessCentar centar = new FitnessCentar( naziv, adresa, godinaC, korisnik.KorisnickoIme, cenaMes, cenaGod, cenaJed, cenaGrup, cenaPers);
            Vlasnik vlasnik = new Vlasnik();
            foreach(Vlasnik v in vlasniks)
            {
                if(v.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    vlasnik = v;
                    vlasniks.Remove(v);
                    break;
                }
            }
            vlasnik.FitnessCentri.Add(naziv);
            vlasniks.Add(vlasnik);

            fitnessCentars.Add(centar);

            IOXML.AzurirajFitnes(centar);
            IOXML.AzurirajVlasnike(vlasnik);

            HttpContext.Application["Vlasnici"] = vlasniks;
            HttpContext.Application["FitnessCentri"] = fitnessCentars;
            return RedirectToAction("Index", "Home");

        }
    }
}