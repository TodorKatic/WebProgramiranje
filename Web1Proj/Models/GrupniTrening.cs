using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class GrupniTrening
    {
        private string naziv;
        private Enums.TipTreninga tipTreninga;
        private string fitnesCentar;
        private int trajanje;
        private string vremeOdrzavanja;
        private int maxBroj;
        private List<string> posetioci = new List<string>();
        private string trener;
        private bool obrisan;

        public string Naziv { get => naziv; set => naziv = value; }
        public Enums.TipTreninga TipTreninga { get => tipTreninga; set => tipTreninga = value; }
        public string FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public int Trajanje { get => trajanje; set => trajanje = value; }
        public string VremeOdrzavanja { get => vremeOdrzavanja; set => vremeOdrzavanja = value; }
        public int MaxBroj { get => maxBroj; set => maxBroj = value; }
        public List<string> Posetioci { get => posetioci; set => posetioci = value; }
        public string Trener { get => trener; set => trener = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }

        public GrupniTrening()
        {
            Posetioci = new List<string>();
            Obrisan = false;
        }
        
        public GrupniTrening(string naziv, Enums.TipTreninga tipTreninga, string fitnesCentar, int trajanjeTreninga, string vremeOdrzavanja, int maxBroj, string trener)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            FitnesCentar = fitnesCentar;
            Trajanje = trajanjeTreninga;
            VremeOdrzavanja = vremeOdrzavanja;
            MaxBroj = maxBroj;
            Posetioci = new List<string>();
            Trener = trener;
            Obrisan = false;
        }
    }
}