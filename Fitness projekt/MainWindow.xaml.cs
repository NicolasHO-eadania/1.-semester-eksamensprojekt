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
        // lister til medlemmer og aktiviteter
        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();
        public Medlemsliste medlemsliste = new Medlemsliste();

        // boolean til at tjekke om man opretter eller redigerer en aktivitet
        public bool opretterAktivitet = false;

        //---------------------------------------------------------------------------------------------------------------------------------------------

        // metode til at læse filen med medlemmer
        void LæsMedlemmerFil()
        {
            // læser alle linjer i filen og gemmer dem i et array af strings
            string[] filLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");

            // loop der går igennem hver linje i filen
            int i = 0;
            while(i < filLines.Length)
            {
                // splitter linjen med ";"
                string[] medlemVariabler = filLines[i].Split(";");
                // konverterer fra string til int
                int alder = Convert.ToInt32(medlemVariabler[2]);
                // opretter et nyt medlem objekt med variablerne fra filen
                Medlem medlem = new Medlem(medlemVariabler[0], medlemVariabler[1], alder, medlemVariabler[3], medlemVariabler[4], medlemVariabler[5]);
                // tilføjer medlemmet til medlemslisten og til medlem listboxen
                medlemsliste.liste.Add(medlem);
                MedlemmerListBox.Items.Add(medlem.fornavn + " " + medlem.efternavn);
                i++;
            }
        }

        // metode til at læse filen med aktiviteter
        void LæsAktiviteterFil()
        {
            // læser alle linjer i filen og gemmer dem i et array af strings
            string[] filLines = System.IO.File.ReadAllLines(@"AktiviteterFil.txt");

            // loop der går igennem hver linje i filen
            int i = 0;
            while (i < filLines.Length)
            {
                // splitter linjen med ";"
                string[] aktivitetVariabler = filLines[i].Split(";");
                // konverterer fra string til int
                int maxDeltagere = Convert.ToInt32(aktivitetVariabler[3]);
                // opretter et nyt aktivitet objekt med variablerne fra filen
                Aktivitet aktivitet = new Aktivitet(aktivitetVariabler[0], aktivitetVariabler[1], aktivitetVariabler[2], maxDeltagere);

                // kalder metode der tilføjer deltagere til aktiviteten
                TilføjDeltagere(aktivitet, aktivitetVariabler);

                // tilføjer aktiviteten til aktivitetslisten og til aktiviteter listboxen
                aktivitetsliste.liste.Add(aktivitet);
                AktiviteterListBox.Items.Add(aktivitet.titel + "     -     " + aktivitet.dato);
                i++;
            }
        }

        // metode til at tilføje deltagere til en aktivitet -- jeg har fået hjælp fra GitHub Copilot for at få en ide til hvordan jeg kunne gøre dette (se bilag)
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
                j++;
            }
        }

        // metode til at gemme aktiviteter til fil -- jeg fik hjælp fra GitHub Copilot og fandt ud af at jeg skulle bruge string.Join (se bilag)
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

        //---------------------------------------------------------------------------------------------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();
            LæsMedlemmerFil();
            LæsAktiviteterFil();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------

        // click event til ny aktivitet
        private void NyAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            opretterAktivitet = true;

            // bruger clear til at tømme tekstbokse og listbox
            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;
            AktivitetMaxDeltagereTextBox.Clear();
            AktivitetDeltagereListBox.Items.Clear();

            // gør at tekstbokse og datepicker kan redigeres
            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;
            AktivitetMaxDeltagereTextBox.IsHitTestVisible = true;

            // gøt at nogle knapper og listbox ikke kan bruges
            RedigerAktivitetButton.IsHitTestVisible = false;
            SletAktivitetButton.IsHitTestVisible = false;
            NyAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = false;
        }

        // selection changed event til aktiviteter listbox
        private void AktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if statement der tjekker om der er valgt en aktivitet og forhindre en fejl
            if (AktiviteterListBox.SelectedIndex < 0 || AktiviteterListBox.SelectedIndex >= aktivitetsliste.liste.Count)
            {
                RedigerAktivitetButton.IsHitTestVisible = false;
                SletAktivitetButton.IsHitTestVisible = false;
                return;
            }
            // viser information om den valgte aktivitet i tekstbokse og listbox
            Aktivitet valgt = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex];
            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoDatePicker.Text = valgt.dato;
            AktivitetMaxDeltagereTextBox.Text = valgt.deltagere.liste.Count + " / " + valgt.maxDeltagere.ToString();
            
            AktivitetDeltagereListBox.Items.Clear();
            // loop der går igennem hver deltager i den valgte aktivitet og tilføjer dem til deltagere listboxen
            if (valgt.deltagere.liste.Count > 0)
            {
                int i = 0;
                while (i < valgt.deltagere.liste.Count)
                {
                    Medlem medlem = valgt.deltagere.liste[i];
                    AktivitetDeltagereListBox.Items.Add(medlem.fornavn + " " + medlem.efternavn + " (" + medlem.brugernavn + ")");
                    i++;
                }
            }
            RedigerAktivitetButton.IsHitTestVisible = true;
            SletAktivitetButton.IsHitTestVisible = true;
        }

        // click event til rediger aktivitet
        private void RedigerAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            opretterAktivitet = false;

            // gør at tekstbokse og datepicker kan redigeres
            GemAktivitetButton.IsHitTestVisible = true;

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;

            RedigerAktivitetButton.IsHitTestVisible = false;
            SletAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = false;
            NyAktivitetButton.IsHitTestVisible = false;
        }

        // click event til gem aktivitet
        private void GemAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            // hvis man opretter en ny aktivitet
            if (opretterAktivitet == true)
            {
                // tjekker om max deltagere tekstboksen er tom eller indeholder et tal
                int maxDeltagere = 999;
                if (!string.IsNullOrEmpty(AktivitetMaxDeltagereTextBox.Text))
                {
                    if (!int.TryParse(AktivitetMaxDeltagereTextBox.Text, out int maxDeltagereAendret))
                    {
                        return;
                    }
                    maxDeltagere = maxDeltagereAendret;
                }
                // opretter en ny aktivitet med de informationer man har indtastet og tilføjer den til aktivitetslisten og aktiviteter listboxen
                Aktivitet nyAktivitet = new Aktivitet(AktivitetTitelTextBox.Text, AktivitetBeskrivelseTextBox.Text, AktivitetDatoDatePicker.Text, maxDeltagere);
                aktivitetsliste.liste.Add(nyAktivitet);
                AktiviteterListBox.Items.Add(nyAktivitet.titel + "     -     " + nyAktivitet.dato);

                // gemmer aktiviteter til fil
                GemAktiviteterFil();

                //retter valget i listen så man ikke får en fejl
                AktiviteterListBox.SelectedIndex = AktiviteterListBox.Items.Count - 1;
            }
            // hvis man redigerer en aktivitet
            else if (AktiviteterListBox.SelectedIndex >= 0 && AktiviteterListBox.SelectedIndex < aktivitetsliste.liste.Count)
            {
                // opdaterer den valgte aktivitet med de nye informationer og gemmer aktiviteter til fil
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel = AktivitetTitelTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse = AktivitetBeskrivelseTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato = AktivitetDatoDatePicker.Text;

                AktiviteterListBox.Items[AktiviteterListBox.SelectedIndex] = AktivitetTitelTextBox.Text + "     -     " + AktivitetDatoDatePicker.Text;
                GemAktiviteterFil();
            }
            opretterAktivitet = false;

            // ændrer hvad der kan klikkes på og rydder tekstbokse og listbox
            AktivitetTitelTextBox.IsHitTestVisible = false;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = false;
            AktivitetDatoDatePicker.IsHitTestVisible = false;
            AktivitetMaxDeltagereTextBox.IsHitTestVisible = false;

            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;
            AktivitetMaxDeltagereTextBox.Clear();
            AktivitetDeltagereListBox.Items.Clear();

            GemAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = true;
            NyAktivitetButton.IsHitTestVisible = true;
        }

        // click event til slet aktivitet
        private void SletAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            // spørger brugeren om de er sikre på at de vil slette aktiviteten
            if (MessageBox.Show("Er du sikker på at du vil slette aktiviteten?", "Slet aktivitet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // hvis ja, sletter aktiviteten fra aktivitetslisten og aktiviteter listboxen
                aktivitetsliste.liste.RemoveAt(AktiviteterListBox.SelectedIndex);
                AktiviteterListBox.Items.RemoveAt(AktiviteterListBox.SelectedIndex);

                AktivitetTitelTextBox.Clear();
                AktivitetBeskrivelseTextBox.Clear();
                AktivitetDatoDatePicker.SelectedDate = null;
                AktivitetMaxDeltagereTextBox.Clear();
                AktivitetDeltagereListBox.Items.Clear();

                SletAktivitetButton.IsHitTestVisible = false;

                // gemmer aktiviteter til fil
                GemAktiviteterFil();
            }
        }

        private void AktivitetTitelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // tjekker om alle nødvendige felter er udfyldt for at kunne gemme en aktivitet
            if (!string.IsNullOrWhiteSpace(AktivitetTitelTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetBeskrivelseTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetDatoDatePicker.Text) && AktiviteterListBox.IsHitTestVisible == false)
            {
                GemAktivitetButton.IsHitTestVisible = true;
            }
            else
            {
                GemAktivitetButton.IsHitTestVisible = false;
            }
        }

        private void AktivitetBeskrivelseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // tjekker om alle nødvendige felter er udfyldt for at kunne gemme en aktivitet
            if (!string.IsNullOrWhiteSpace(AktivitetBeskrivelseTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetTitelTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetDatoDatePicker.Text) && AktiviteterListBox.IsHitTestVisible == false)
            {
                GemAktivitetButton.IsHitTestVisible = true;
            }
            else
            {
                GemAktivitetButton.IsHitTestVisible = false;
            }
        }

        private void AktivitetDatoDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // tjekker om alle nødvendige felter er udfyldt for at kunne gemme en aktivitet
            if (!string.IsNullOrWhiteSpace(AktivitetBeskrivelseTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetTitelTextBox.Text) && !string.IsNullOrWhiteSpace(AktivitetDatoDatePicker.Text) && AktiviteterListBox.IsHitTestVisible == false)
            {
                GemAktivitetButton.IsHitTestVisible = true;
            }
            else
            {
                GemAktivitetButton.IsHitTestVisible = false;
            }
        }

        private void NyAktivitetButton_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // gør det tydeligt om knappen kan bruges
            if (NyAktivitetButton.IsHitTestVisible == false)
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
            // gør det tydeligt om knappen kan bruges
            if (RedigerAktivitetButton.IsHitTestVisible == false)
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
            // gør det tydeligt om knappen kan bruges
            if (GemAktivitetButton.IsHitTestVisible == false)
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
            // gør det tydeligt om knappen kan bruges
            if (SletAktivitetButton.IsHitTestVisible == false)
            {
                SletAktivitetButton.Opacity = 0.5;
            }
            else
            {
                SletAktivitetButton.Opacity = 1;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------

        // selection changed event til medlemmer listbox i medlemmer tabben
        private void MedlemmerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // rydder tekstbokse
            MedlemNavnTextBox.Clear();
            MedlemAlderTextBox.Clear();
            MedlemBrugernavnTextBox.Clear();
            MedlemAdgangskodeTextBox.Clear();
            MedlemMedlemskabTextBox.Clear();

            // viser information om det valgte medlem i tekstbokse
            Medlem valgt = medlemsliste.liste[MedlemmerListBox.SelectedIndex];

            MedlemNavnTextBox.Text = valgt.fornavn + " " + valgt.efternavn;
            MedlemAlderTextBox.Text = valgt.alder.ToString();
            MedlemBrugernavnTextBox.Text = valgt.brugernavn;
            MedlemAdgangskodeTextBox.Text = valgt.adgangskode;
            MedlemMedlemskabTextBox.Text = valgt.medlemskab;
        }
    }
}