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
        // --------- Validering ---------- maks deltagere
        
        
        
        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();
        public Medlemsliste medlemsliste = new Medlemsliste();

        private readonly string inputBrugernavn;
        private readonly string inputAdgangskode;


        void LæsMedlemmerFil()
        {
            string[] filLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");

            int i = 0;
            while (i < filLines.Length)
            {
                string[] medlemVariabler = filLines[i].Split(";");
                int alder = Convert.ToInt32(medlemVariabler[2]);
                Medlem medlem = new Medlem(medlemVariabler[0], medlemVariabler[1], alder, medlemVariabler[3], medlemVariabler[4], medlemVariabler[5]);
                medlemsliste.liste.Add(medlem);
                i++;
            }
        }


        void LæsAktiviteterFil()
        {
            string[] FilLines = System.IO.File.ReadAllLines(@"AktiviteterFil.txt");

            int i = 0;
            while (i < FilLines.Length)
            {
                string[] AktivitetVariabler = FilLines[i].Split(";");
                int maxDeltagere = Convert.ToInt32(AktivitetVariabler[3]);
                Aktivitet aktivitet = new Aktivitet(AktivitetVariabler[0], AktivitetVariabler[1], AktivitetVariabler[2], maxDeltagere);

                TilføjDeltagere(aktivitet, AktivitetVariabler);

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
                if (brugerErDeltager == true)
                {
                    MineAktiviteterListBox.Items.Add(aktivitet);
                }
                else
                {
                    AlleAktiviteterListBox.Items.Add(aktivitet);
                }

                aktivitetsliste.liste.Add(aktivitet);
                i++;
            }
        }


        void GemAktiviteterFil()
        {
            List<string> filLines = new List<string>();
            int i = 0;
            while (i < aktivitetsliste.liste.Count)
            {
                Aktivitet aktivitet = aktivitetsliste.liste[i];

                List<string> dele = new List<string>();
                dele.Add(aktivitet.titel);
                dele.Add(aktivitet.beskrivelse);
                dele.Add(aktivitet.dato);
                dele.Add(aktivitet.maxDeltagere.ToString());

                int j = 0;
                while (j < aktivitet.deltagere.liste.Count)
                {
                    Medlem medlem = aktivitet.deltagere.liste[j];
                    dele.Add(medlem.brugernavn);
                    j++;
                }

                filLines.Add(string.Join(";", dele));
                i++;
            }

            File.WriteAllLines(@"AktiviteterFil.txt", filLines);
        }


        void TilføjDeltagere(Aktivitet aktivitet, string[] aktivitetVariabler)
        {
            int j = 4;
            while (j < aktivitetVariabler.Length)
            {
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
                j++;
            }
        }





        public MedlemWindow(string inputBrugernavn, string inputAdgangskode)
        {
            InitializeComponent();

            this.inputBrugernavn = inputBrugernavn;
            this.inputAdgangskode = inputAdgangskode;

            LæsMedlemmerFil();
            LæsAktiviteterFil();
        }









        private void MineAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // github copilot
            Aktivitet valgt = MineAktiviteterListBox.SelectedItem as Aktivitet;

            if (valgt == null)
            {
                return;
            }
            AlleAktiviteterListBox.UnselectAll();

            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoTextBox.Text = valgt.dato;
            AktivitetDeltagereTextBlock.Text = valgt.maxDeltagere.ToString();

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

            ForladButton.IsHitTestVisible = true;
            DeltagButton.IsHitTestVisible = false;
        }

        private void AlleAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // github copilot
            Aktivitet valgt = AlleAktiviteterListBox.SelectedItem as Aktivitet;

            if (valgt == null)
            {
                return;
            }
            MineAktiviteterListBox.UnselectAll();


            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoTextBox.Text = valgt.dato;
            AktivitetDeltagereTextBlock.Text = valgt.maxDeltagere.ToString();

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

            ForladButton.IsHitTestVisible = false;
            DeltagButton.IsHitTestVisible = true;
        }

        private void DeltagButton_Click(object sender, RoutedEventArgs e)
        {
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
                //viser en fejlmeddelelse
                MessageBox.Show("Aktiviteten er fuld", "Fejl", MessageBoxButton.OK);
                return;
            }

            // tilføj bruger til aktivitetens deltagerliste
            valgt.deltagere.liste.Add(bruger);

            // flyt aktivitet til den anden listbox
            AlleAktiviteterListBox.Items.Remove(valgt);
            MineAktiviteterListBox.Items.Add(valgt);

            // tilføj bruger til deltagere listbox
            AktivitetDeltagereListBox.Items.Add(bruger.fornavn + " " + bruger.efternavn);

            ForladButton.IsHitTestVisible = false;
            DeltagButton.IsHitTestVisible = false;

            GemAktiviteterFil();
        }

        private void ForladButton_Click(object sender, RoutedEventArgs e)
        {
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

            valgt.deltagere.liste.Remove(bruger);

            // flyt aktivitet til den anden listbox
            MineAktiviteterListBox.Items.Remove(valgt);
            AlleAktiviteterListBox.Items.Add(valgt);


            ForladButton.IsHitTestVisible = false;
            DeltagButton.IsHitTestVisible = false;

            GemAktiviteterFil();
        }

        private void DeltagButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
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
