using LibrarieClase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
namespace Proiect_practicaDI
{
    public static class Metode
    {
        public static void Meniu()
        {
            Console.WriteLine("----MENIU----");
            Console.WriteLine("C. Citire utilizator.");
            Console.WriteLine("S. Salvare utilizator.");
            Console.WriteLine("A. Afisare utilizatori din fisier.");
            Console.WriteLine("L. Cautare utilizator dupa nume.");
            Console.WriteLine("M. Afiseaza adresa MAC a acestui PC.");
            Console.WriteLine("E. Sterge un utilizator din fisier.");
        }
        public static void AfisareUtilizatori(Utilizator[] utilizatori, int nrUtilizatori)
        {
            Console.WriteLine("Utilizatorii salvati in fisier sunt:");
            /*SE PARCURGE TABLOUL DE OBIECTE SI SE AFISEAZA INFORMATIILE IN FORMATUL CORESPUNZATOR*/
            for (int contor = 0; contor < nrUtilizatori; contor++)
            {
                string infoLocuri = utilizatori[contor].Info();
                Console.WriteLine(infoLocuri);
            }
        }
        public static string ValidareSiCorectareNumar(string numar)
        {
            if (numar.StartsWith("+40"))
            {
                /*Verificare daca numarul are exact 13 caractere si sunt doar cifre dupa +40*/
                if (numar.Length == 13 && numar.Substring(3).All(char.IsDigit))
                {
                    return numar;
                }
                else
                {
                    return null; /*Invalid*/
                }
            }
            else
            {
                /*Verificare daca numarul are exact 10 caractere si sunt doar cifre*/
                if (numar.Length == 10 && numar.All(char.IsDigit)&& numar.StartsWith("0"))
                {
                    return "+4" + numar; /*Adaugam prefixul +4 -> 0 fiind deja inclus*/
                }
                else
                {
                    return null; /*Invalid*/
                }
            }
        }
        public static string ValidareSiFormatareAdresaMac(string adresamac)
        {
            /*Elimina toate caracterele non-hexadecimale si normalizeaza*/
            var cleanAddress = new string(adresamac
                .Where(c => "0123456789ABCDEF".Contains(char.ToUpper(c)))
                .ToArray());
            /*Verificare daca are exact 12 caractere (6 octeti)*/
            if (cleanAddress.Length == 12)
            {
                /*Formateaza in formatul 00::11::22::33::44::55*/
                return string.Join("::", Enumerable.Range(0, cleanAddress.Length / 2)
                    .Select(i => cleanAddress.Substring(i * 2, 2)));
            }
            else
            {
                return null; /*Invalid*/
            }
        }
        public static Utilizator CitireUtilizatorTastatura()
        {
            Console.WriteLine("Introduceti datele utilizatorului:");
            Console.WriteLine("Nume:");
            string nume = Console.ReadLine();
            string numarcitit;
            do
            {
                Console.WriteLine("Numar:");
                numarcitit = Console.ReadLine();
                numarcitit = ValidareSiCorectareNumar(numarcitit);
                if (numarcitit == null)
                {
                    Console.WriteLine("Numărul de telefon introdus nu este valid. Te rugăm să încerci din nou.");
                }
            } while (numarcitit == null);
            string adresamac;
            do
            {
                Console.WriteLine("Adresa MAC (format: 00::11::22::33::44::55):");
                adresamac = Console.ReadLine();
                adresamac = ValidareSiFormatareAdresaMac(adresamac);
                if (adresamac == null)
                {
                    Console.WriteLine("Adresa MAC introdusă nu este validă. Te rugăm să încerci din nou.");
                }
            } while (adresamac == null);
            Utilizator utilizator = new Utilizator(nume, numarcitit, adresamac);
            return utilizator;
        }
        public static string GetMacAddress()
        {
            /*Obtine lista de placi de retea*/
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            /*Cautam prima placa de retea activa si obtinem adresa MAC*/
            foreach (var networkInterface in networkInterfaces)
            {
                /*Verifica daca placa de retea nu este de tip Loopback si este activs*/
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var macAddress = networkInterface.GetPhysicalAddress();
                    if (macAddress != null)
                    {
                        /*Formateaza adresa MAC intr-un sir de caractere hexazecimale*/
                        return string.Join("::", macAddress.GetAddressBytes().Select(b => b.ToString("X2")));
                    }
                }
            }
            return "Adresa MAC nu a putut fi gasita";
        }
    }
}