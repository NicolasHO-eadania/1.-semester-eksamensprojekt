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
        public string medlemskab;

        public Medlem(string fornavn, string efternavn, int alder, string brugernavn, string adgangskode, string medlemskab)
        {
            this.fornavn = fornavn;
            this.efternavn = efternavn;
            this.alder = alder;
            this.brugernavn = brugernavn;
            this.adgangskode = adgangskode;
            this.medlemskab = medlemskab;
        }
    }
    public class Administrator
    {
        public string brugernavn;
        public string adgangskode;

        public Administrator(string brugernavn, string adgangskode)
        {
            this.brugernavn = brugernavn;
            this.adgangskode = adgangskode;
        }
    }
    public class Aktivitet
    {
        public string titel;
        public string beskrivelse;
        public string dato;
        public int maxDeltagere;
        public Deltagerliste deltagere;

        public Aktivitet(string titel, string beskrivelse, string dato, int maxDeltagere)
        {
            deltagere = new Deltagerliste();
            this.titel = titel;
            this.beskrivelse = beskrivelse;
            this.dato = dato;
            this.maxDeltagere = maxDeltagere;
        }
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
