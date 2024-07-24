using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NivelStocareDate;
using System.Configuration;
using LibrarieClase;
using System.Windows.Forms;
using InterfataUtilizator;
using System.Runtime.InteropServices;
namespace Proiect_practicaDI
{
    internal class Program
        {
        [DllImport("kernel32.dll")]/*Se importa functii pentru a ascunde/afisa consola*/
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        [STAThread]
            static void Main(string[] args)
            {
            IntPtr handle = GetConsoleWindow();/*Obtine handle-ul ferestrei consolei*/
            ShowWindow(handle, SW_HIDE);/*Ascunde consola*/
            /*Setam aplicatia pentru a folosi Windows Forms*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*Afisam fereastra de alegere*/
            DialogResult result = MessageBox.Show(
                "Doriti sa intrati in interfata grafica a aplicatiei?\n\n" +
                "*Selectand butonul No alegeti optiunea de a folosi aplicatia in consola.\n",
                "Selectați modul de utilizare",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Run(new InterfataGrafica());/*Deschide interfata grafica*/
            }
            else
            {
                ShowWindow(handle, SW_SHOW);/*reafiseaza consola*/
                StartCommandPromptMode(); /*Continua cu modul Command Prompt*/
            }
        }
        static void StartCommandPromptMode()
        {
            Init.Initialize(out Administrare_FisierText admin, out Utilizator utilizatornou); /*initializari*/
            Metode.Meniu();/*text meniu*/
            do
            {
                Console.WriteLine("\nIntrodu optiunea dorita:");
                string optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "C":
                        utilizatornou = Metode.CitireUtilizatorTastatura();
                        break;
                    case "S":
                        /* Verificare daca a fost introdus un utilizator nou */ 
                        if (utilizatornou.Nume!=string.Empty)
                        { 
                            admin.AddUtilizator(utilizatornou);
                            Console.WriteLine("Utilizatorul a fost adaugat cu succes.");
                        }
                        else
                        {
                            Console.WriteLine("Salvare nereusita. Nu ati introdus niciun utilizator nou.");
                        }
                        break;
                    case "A":
                        Utilizator[] utilizatori = admin.GetUtilizatori(out int nrUtilizatori);/*SE CREEAZA UN TABLOU DE OBIECTE*/
                        Metode.AfisareUtilizatori(utilizatori, nrUtilizatori);
                        break;
                    case "L":
                        Console.WriteLine("Introduceti criteriul de cautare:");
                        string criteriu = Console.ReadLine();
                        Utilizator[] utilizatoriGasiti = admin.CautaUtilizator(criteriu);
                        if (utilizatoriGasiti.Length > 0)
                        {
                            Metode.AfisareUtilizatori(utilizatoriGasiti, utilizatoriGasiti.Length);
                        }
                        else
                        {
                            Console.WriteLine("Nu s-au găsit utilizatori care să corespundă criteriului.");
                        }
                        break;
                    case "M":
                        string adresam = Metode.GetMacAddress();
                        Console.WriteLine("Adresa MAC a calculatorului este: " + adresam);
                        break;
                    case "E":
                        Console.WriteLine("Introdu numele utilizatorului de sters:");
                        string numedesters=Console.ReadLine();
                        admin.StergeUtilizator(numedesters);
                        break;
                }
            } while (true);
        }
    }
}
