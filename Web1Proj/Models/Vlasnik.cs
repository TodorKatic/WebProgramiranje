using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class Vlasnik : Korisnik
    {

    
        public List<string> FitnessCentri { get; set; }

        public Vlasnik() : base()
        {
            FitnessCentri = new List<string>();
            this.UlogaKorisnika = Enums.Uloga.Vlasnik;
        }
        public Vlasnik(string korisnickoIme, string lozinka, string ime, string prezime, Enums.Pol pol, string email, string datumRodjenja) : base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            FitnessCentri = new List<string>();

            UlogaKorisnika = Enums.Uloga.Vlasnik;
        }
    }

}