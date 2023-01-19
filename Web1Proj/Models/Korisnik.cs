using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class Korisnik
    {
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private Enums.Pol polKorisnika;
        private string email;
        private string datumRodjenja;
        private Enums.Uloga ulogaKorisnika;

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public Enums.Pol PolKorisnika { get => polKorisnika; set => polKorisnika = value; }
        public string Email { get => email; set => email = value; }
        public string DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public Enums.Uloga UlogaKorisnika { get => ulogaKorisnika; set => ulogaKorisnika = value; }

        public Korisnik() { }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, Enums.Pol pol, string email, string datumRodjenja)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Prezime = prezime;
            Ime = ime;
            PolKorisnika = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;

        }
    }
}