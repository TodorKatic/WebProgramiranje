using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Web1Proj.Models
{
    public class Trener : Korisnik
    {
        
        public List<string> Treninzi { get; set; }
        public string FitnessCentar { get; set; }
        public bool Blokiran { get; set; }

        public Trener()
        {
            Treninzi = new List<string>();
            Blokiran = false;
            UlogaKorisnika = Enums.Uloga.Trener;
        }
        public Trener(string korisnickoIme, string lozinka, string ime, string prezime, Enums.Pol pol, string email, string datumRodjenja, string fitnessCentar) : base(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja)
        {
            Treninzi = new List<string>();
            FitnessCentar = fitnessCentar;
            Blokiran = false;
            UlogaKorisnika = Enums.Uloga.Trener;
        }
    }
}