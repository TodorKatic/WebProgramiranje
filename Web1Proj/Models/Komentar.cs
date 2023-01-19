using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class Komentar
    {
        private string posetilac;
        private string fitnessCentar;
        private string tekstKomentara;
        private int ocena;
        private bool odobren;

        public string Posetilac { get => posetilac; set => posetilac = value; }
        public string FitnessCentar { get => fitnessCentar; set => fitnessCentar = value; }
        public string TekstKomentara { get => tekstKomentara; set => tekstKomentara = value; }
        public int Ocena { get => ocena; set => ocena = value; }
       
        public bool Odobren { get => odobren; set => odobren = value; }

        public Komentar() { }

        public Komentar(string posetilac, string fitnessCentar, string tekstKomentara, int ocena)
        {
            Posetilac = posetilac;
            FitnessCentar = fitnessCentar;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Odobren = false;
        }
    }
}