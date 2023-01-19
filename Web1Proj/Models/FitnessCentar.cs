using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class FitnessCentar
    {
        private string naziv;
        private string adresa;
        private int godinaOtvaranja;
        private string vlasnikCen;
        private double cenaMesec;
        private double cenaGodina;
        private double cenaJedan;
        private double cenaGrupni;
        private double cenaPersonalniTrener;
        private bool obrisan;

        public string Naziv { get => naziv; set => naziv = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public int GodinaOtvaranja { get => godinaOtvaranja; set => godinaOtvaranja = value; }
        public string VlasnikCen { get => vlasnikCen; set => vlasnikCen = value; }
        public double CenaMesec { get => cenaMesec; set => cenaMesec = value; }
        public double CenaGodina { get => cenaGodina; set => cenaGodina = value; }
        public double CenaJedan { get => cenaJedan; set => cenaJedan = value; }
        public double CenaGrupni { get => cenaGrupni; set => cenaGrupni = value; }
        public double CenaPersonalniTrener { get => cenaPersonalniTrener; set => cenaPersonalniTrener = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }

        public FitnessCentar() { }
        public FitnessCentar(string naziv, string adresa, int godinaOtvaranja, string vlasnikCen, double cenaM, double cenaG, double cenaJedan, double cenaGr, double cenaPers)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            VlasnikCen = vlasnikCen;
            CenaMesec = cenaM;
            CenaGodina = cenaG;
            CenaJedan = cenaJedan;
            CenaGrupni = cenaGr;
            CenaPersonalniTrener = cenaPers;

        }
    }
}