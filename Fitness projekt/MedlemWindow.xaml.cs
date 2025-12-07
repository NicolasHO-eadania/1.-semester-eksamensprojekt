using System;
using System.Collections.Generic;
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
        public Aktivitetsliste aktivitetsliste = new Aktivitetsliste();

        void LæsAktiviteterFil()
        {
            string[] FilLines = System.IO.File.ReadAllLines(@"AktiviteterFil.txt");

            int i = 0;
            while (i < FilLines.Length)
            {
                string[] AktivitetVariabler = FilLines[i].Split(";");
                int maxDeltagere = Convert.ToInt32(AktivitetVariabler[3]);
                Aktivitet aktivitet = new Aktivitet(AktivitetVariabler[0], AktivitetVariabler[1], AktivitetVariabler[2], maxDeltagere);
                aktivitetsliste.liste.Add(aktivitet);
                AlleAktiviteterListBox.Items.Add(AktivitetVariabler[0] + "     -     " + AktivitetVariabler[2]);
                i++;
            }
        }


        public MedlemWindow()
        {
            InitializeComponent();
            LæsAktiviteterFil();
        }

        private void MineAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AktivitetTitelTextBox.Text = aktivitetsliste.liste[MineAktiviteterListBox.SelectedIndex].titel;
            AktivitetBeskrivelseTextBox.Text = aktivitetsliste.liste[MineAktiviteterListBox.SelectedIndex].beskrivelse;
            AktivitetDatoTextBox.Text = aktivitetsliste.liste[MineAktiviteterListBox.SelectedIndex].dato;
            AktivitetDeltagereTextBlock.Text = aktivitetsliste.liste[MineAktiviteterListBox.SelectedIndex].maxDeltagere.ToString();

            ForladButton.IsHitTestVisible = true;
            DeltagButton.IsHitTestVisible = false;
        }

        private void AlleAktiviteterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AktivitetTitelTextBox.Text = aktivitetsliste.liste[AlleAktiviteterListBox.SelectedIndex].titel;
            AktivitetBeskrivelseTextBox.Text = aktivitetsliste.liste[AlleAktiviteterListBox.SelectedIndex].beskrivelse;
            AktivitetDatoTextBox.Text = aktivitetsliste.liste[AlleAktiviteterListBox.SelectedIndex].dato;
            AktivitetDeltagereTextBlock.Text = aktivitetsliste.liste[AlleAktiviteterListBox.SelectedIndex].maxDeltagere.ToString();

            ForladButton.IsHitTestVisible = false;
            DeltagButton.IsHitTestVisible = true;

        }

        private void DeltagButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ForladButton_Click(object sender, RoutedEventArgs e)
        {

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
