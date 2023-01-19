using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class Posetilac : Korisnik
    {
        public List<string> GrupniTreninzi { get; set; }

        public Posetilac() : base()
        {
           
            UlogaKorisnika = Enums.Uloga.Posetilac;
            GrupniTreninzi = new List<string>();
        }

        public Posetilac(string korisnickoIme, string lozinka, string ime, string prezime, Enums.Pol pol, string email, string datumRodjenja): base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            UlogaKorisnika = Enums.Uloga.Posetilac;
            GrupniTreninzi = new List<string>();
        }
    }
}