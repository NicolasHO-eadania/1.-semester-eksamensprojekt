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
using static Fitness_projekt.MainWindow;

namespace Fitness_projekt
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // opretter et administrator objekt med brugernavn og adgangskode
        Administrator administrator = new Administrator("Admin", "Admin123");

        // click event for log ind knappen
        private void LogIndButton_Click(object sender, RoutedEventArgs e)
        {
            // brugernavn og adgangskode fra tekstbokse
            string inputBrugernavn = BrugernavnTextBox.Text;
            string inputAdgangskode = AdgangskodeTextBox.Text;

            // tjekker om brugernavn og adgangskode matcher administrator objektet
            if (inputBrugernavn == administrator.brugernavn && inputAdgangskode == administrator.adgangskode)
            {
                // åbner administrator vinduet (MainWindow)
                MainWindow AdministratorWindow = new MainWindow();
                AdministratorWindow.Show();
                Close();
                return;
            }

            // læser medlemmer fra fil og tjekker om brugernavn og adgangskode matcher et medlem
            string[] FilLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");
            int i = 0;
            while (i < FilLines.Length)
            {
                string[] MedlemVariabler = FilLines[i].Split(";");
                if (inputBrugernavn == MedlemVariabler[3] && inputAdgangskode == MedlemVariabler[4])
                {
                    // åbner medlem vinduet
                    MedlemWindow medlemWindow = new MedlemWindow(inputBrugernavn, inputAdgangskode);
                    medlemWindow.Show();
                    Close();
                    return;
                }
                i++;
            }
            // viser fejlbesked hvis brugernavn eller adgangskode er forkert
            MessageBox.Show("Forkert brugernavn eller adgangskode", "Fejl", MessageBoxButton.OK);
        }
    }
}
