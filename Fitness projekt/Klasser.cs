using System;
using System.Collections.Generic;
using System.Text;

namespace Fitness_projekt
{




    public class Medlem
    {
        public string fornavn;
        public string efternavn;
        public int alder;
        public string brugernavn;
        public string adgangskode;
        public Medlemskab medlemskab;

        public Medlem(string fornavn, string efternavn, int alder, string brugernavn, string adgangskode)
        {
            this.fornavn = fornavn;
            this.efternavn = efternavn;
            this.alder = alder;
            this.brugernavn = brugernavn;
            this.adgangskode = adgangskode;
            this.medlemskab = new Medlemskab();
        }
    }
    public class Administrator
    {
        public string brugerNavn;
        public string adgangsKode;
    }
    public class Aktivitet
    {
        public string titel;
        public string beskrivelse;
        public string dato;
        public Deltagerliste deltagere;
        public int maxDeltagere;

        public Aktivitet(string titel, string beskrivelse, string dato, int maxDeltagere)
        {
            deltagere = new Deltagerliste();
            this.titel = titel;
            this.beskrivelse = beskrivelse;
            this.dato = dato;
            this.maxDeltagere = maxDeltagere;
        }
    }
    public class Medlemskab
    {
        public bool erMedlem;
    }
    public class Aktivitetsliste
    {
        public List<Aktivitet> liste = new List<Aktivitet>();
    }
    public class Medlemsliste
    {
        public List<Medlem> liste = new List<Medlem>();
    }
    public class Deltagerliste
    {
        public List<Medlem> liste = new List<Medlem>();
    }

}
