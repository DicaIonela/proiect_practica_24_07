using LibrarieClase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace NivelStocareDate
{
    public class Administrare_FisierText
    {
        private const int NR_MAX = 50;
        private string numeFisier;
        /*CONSTRUCTOR LINII FISIER*/
        public Administrare_FisierText(string numeFisier)
        {
            this.numeFisier= numeFisier;
            Stream streamFisier = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisier.Close();
        }
        /*ADAUGARE UTILIZATOR IN FISIER */
        public void AddUtilizator(Utilizator utilizator)
        {
            using (StreamWriter streamwriterFisierText = new StreamWriter(numeFisier, true))
            {
                streamwriterFisierText.WriteLine(utilizator.Conversie_PentruFisier());
            }
        }
        /*VERIFICA EXISTENTA UTILIZATORULUI IN FISIER PENTRU A EVITA SCRIEREA UNEI COPII*/
        public bool UtilizatorExista(string nume, string numar)
        {
            if (!File.Exists(numeFisier))
                return false;
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = streamReader.ReadLine()) != null)
                {
                    Utilizator utilizator = new Utilizator(linie);
                    if (utilizator.Nume == nume && utilizator.Numar == numar)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /*STOCHEAZA UTILIZATORII DIN FISIER INTR-UN TABLOU DE OBIECTE*/
        public Utilizator[] GetUtilizatori(out int nrUtilizatori)
        {
            Utilizator[] utilizatori = new Utilizator[NR_MAX];
            using (StreamReader streamrdr=new StreamReader(numeFisier))
            {
                string linieFisier;
                nrUtilizatori = 0;
                while ((linieFisier = streamrdr.ReadLine()) != null)
                {
                    if (linieFisier != "Nume;Numar;Adresa MAC")
                    {
                        utilizatori[nrUtilizatori++] = new Utilizator(linieFisier);
                    }
                }
                Array.Resize(ref utilizatori, nrUtilizatori);
            }
            return utilizatori;
        }
        public Utilizator[] CautaUtilizator(string criteriu)
        {
            int nrUtilizatori = 0;
            Utilizator[] utilizatori = GetUtilizatori(out nrUtilizatori);
            List <Utilizator> utilizatorigasiti= new List<Utilizator>();/*se creeaza o lista pentru utilizatorii gasiti*/
            foreach (Utilizator utilizator in utilizatori)/*se parcurge tabloul de obiecte*/
            {
                if (utilizator != null && utilizator.Nume != null && utilizator.Nume.Contains(criteriu) )/*se verifica daca numele contine literele introduse*/
                {
                    utilizatorigasiti.Add(utilizator);/*daca numele a indeplinit conditia, se va adauga obiectul la lista de utilizatori gasiti*/
                }
            }
            return utilizatorigasiti.ToArray();
        }
        public void StergeUtilizator(string nume)
        {
            if (!File.Exists(numeFisier))
            {
                Console.WriteLine("Fișierul nu există.");
                return;
            }
            /*Citeste toate liniile din fisier*/
            var linii = new List<string>(File.ReadAllLines(numeFisier));
            /*Cream o lista pentru liniile care nu contin utilizatorul de sters*/
            var liniiNou = new List<string>();
            foreach (var linie in linii)/*se parcurge lista*/
            {
                var utilizator = new Utilizator(linie);
                if (!utilizator.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase))/*Verificam daca linia contine numele utilizatorului care trebuie sters*/
                {
                    liniiNou.Add(linie);/*daca numele nu indeplineste criteriul, se va adauga in lista noua*/
                }
            }
            File.WriteAllLines(numeFisier, liniiNou);/*se pun inapoi in fisier liniile care nu au fost sterse*/
            Console.WriteLine("Utilizatorul cu numele '{0}' a fost sters, daca a fost gasit.", nume);
        }
    }
 }