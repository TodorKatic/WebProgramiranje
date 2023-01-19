using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Web1Proj.Models.XMLDATA
{
    public class IOXML
    {
        private static readonly string posetiociPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\posetioci.txt";
        private static string treneriPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\treneri.txt";
        private static string vlasniciPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\vlasnici.txt";
        private static string treninziPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\grupniTreninzi.txt";
        private static string fitnesCentriPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\fitnesCentri.txt";
        private static string komentariPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\komentari.txt";

        public static PosetiociXML IzvlaciPosetioce()
        {
            PosetiociXML posetioci = new PosetiociXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PosetiociXML));
            using(StreamReader sr = new StreamReader(posetiociPath))
            {
                posetioci = (PosetiociXML)xmlSerializer.Deserialize(sr);
            }
            return posetioci;
        }
        public static void AzurirajPosetioce(Posetilac posetilac)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PosetiociXML));
            PosetiociXML posetioci = IzvlaciPosetioce();
            foreach(Posetilac p in posetioci.Posetilacs.ToList())
            {
                if (p.KorisnickoIme == posetilac.KorisnickoIme)
                    posetioci.Posetilacs.Remove(p);
            }
            posetioci.Posetilacs.Add(posetilac);
            using(StreamWriter sw = new StreamWriter(posetiociPath))
            {
                xmlSerializer.Serialize(sw, posetioci);
            }
        }


        public static TreneriXML IzvlaciTrenere()
        {
            TreneriXML treneri = new TreneriXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TreneriXML));
            using (StreamReader sr = new StreamReader(treneriPath))
            {
                treneri = (TreneriXML)xmlSerializer.Deserialize(sr);
            }
            return treneri;
        }
        public static void AzurirajTrenere(Trener trener)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TreneriXML));
            TreneriXML treneri = IzvlaciTrenere();
            foreach (Trener t in treneri.Treners.ToList())
            {
                if (t.KorisnickoIme == trener.KorisnickoIme)
                    treneri.Treners.Remove(t);
            }
            treneri.Treners.Add(trener);
            using (StreamWriter sw = new StreamWriter(treneriPath))
            {
                xmlSerializer.Serialize(sw, treneri);
            }
        }


        public static VlasniciXML IzvlaciVlasnike()
        {
            VlasniciXML posetioci = new VlasniciXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VlasniciXML));
            using (StreamReader sr = new StreamReader(vlasniciPath))
            {
                posetioci = (VlasniciXML)xmlSerializer.Deserialize(sr);
            }
            return posetioci;
        }
        public static void AzurirajVlasnike(Vlasnik posetilac)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VlasniciXML));
            VlasniciXML posetioci = IzvlaciVlasnike();
            foreach (Vlasnik p in posetioci.Vlasniks.ToList())
            {
                if (p.KorisnickoIme == posetilac.KorisnickoIme)
                    posetioci.Vlasniks.Remove(p);
            }
            posetioci.Vlasniks.Add(posetilac);
            using (StreamWriter sw = new StreamWriter(vlasniciPath))
            {
                xmlSerializer.Serialize(sw, posetioci);
            }
        }


        public static GrupniXML IzvlaciTreninge()
        {
            GrupniXML posetioci = new GrupniXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GrupniXML));
            using (StreamReader sr = new StreamReader(treninziPath))
            {
                posetioci = (GrupniXML)xmlSerializer.Deserialize(sr);
            }
            return posetioci;
        }
        public static void AzurirajTreninge(GrupniTrening posetilac)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GrupniXML));
            GrupniXML posetioci = IzvlaciTreninge();
            foreach (GrupniTrening p in posetioci.GrupniTrenings.ToList())
            {
                if (p.Naziv == posetilac.Naziv)
                    posetioci.GrupniTrenings.Remove(p);
            }
            posetioci.GrupniTrenings.Add(posetilac);
            using (StreamWriter sw = new StreamWriter(treninziPath))
            {
                xmlSerializer.Serialize(sw, posetioci);
            }
        }


        public static FitnessXML IzvlaciFitnese()
        {
            FitnessXML posetioci = new FitnessXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FitnessXML));
            using (StreamReader sr = new StreamReader(fitnesCentriPath))
            {
                posetioci = (FitnessXML)xmlSerializer.Deserialize(sr);
            }
            return posetioci;
        }
        public static void AzurirajFitnes(FitnessCentar posetilac)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FitnessXML));
            FitnessXML posetioci = IzvlaciFitnese();
            foreach (FitnessCentar p in posetioci.FitnessCentars.ToList())
            {
                if (p.Naziv == posetilac.Naziv)
                    posetioci.FitnessCentars.Remove(p);
            }
            posetioci.FitnessCentars.Add(posetilac);
            using (StreamWriter sw = new StreamWriter(fitnesCentriPath))
            {
                xmlSerializer.Serialize(sw, posetioci);
            }
        }


        public static KomentariXML IzvlaciKomentare()
        {
            KomentariXML komentari = new KomentariXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(KomentariXML));
            using (StreamReader sr = new StreamReader(komentariPath))
            {
                komentari = (KomentariXML)xmlSerializer.Deserialize(sr);
            }
            return komentari;
        }
        public static void AzurirajKomentare(Komentar komentar)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(KomentariXML));
            KomentariXML komentari = IzvlaciKomentare();
            foreach (Komentar k in komentari.Komentars.ToList())
            {
                if (k.Posetilac == komentar.Posetilac && k.FitnessCentar == komentar.FitnessCentar)
                    komentari.Komentars.Remove(k);
            }
            komentari.Komentars.Add(komentar);
            using (StreamWriter sw = new StreamWriter(komentariPath))
            {
                xmlSerializer.Serialize(sw, komentari);
            }
        }


    }
}