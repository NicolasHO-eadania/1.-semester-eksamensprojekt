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

        //--------------------forkert brugernavn eller adgangskode--------------------

        public LoginWindow()
        {
            InitializeComponent();
        }

        Administrator administrator = new Administrator("Admin", "Admin123");

        private void LogIndButton_Click(object sender, RoutedEventArgs e)
        {
            string inputBrugernavn = BrugernavnTextBox.Text;
            string inputAdgangskode = AdgangskodeTextBox.Text;

            if(inputBrugernavn == administrator.brugernavn && inputAdgangskode == administrator.adgangskode)
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
                if (inputBrugernavn == MedlemVariabler[3] && inputAdgangskode == MedlemVariabler[4])
                {
                    MedlemWindow medlemWindow = new MedlemWindow(inputBrugernavn, inputAdgangskode);
                    medlemWindow.Show();
                    Close();
                    return;
                }
                i++;
            }

        }
    }
}
