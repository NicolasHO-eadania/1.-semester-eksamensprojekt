using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fitness_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();
        public Medlemsliste medlemsliste = new Medlemsliste();

        public bool opretterAktivitet = false;


        //-----------Startmedlemmer og aktiviteter----------//

        void LæsMedlemmerFil()
        {
            string[] FilLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");

            int i = 0;
            while(i < FilLines.Length)
            {
                string[] MedlemVariabler = FilLines[i].Split(";");
                int alder = Convert.ToInt32(MedlemVariabler[2]);
                Medlem medlem = new Medlem(MedlemVariabler[0], MedlemVariabler[1], alder, MedlemVariabler[3], MedlemVariabler[4]);
                medlemsliste.liste.Add(medlem);
                MedlemmerListBox.Items.Add(MedlemVariabler[0] + " " + MedlemVariabler[1]);
                i++;
            }
        }

        //--------------------------------------------------//


        public MainWindow()
        {
            InitializeComponent();
            LæsMedlemmerFil();
        }
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





        //--------------------------Administrator Aktivitetsstyring--------------------------//
        // MANGLER: hint tekst i tekstbokse + validering af input + (gemme data ved lukning + indlæse data ved opstart)
        private void NyAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            opretterAktivitet = true;

            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;
            AktivitetMaxDeltagereTextBox.Clear();

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;
            GemAktivitetButton.IsHitTestVisible = true;
            AktivitetMaxDeltagereTextBox.IsHitTestVisible = true;

            RedigerAktivitetButton.IsHitTestVisible = false;
            SletAktivitetButton.IsHitTestVisible = false;
        }

        private void AktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AktiviteterListBox.SelectedIndex < 0 || AktiviteterListBox.SelectedIndex >= aktivitetsliste.liste.Count)
            {
                RedigerAktivitetButton.IsHitTestVisible = false;
                SletAktivitetButton.IsHitTestVisible = false;
                return;
            }
            AktivitetTitelTextBox.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel;
            AktivitetBeskrivelseTextBox.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse;
            AktivitetDatoDatePicker.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato;
            AktivitetMaxDeltagereTextBox.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].maxDeltagere.ToString();

            RedigerAktivitetButton.IsHitTestVisible = true;
            SletAktivitetButton.IsHitTestVisible = true;
        }

        private void RedigerAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            opretterAktivitet = false;

            GemAktivitetButton.IsHitTestVisible = true;

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;

            RedigerAktivitetButton.IsHitTestVisible = false;
            SletAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = false;
            NyAktivitetButton.IsHitTestVisible = false;
        }

        private void GemAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            int maxDeltagere = 999;
            if (!string.IsNullOrEmpty(AktivitetMaxDeltagereTextBox.Text))
            {
                if (!int.TryParse(AktivitetMaxDeltagereTextBox.Text, out int maxDeltagereAendret))
                {
                    return;
                }
                maxDeltagere = maxDeltagereAendret;
            }
            if (opretterAktivitet == true)
            {
                Aktivitet nyAktivitet = new Aktivitet(AktivitetTitelTextBox.Text, AktivitetBeskrivelseTextBox.Text, AktivitetDatoDatePicker.Text, maxDeltagere);
                aktivitetsliste.liste.Add(nyAktivitet);

                AktiviteterListBox.Items.Add(nyAktivitet.titel + "     -     " + nyAktivitet.dato);

                //retter valget i listen til den nye aktivitet der blev oprettet
                AktiviteterListBox.SelectedIndex = AktiviteterListBox.Items.Count - 1;
            }
            else if (AktiviteterListBox.SelectedIndex >= 0 && AktiviteterListBox.SelectedIndex < aktivitetsliste.liste.Count)
            {
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel = AktivitetTitelTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse = AktivitetBeskrivelseTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato = AktivitetDatoDatePicker.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].maxDeltagere = maxDeltagere;

                AktiviteterListBox.Items[AktiviteterListBox.SelectedIndex] = AktivitetTitelTextBox.Text + "     -     " + AktivitetDatoDatePicker.Text;
            }
            opretterAktivitet = false;

            AktivitetTitelTextBox.IsHitTestVisible = false;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = false;
            AktivitetDatoDatePicker.IsHitTestVisible = false;
            AktivitetMaxDeltagereTextBox.IsHitTestVisible = false;

            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;
            AktivitetMaxDeltagereTextBox.Clear();

            GemAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = true;
            NyAktivitetButton.IsHitTestVisible = true;
        }

        private void SletAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Er du sikker på at du vil slette aktiviteten?", "Slet aktivitet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                aktivitetsliste.liste.RemoveAt(AktiviteterListBox.SelectedIndex);
                AktiviteterListBox.Items.RemoveAt(AktiviteterListBox.SelectedIndex);

                AktivitetTitelTextBox.Clear();
                AktivitetBeskrivelseTextBox.Clear();
                AktivitetDatoDatePicker.SelectedDate = null;
                AktivitetMaxDeltagereTextBox.Clear();

                SletAktivitetButton.IsHitTestVisible = false;

            }
        }

        private void AktivitetTitelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AktivitetBeskrivelseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NyAktivitetButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(NyAktivitetButton.IsHitTestVisible == false)
            {
                NyAktivitetButton.Opacity = 0.5;
            }
            else
            {
                NyAktivitetButton.Opacity = 1;
            }
        }

        private void RedigerAktivitetButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(RedigerAktivitetButton.IsHitTestVisible == false)
            {
                RedigerAktivitetButton.Opacity = 0.5;
            }
            else
            {
                RedigerAktivitetButton.Opacity = 1;
            }
        }

        private void GemAktivitetButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(GemAktivitetButton.IsHitTestVisible == false)
            {
                GemAktivitetButton.Opacity = 0.5;
            }
            else
            {
                GemAktivitetButton.Opacity = 1;
            }
        }

        private void SletAktivitetButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SletAktivitetButton.IsHitTestVisible == false)
            {
                SletAktivitetButton.Opacity = 0.5;
            }
            else
            {
                SletAktivitetButton.Opacity = 1;
            }
        }

        private void MedlemmerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}