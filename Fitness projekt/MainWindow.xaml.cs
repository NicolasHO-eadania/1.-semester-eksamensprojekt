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
        //startup window  +  hint tekst  +  validering (1/2)  +  deltagere(1/2)  +  kommentarer  +  medlemmer tab  +  medlemskabstyper  +  (gem ændringer til filer)






        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();
        public Medlemsliste medlemsliste = new Medlemsliste();

        public bool opretterAktivitet = false;

        
        //-----------Startmedlemmer og aktiviteter----------//

        void LæsMedlemmerFil()
        {
            string[] filLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");

            int i = 0;
            while(i < filLines.Length)
            {
                string[] MedlemVariabler = filLines[i].Split(";");
                int alder = Convert.ToInt32(MedlemVariabler[2]);
                Medlem medlem = new Medlem(MedlemVariabler[0], MedlemVariabler[1], alder, MedlemVariabler[3], MedlemVariabler[4]);
                medlemsliste.liste.Add(medlem);
                MedlemmerListBox.Items.Add(MedlemVariabler[0] + " " + MedlemVariabler[1]);
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

                aktivitetsliste.liste.Add(aktivitet);
                AktiviteterListBox.Items.Add(AktivitetVariabler[0] + "     -     " + AktivitetVariabler[2]);
                i++;
            }
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


        // hjælp fra GitHub Copilot
        void GemAktiviteterFil()
        {
            List<string> FilLines = new List<string>();
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

                FilLines.Add(string.Join(";", dele));
                i++;
            }

            File.WriteAllLines(@"AktiviteterFil.txt", FilLines);
        }




        //--------------------------------------------------//


        public MainWindow()
        {
            InitializeComponent();
            LæsMedlemmerFil();
            LæsAktiviteterFil();
        }


        private void NyAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            opretterAktivitet = true;
            
            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;
            AktivitetMaxDeltagereTextBox.Clear();
            AktivitetDeltagereListBox.Items.Clear();

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;
            AktivitetMaxDeltagereTextBox.IsHitTestVisible = true;


            RedigerAktivitetButton.IsHitTestVisible = false;
            SletAktivitetButton.IsHitTestVisible = false;
            NyAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = false;
        }

        private void AktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AktiviteterListBox.SelectedIndex < 0 || AktiviteterListBox.SelectedIndex >= aktivitetsliste.liste.Count)
            {
                RedigerAktivitetButton.IsHitTestVisible = false;
                SletAktivitetButton.IsHitTestVisible = false;
                return;
            }

            Aktivitet valgt = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex];

            AktivitetTitelTextBox.Text = valgt.titel;
            AktivitetBeskrivelseTextBox.Text = valgt.beskrivelse;
            AktivitetDatoDatePicker.Text = valgt.dato;
            AktivitetMaxDeltagereTextBox.Text = valgt.deltagere.liste.Count + " / " + valgt.maxDeltagere.ToString();

            AktivitetDeltagereListBox.Items.Clear();
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
            if (opretterAktivitet == true)
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

                Aktivitet nyAktivitet = new Aktivitet(AktivitetTitelTextBox.Text, AktivitetBeskrivelseTextBox.Text, AktivitetDatoDatePicker.Text, maxDeltagere);
                aktivitetsliste.liste.Add(nyAktivitet);

                AktiviteterListBox.Items.Add(nyAktivitet.titel + "     -     " + nyAktivitet.dato);

                GemAktiviteterFil();
                
                //retter valget i listen til den nye aktivitet der blev oprettet
                AktiviteterListBox.SelectedIndex = AktiviteterListBox.Items.Count - 1;
            }
            else if (AktiviteterListBox.SelectedIndex >= 0 && AktiviteterListBox.SelectedIndex < aktivitetsliste.liste.Count)
            {
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel = AktivitetTitelTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse = AktivitetBeskrivelseTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato = AktivitetDatoDatePicker.Text;

                AktiviteterListBox.Items[AktiviteterListBox.SelectedIndex] = AktivitetTitelTextBox.Text + "     -     " + AktivitetDatoDatePicker.Text;
                GemAktiviteterFil();
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
            AktivitetDeltagereListBox.Items.Clear();

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
                AktivitetDeltagereListBox.Items.Clear();

                SletAktivitetButton.IsHitTestVisible = false;

                GemAktiviteterFil();
            }
        }

        private void AktivitetTitelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        

        //---------------------------------------------------------------------------------------



        private void MedlemmerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}