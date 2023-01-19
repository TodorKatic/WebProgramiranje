using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1Proj.Models;
using Web1Proj.Models.XMLDATA;
using static Web1Proj.Models.Enums;

namespace Web1Proj.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Korisnik korisnik = null;
            //HttpContext.Session["UlogovaniKorisnik"] = korisnik;
            List<FitnessCentar> fitnessCentrovi = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<FitnessCentar> tempFit = new List<FitnessCentar>();
            foreach(FitnessCentar f in fitnessCentrovi)
            {
                if (f.Obrisan == false)
                    tempFit.Add(f);
            }
            HttpContext.Application["FC"] = tempFit;
            
            return View();
        }
        #region Reg
        public ActionResult Registration()
        {
            ViewBag.Message = "";
            return View("Registracija");
        }
        public ActionResult Registracija(string username, string password, string firstname, string lastname, string email, string gender,  DateTime dateofbirth)
        {
            List<Posetilac> posetioci = (List<Posetilac>)HttpContext.Application["Posetioci"];
            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];

            if(username=="" || firstname=="" || lastname=="")
            {
                ViewBag.Message = "Popunite korisnicko ime, ime i prezime!";
                return View("Registracija");
            }
            if (password == "" || email == "")
            {
                ViewBag.Message = "Email  i lozinka moraju biti popunjeni";
                return View("Registracija");
            }
            foreach (Posetilac po in posetioci)
            {
                if (po.KorisnickoIme == username)
                {
                    ViewBag.Message = "Korisnik vec postoji!";
                    return View();
                }
            }
            foreach (Trener t in treners)
            {
                if (t.KorisnickoIme == username)
                {
                    ViewBag.Message = "Korisnik vec postoji!";
                    return View();
                }
            }
            foreach (Vlasnik v in vlasniks)
            {
                if (v.KorisnickoIme == username)
                {
                    ViewBag.Message = "Korisnik vec postoji!";
                    return View();
                }
            }
            if(dateofbirth.AddYears(14) > DateTime.Now)
            {
                ViewBag.Message="Korisnik nema dovoljno godina!";
                return View();
            }
            Pol pol = (Pol)Enum.Parse(typeof(Pol), gender);
            Posetilac p = new Posetilac(username, password, firstname, lastname, pol, email, dateofbirth.ToString("dd/MM/yyyy"));
            posetioci.Add(p);
            Korisnik k = p;
            
            IOXML.AzurirajPosetioce(p);
            HttpContext.Application["Posetioci"] = posetioci;
            Session["UlogovaniKorisnik"] = (Korisnik)p;
            return View("Index");
        }
        #endregion
        #region Login
        public ActionResult LoginRedirect()
        {
            ViewBag.Message = "";
            return View("Login");
        }

        public ActionResult Login(string username, string password)
        {
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];
            Korisnik korisnik = null;
            if (username == "" || password == "")
            {
                ViewBag.Message = "Popunite podatke!";
                return View();
            }
            foreach(Posetilac p in posetilacs)
            {
                if(p.KorisnickoIme == username && p.Lozinka == password)
                {
                    korisnik = (Korisnik)p;
                    korisnik.UlogaKorisnika = Uloga.Posetilac;
                    Session["UlogovaniKorisnik"] = korisnik;
                    return RedirectToAction("Index");
                }
            }
            foreach (Trener t in treners)
            {
                if (t.KorisnickoIme == username && t.Lozinka == password)
                {
                    if(t.Blokiran == true)
                    {
                        ViewBag.Message = "Trener je blokiran";
                        return View();
                    }
                    korisnik = (Korisnik)t;
                    korisnik.UlogaKorisnika = Uloga.Trener;
                    Session["UlogovaniKorisnik"] = korisnik;
                    return RedirectToAction("Index");
                }
            }
            foreach (Vlasnik v in vlasniks)
            {
                if (v.KorisnickoIme == username && v.Lozinka == password)
                {
                    korisnik = (Korisnik)v;
                    korisnik.UlogaKorisnika = Uloga.Vlasnik;
                    Session["UlogovaniKorisnik"] = korisnik;
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Message = "Korisnicko ime ili lozinka neispravni!";
            return View();

        }
        #endregion
        #region Sort/Search
        public ActionResult Search(string naziv, string adresa, string godinaDonja, string godinaGornja)
        {
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<FitnessCentar> temp = new List<FitnessCentar>();
            List<FitnessCentar> tempic = new List<FitnessCentar>();
            foreach (FitnessCentar fc in fitnessCentars)
            {
                if (fc.Obrisan == false)
                {
                    tempic.Add(fc);
                }
            }
            bool imalNesto = false;
            if(naziv != "")
            {
                imalNesto = true;
               foreach(FitnessCentar fc in tempic.ToList())
                {
                    if (fc.Naziv.Contains(naziv))
                    {
                        temp.Add(fc);

                    }
                }
            }
            if (adresa != "")
            {

                if (imalNesto)
                {
                    foreach (FitnessCentar fc in temp.ToList())
                    {
                        if (!fc.Adresa.Contains(adresa))
                        {
                            temp.Remove(fc);
                        }
                    }
                }
                else
                {
                    foreach (FitnessCentar fc in tempic.ToList())
                    {
                        if (fc.Adresa.Contains(adresa))
                        {
                            temp.Add(fc);
                        }
                    }
                }
                imalNesto = true;
            }
            if (godinaDonja != "")
            {
                int donja = Int32.Parse(godinaDonja);
                if (imalNesto)
                {
                    foreach (FitnessCentar fc in temp.ToList())
                    {
                        if (fc.GodinaOtvaranja < donja)
                        {
                            temp.Remove(fc);
                        }
                    }
                }
                else
                {
                    foreach (FitnessCentar fc in tempic.ToList())
                    {
                        if (fc.GodinaOtvaranja > donja)
                        {
                            temp.Add(fc);
                        }
                    }

                }
                imalNesto = true;
            }
            if (godinaGornja != "")
            {
                int gornja = Int32.Parse(godinaGornja);
                if (imalNesto)
                {
                    foreach (FitnessCentar fc in temp.ToList())
                    {
                        if (fc.GodinaOtvaranja > gornja)
                        {
                            temp.Remove(fc);
                        }
                    }
                }
                else
                {
                    foreach (FitnessCentar fc in tempic.ToList())
                    {
                        if (fc.GodinaOtvaranja < gornja)
                        {
                            temp.Add(fc);
                        }
                    }

                }
                imalNesto = true;
            }
            if (imalNesto) 
                HttpContext.Application["FC"] = temp;
            else
                HttpContext.Application["FC"] = tempic;


            return View("Index");
        }
        
        public ActionResult Sort(string vrstaFiltera, string rasOp)
        {
            List<FitnessCentar> fitnessCentars = (List<FitnessCentar>)HttpContext.Application["FitnessCentri"];
            List<FitnessCentar> tempic = new List<FitnessCentar>();
            foreach(FitnessCentar fc in fitnessCentars) 
            {
                if (fc.Obrisan == false)
                {
                    tempic.Add(fc);
                }
            }
            if(rasOp == "rastuce")
            {
                if (vrstaFiltera == "naziv")
                {
                    HttpContext.Application["FC"]=tempic.OrderBy(f => f.Naziv).ToList();
                }
                if (vrstaFiltera == "adresa")
                {
                    HttpContext.Application["FC"] = tempic.OrderBy(f => f.Adresa).ToList();
                }
                if (vrstaFiltera == "godina")
                {
                    HttpContext.Application["FC"] = tempic.OrderBy(f => f.GodinaOtvaranja).ToList();
                }
            }
            if (rasOp == "opadajuce")
            {
                if (vrstaFiltera == "naziv")
                {
                    HttpContext.Application["FC"] = tempic.OrderByDescending(f => f.Naziv).ToList();
                }
                if (vrstaFiltera == "adresa")
                {
                    HttpContext.Application["FC"] = tempic.OrderByDescending(f => f.Adresa).ToList();
                }
                if (vrstaFiltera == "godina")
                {
                    HttpContext.Application["FC"] = tempic.OrderByDescending(f => f.GodinaOtvaranja).ToList();
                }
            }
            return View("Index");
        }
        #endregion
        public ActionResult LogOut()
        {
            Session["UlogovaniKorisnik"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult UrediProfil()
        {
            return View();
        }
        public ActionResult Uredjivanje(string password, string firstName, string lastName,string email, string pol)
        {
            Korisnik korisnik = (Korisnik)Session["UlogovaniKorisnik"];
            List<Posetilac> posetilacs = (List<Posetilac>)HttpContext.Application["Posetioci"];
            Posetilac posetilac;
            if (korisnik.UlogaKorisnika == Uloga.Posetilac)
            {
                foreach (Posetilac p in posetilacs)
                {
                    if (p.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        posetilac = p;
                        if (password != "")
                            posetilac.Lozinka = password;
                        if (firstName != "")
                            posetilac.Ime = firstName;
                        if (lastName != "")
                            posetilac.Prezime = lastName;
                        if (email != "")
                            posetilac.Email = email;
                        posetilac.PolKorisnika = (Pol)Enum.Parse(typeof(Enums.Pol), pol);
                        posetilacs.Remove(p);
                        posetilacs.Add(posetilac);
                        HttpContext.Application["Posetioci"] = posetilacs;
                        IOXML.AzurirajPosetioce(posetilac);

                        return RedirectToAction("Index");
                    }
                }
            }
            List<Trener> treners = (List<Trener>)HttpContext.Application["Treneri"];
            Trener trener;
            if (korisnik.UlogaKorisnika == Uloga.Trener)
            {
                foreach (Trener t in treners)
                {
                    if (t.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        trener = t;
                        if (password != "")
                            trener.Lozinka = password;
                        if (firstName != "")
                            trener.Ime = firstName;
                        if (lastName != "")
                            trener.Prezime = lastName;
                        if (email != "")
                            trener.Email = email;
                        trener.PolKorisnika = (Pol)Enum.Parse(typeof(Enums.Pol), pol);
                        treners.Remove(t);
                        treners.Add(trener);
                        HttpContext.Application["Treneri"] = treners;
                        IOXML.AzurirajTrenere(trener);

                        return RedirectToAction("Index");
                    }
                }
            }
            List<Vlasnik> vlasniks = (List<Vlasnik>)HttpContext.Application["Vlasnici"];
            Vlasnik vlasnik;
            if (korisnik.UlogaKorisnika == Uloga.Vlasnik)
            {
                foreach (Vlasnik v in vlasniks)
                {
                    if (v.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        vlasnik = v;
                        if (password != "")
                            vlasnik.Lozinka = password;
                        if (firstName != "")
                            vlasnik.Ime = firstName;
                        if (lastName != "")
                            vlasnik.Prezime = lastName;
                        if (email != "")
                            vlasnik.Email = email;
                        vlasnik.PolKorisnika = (Pol)Enum.Parse(typeof(Enums.Pol), pol);
                        vlasniks.Remove(v);
                        vlasniks.Add(vlasnik);
                        HttpContext.Application["Vlasnici"] = vlasniks;
                        IOXML.AzurirajVlasnike(vlasnik);

                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");

        }
    }
}