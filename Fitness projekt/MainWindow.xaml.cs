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
        public MainWindow()
        {
            InitializeComponent();
        }
        class Medlem
        {
            public string navn;
            public int alder;
            public string brugerNavn;
            public string adgangsKode;
            public Medlemskab medlemskab;
        }
        class Administrator
        {
            public string brugerNavn;
            public string adgangsKode;
        }
        class Aktivitet
        {
            public string titel;
            public string beskrivelse;
            public string dato;
            public Deltagerliste deltagere;
        }
        class Medlemskab
        {
            public bool erMedlem;
        }
        class Aktivitetsliste
        {
            public List<Aktivitet> liste = new List<Aktivitet>();
        }
        class Medlemsliste
        {
            public List<Medlem> liste = new List<Medlem>();
        }
        class Deltagerliste
        {
            public List<Medlem> liste = new List<Medlem>();
        }
    }
}