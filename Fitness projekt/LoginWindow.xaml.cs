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

        Administrator administrator = new Administrator("Admin", "Admin123");

        private void LogIndButton_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = BrugernavnTextBox.Text;
            string adgangskode = AdgangskodeTextBox.Text;

            if(brugernavn == administrator.brugernavn && adgangskode == administrator.adgangskode)
            {
                MainWindow AdministratorWindow = new MainWindow();
                AdministratorWindow.Show();
                Close();
                return;
            }

            string[] FilLines = System.IO.File.ReadAllLines(@"MedlemmerFil.txt");
            int i = 0;
            while (i < FilLines.Length)
            {
                string[] MedlemVariabler = FilLines[i].Split(";");
                if (brugernavn == MedlemVariabler[3] && adgangskode == MedlemVariabler[4])
                {
                    MedlemWindow medlemWindow = new MedlemWindow(brugernavn, adgangskode);
                    medlemWindow.Show();
                    Close();
                    return;
                }
                i++;
            }

        }
    }
}
