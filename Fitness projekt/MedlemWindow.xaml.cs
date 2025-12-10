using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fitness_projekt
{
    /// <summary>
    /// Interaction logic for MedlemWindow.xaml
    /// </summary>
    public partial class MedlemWindow : Window
    {
        // lister til medlemmer og aktiviteter
        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();
        public Medlemsliste medlemsliste = new Medlemsliste();

        // gemmer input fra login -- jeg har fået hjælp fra github copilot til denne del (se bilag)
        private readonly string inputBrugernavn;
        private readonly string inputAdgangskode;

        //-------------------------------------------------------------------------------------------------------------------------------

        // metode til at læse medlemmer fra fil
        void LæsMedlemmerFil()
        {
            // læser alle linjer i filen og gemmer dem i et array af strings
            string[] filLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");

            // loop der går igennem hver linje i filen
            int i = 0;
            while (i < filLines.Length)
            {
                // splitter linjen med ";"
                string[] medlemVariabler = filLines[i].Split(";");
                // konverterer fra string til int
                int alder = Convert.ToInt32(medlemVariabler[2]);
                // opretter et nyt medlem objekt med variablerne fra filen
                Medlem medlem = new Medlem(medlemVariabler[0], medlemVariabler[1], alder, medlemVariabler[3], medlemVariabler[4], medlemVariabler[5]);
                // tilføjer medlemmet til medlemslisten
                medlemsliste.liste.Add(medlem);
                i++;
            }
        }

        // metode til at læse aktiviteter fra fil
        void LæsAktiviteterFil()
        {
            // læser alle linjer i filen og gemmer dem i et array af strings
            string[] FilLines = System.IO.File.ReadAllLines(@"AktiviteterFil.txt");

            // loop der går igennem hver linje i filen
            int i = 0;
            while (i < FilLines.Length)
            {
                // splitter linjen med ";"
                string[] AktivitetVariabler = FilLines[i].Split(";");
                // konverterer fra string til int
                int maxDeltagere = Convert.ToInt32(AktivitetVariabler[3]);
                // opretter et nyt aktivitet objekt med variablerne fra filen
                Aktivitet aktivitet = new Aktivitet(AktivitetVariabler[0], AktivitetVariabler[1], AktivitetVariabler[2], maxDeltagere);

                // kalder metode der tilføjer deltagere til aktiviteten
                TilføjDeltagere(aktivitet, AktivitetVariabler);

                // loop gennemgår aktivitetens deltagerliste og tjekker om brugeren er deltager i aktiviteten
                bool brugerErDeltager = false;
                int d = 0;
                while (d < aktivitet.deltagere.liste.Count)
                {
                    if (aktivitet.deltagere.liste[d].brugernavn == inputBrugernavn)
                    {
                        brugerErDeltager = true;
                        break;
                    }
                    d++;
                }
                // tilføjer aktiviteten til den rigtige listbox afhængig af om brugeren er deltager
                if (brugerErDeltager == true)
                {
                    MineAktiviteterListBox.Items.Add(aktivitet);
                }
                else
                {
                    AlleAktiviteterListBox.Items.Add(aktivitet);
                }

                // tilføjer aktiviteten til aktivitetslisten
                aktivitetsliste.liste.Add(aktivitet);
                i++;
            }
        }

        // metode til at gemme aktiviteter til fil
        void GemAktiviteterFil()
        {
            // laver en liste af strings til at gemme linjerne i filen på sammen måde som de blev læst
            List<string> filLines = new List<string>();

            // loop der går igennem hver aktivitet i aktivitetslisten
            int i = 0;
            while (i < aktivitetsliste.liste.Count)
            {
                Aktivitet aktivitet = aktivitetsliste.liste[i];

                // laver en liste af strings til at gemme variablerne for hver aktivitet
                List<string> dele = new List<string>();
                dele.Add(aktivitet.titel);
                dele.Add(aktivitet.beskrivelse);
                dele.Add(aktivitet.dato);
                dele.Add(aktivitet.maxDeltagere.ToString());

                // loop der går igennem hver deltager i aktivitetens deltagerliste
                int j = 0;
                while (j < aktivitet.deltagere.liste.Count)
                {
                    // tilføjer deltagerens brugernavn til dele listen
                    Medlem medlem = aktivitet.deltagere.liste[j];
                    dele.Add(medlem.brugernavn);
                    j++;
                }
                // tilføjer den samlede linje til filLines listen med string.Join
                filLines.Add(string.Join(";", dele));
                i++;
            }
            // skriver alle linjerne til filen
            File.WriteAllLines(@"AktiviteterFil.txt", filLines);
        }

        // metode til at tilføje deltagere til en aktivitet
        void TilføjDeltagere(Aktivitet aktivitet, string[] aktivitetVariabler)
        {
            // loop der går igennem alle brugernavne ved at starte fra index 4 i arrayet
            int j = 4;
            while (j < aktivitetVariabler.Length)
            {
                // finder medlemmet med det det samme brugernavn som i filen og tilføjer det til aktivitetens deltagerliste
                Medlem match = null;
                int k = 0;
                while (k < medlemsliste.liste.Count)
                {
                    Medlem medlem = medlemsliste.liste[k];
                    if (medlem.brugernavn == aktivitetVariabler[j])
                    {
                        match = medlem;
                        break;
                    }
                    k++;
                }
                aktivitet.deltagere.liste.Add(match);
                
                // skriver medlemmets navn i vinduet
                fuldeNavnTextBlock.Text = match.fornavn + " " + match.efternavn;
                j++;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------

        public MedlemWindow(string inputBrugernavn, string inputAdgangskode)
        {
            InitializeComponent();

            this.inputBrugernavn = inputBrugernavn;
            this.inputAdgangskode = inputAdgangskode;

            LæsMedlemmerFil();
            LæsAktiviteterFil();
        }

        //-------------------------------------------------------------------------------------------------------------------------------

        // selection changed event for listboxen med aktiviteter brugeren deltager i
        private void MineAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // bruger den valgte aktivitet som et aktivitet objekt
            Aktivitet valgt = MineAktiviteterListBox.SelectedItem as Aktivitet;
            
            // forhindrer fejl når der skiftes mellem de to listboxe
            if (valgt == null)
            {
                return;
            }
            // unselecter den anden listbox
            AlleAktiviteterListBox.UnselectAll();

            // opdaterer tekstboksene med den valgte aktivitets information
            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoTextBox.Text = valgt.dato;
            AktivitetDeltagereTextBlock.Text = valgt.deltagere.liste.Count + " / " + valgt.maxDeltagere.ToString();

            // opdaterer deltagere listboxen
            AktivitetDeltagereListBox.Items.Clear();
            if (valgt.deltagere.liste.Count > 0)
            {
                int i = 0;
                while (i < valgt.deltagere.liste.Count)
                {
                    Medlem medlem = valgt.deltagere.liste[i];
                    AktivitetDeltagereListBox.Items.Add(medlem.fornavn + " " + medlem.efternavn);
                    i++;
                }
            }
            // aktiverer den rigtige knap
            ForladButton.IsHitTestVisible = true;
            DeltagButton.IsHitTestVisible = false;
        }

        // selection changed event for listboxen med tilgængelige aktiviteter
        private void AlleAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // bruger den valgte aktivitet som et aktivitet objekt
            Aktivitet valgt = AlleAktiviteterListBox.SelectedItem as Aktivitet;

            // forhindrer fejl når der skiftes mellem de to listboxe
            if (valgt == null)
            {
                return;
            }
            // unselecter den anden listbox
            MineAktiviteterListBox.UnselectAll();

            // opdaterer tekstboksene med den valgte aktivitets information
            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoTextBox.Text = valgt.dato;
            AktivitetDeltagereTextBlock.Text = valgt.deltagere.liste.Count + " / " + valgt.maxDeltagere.ToString();

            // opdaterer deltagere listboxen
            AktivitetDeltagereListBox.Items.Clear();
            if (valgt.deltagere.liste.Count > 0)
            {
                int i = 0;
                while (i < valgt.deltagere.liste.Count)
                {
                    Medlem medlem = valgt.deltagere.liste[i];
                    AktivitetDeltagereListBox.Items.Add(medlem.fornavn + " " + medlem.efternavn);
                    i++;
                }
            }
            // aktiverer den rigtige knap
            ForladButton.IsHitTestVisible = false;
            DeltagButton.IsHitTestVisible = true;
        }

        // deltag knap click event
        private void DeltagButton_Click(object sender, RoutedEventArgs e)
        {
            // spørger brugeren om de er sikre på at de vil deltage i aktiviteten
            if (MessageBox.Show("Er du sikker på at du vil deltage i aktiviteten?", "Deltag i aktivitet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // bruger den valgte aktivitet som et aktivitet objekt
                Aktivitet valgt = AlleAktiviteterListBox.SelectedItem as Aktivitet;

                // finder brugeren, som er logget ind, i medlemsliste
                Medlem bruger = null;
                int i = 0;
                while (i < medlemsliste.liste.Count)
                {
                    if (medlemsliste.liste[i].brugernavn == inputBrugernavn)
                    {
                        bruger = medlemsliste.liste[i];
                        break;
                    }
                    i++;
                }
                // tjekker om aktiviteten er fuld
                if (valgt.deltagere.liste.Count >= valgt.maxDeltagere)
                {
                    // viser en fejlmeddelelse hvis aktiviteten er fuld
                    MessageBox.Show("Aktiviteten er fuld", "Fejl", MessageBoxButton.OK);
                    return;
                }
                // tilføjer bruger til aktivitetens deltagerliste
                valgt.deltagere.liste.Add(bruger);

                // flytter aktivitet til den anden listbox
                AlleAktiviteterListBox.Items.Remove(valgt);
                MineAktiviteterListBox.Items.Add(valgt);

                // tilføjer bruger til deltagere listbox
                AktivitetDeltagereListBox.Items.Add(bruger.fornavn + " " + bruger.efternavn);

                ForladButton.IsHitTestVisible = false;
                DeltagButton.IsHitTestVisible = false;

                // gemmer ændringerne til fil
                GemAktiviteterFil();
            }
        }

        // forlad knap click event
        private void ForladButton_Click(object sender, RoutedEventArgs e)
        {
            // spørger brugeren om de er sikre på at de vil forlade aktiviteten
            if (MessageBox.Show("Er du sikker på at du vil forlade aktiviteten?", "Forlad aktivitet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // bruger den valgte aktivitet som et aktivitet objekt
                Aktivitet valgt = MineAktiviteterListBox.SelectedItem as Aktivitet;

                // finder brugeren, som er logget ind, i medlemsliste
                Medlem bruger = null;
                int i = 0;
                while (i < medlemsliste.liste.Count)
                {
                    if (medlemsliste.liste[i].brugernavn == inputBrugernavn)
                    {
                        bruger = medlemsliste.liste[i];
                        break;
                    }
                    i++;
                }
                // fjerner bruger fra aktivitetens deltagerliste
                valgt.deltagere.liste.Remove(bruger);

                // flyt aktivitet til den anden listbox
                MineAktiviteterListBox.Items.Remove(valgt);
                AlleAktiviteterListBox.Items.Add(valgt);


                ForladButton.IsHitTestVisible = false;
                DeltagButton.IsHitTestVisible = false;

                // gemmer ændringerne til fil
                GemAktiviteterFil();
            }
        }

        private void DeltagButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // gør det tydeligt om knappen kan bruges
            if (DeltagButton.IsHitTestVisible == false)
            {
                DeltagButton.Opacity = 0.5;
            }
            else
            {
                DeltagButton.Opacity = 1;
            }
        }

        private void ForladButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // gør det tydeligt om knappen kan bruges
            if (ForladButton.IsHitTestVisible == false)
            {
                ForladButton.Opacity = 0.5;
            }
            else
            {
                ForladButton.Opacity = 1;
            }
        }
    }
}
