using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public MainWindow()
        {
            InitializeComponent();
        }
        public class Medlem
        {
            public string navn;
            public int alder;
            public string brugerNavn;
            public string adgangsKode;
            public Medlemskab medlemskab;
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

            public Aktivitet(string titel, string beskrivelse, string dato)
            {
                deltagere = new Deltagerliste();
                this.titel = titel;
                this.beskrivelse = beskrivelse;
                this.dato = dato;
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

        private void NyAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;
            GemAktivitetButton.IsHitTestVisible = true;
        }

        private void AktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            AktivitetTitelTextBox.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel;
            AktivitetBeskrivelseTextBox.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse;
            AktivitetDatoDatePicker.Text = aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato;

            RedigerAktivitetButton.IsHitTestVisible = true;
        }

        private void RedigerAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            GemAktivitetButton.IsHitTestVisible = true;

            AktivitetTitelTextBox.IsHitTestVisible = true;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = true;
            AktivitetDatoDatePicker.IsHitTestVisible = true;

            RedigerAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = false;
            NyAktivitetButton.IsHitTestVisible = false;
        }

        private void GemAktivitetButton_Click(object sender, RoutedEventArgs e)
        {
            if(NyAktivitetButton.IsHitTestVisible == true)
            {
                Aktivitet nyAktivitet = new Aktivitet(AktivitetTitelTextBox.Text, AktivitetBeskrivelseTextBox.Text, AktivitetDatoDatePicker.Text);
                aktivitetsliste.liste.Add(nyAktivitet);
                AktiviteterListBox.Items.Add(nyAktivitet.titel);
            }
            else
            {
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].titel = AktivitetTitelTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].beskrivelse = AktivitetBeskrivelseTextBox.Text;
                aktivitetsliste.liste[AktiviteterListBox.SelectedIndex].dato = AktivitetDatoDatePicker.Text;
                AktiviteterListBox.Items[AktiviteterListBox.SelectedIndex] = AktivitetTitelTextBox.Text;
            }
            AktivitetTitelTextBox.IsHitTestVisible = false;
            AktivitetBeskrivelseTextBox.IsHitTestVisible = false;
            AktivitetDatoDatePicker.IsHitTestVisible = false;

            AktivitetTitelTextBox.Clear();
            AktivitetBeskrivelseTextBox.Clear();
            AktivitetDatoDatePicker.SelectedDate = null;

            GemAktivitetButton.IsHitTestVisible = false;
            AktiviteterListBox.IsHitTestVisible = true;
            NyAktivitetButton.IsHitTestVisible = true;
        }

        private void AktivitetTitelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AktivitetBeskrivelseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}