using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web1Proj.Models
{
    public class Adresa
    {
        public string Ulica { get; set; }
        public int Broj { get; set; }
        public string Mesto { get; set; }
        public int PostanskiBroj { get; set; }

        public Adresa(string ulica, int broj, string mesto, int postanskiBroj)
        {
            Ulica = ulica;
            Broj = broj;
            Mesto = mesto;
            PostanskiBroj = postanskiBroj;
        }
        public Adresa() { }
    }
}