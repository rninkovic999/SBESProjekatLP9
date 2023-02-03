using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Database
    {
        private static string path = @"..\..\items";

        public static List<Koncert> koncerti;
        public static List<Rezervacija> rezervacije;
        public static List<Korisnik> korisnici;
        public static double Racun { get; set; }

        public static List<Koncert> ReadKoncerti()
        {
            List<Koncert> kon = new List<Koncert>();
            try
            {
                string paths = Path.GetFullPath(path + "koncerti.txt");

                FileStream stream = new FileStream(paths, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tok = line.Split(';');
                    string[] dtok = tok[2].Split('/');
                    DateTime date = new DateTime(int.Parse(dtok[2]), int.Parse(dtok[1]), int.Parse(dtok[0]));

                    Koncert k = new Koncert(int.Parse(tok[0]), tok[1], date, tok[3], double.Parse(tok[4]));
                    kon.Add(k);
                }

                reader.Close();
                stream.Close();
                try
                {
                    Audit.ReadFromFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "koncerti.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.ReadFromFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "koncerti.txt", e.Message);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.Write($"Greska prilikom ucitavanja koncerta:{ e.Message}");
            }
            return kon;
        }

        public static List<Korisnik> ReadKorisnici()
        {
            List<Korisnik> kor = new List<Korisnik>();
            try
            {
                string paths = Path.GetFullPath(path + "korisnici.txt");

                FileStream stream = new FileStream(paths, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    List<Rezervacija> reze = new List<Rezervacija>();
                    string[] tok = line.Split(';');
                    Korisnik korisnik = null;
                    int brojac = tok[3].Count(x => x == ',');
                    if (brojac != 0)
                    {
                        string[] id = tok[3].Split(',');
                        for (int i = 0; i < brojac; i++)
                        {
                            foreach (Rezervacija rez in rezervacije)
                            {
                                if (int.Parse(id[i]) == rez.Id)
                                {
                                    reze.Add(rez);
                                }
                            }
                        }
                        korisnik = new Korisnik(tok[0], tok[1], double.Parse(tok[2]));
                    }
                    else
                    {

                    }
                    kor.Add(korisnik);
                }

                reader.Close();
                stream.Close();
                try
                {
                    Audit.ReadFromFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "korisnici.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.ReadFromFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "korisnici.txt", e.Message);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.Write($"Greska prilikom ucitavanja koncerta:{ e.Message}");
            }
            return kor;
        }

        public static void ReadRacun() {
            try
            {
                string paths = Path.GetFullPath(path + "racuni.txt");

                FileStream stream = new FileStream(paths, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    Racun = int.Parse(line);
                }

                reader.Close();
                stream.Close();
                try
                {
                    Audit.ReadFromFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "racuni.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.ReadFromFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "racuni.txt", e.Message);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.Write($"Greska prilikom ucitavanja koncerta:{ e.Message}");
            }
        }

        public static List<Rezervacija> ReadRezervacije()
        {
            List<Rezervacija> re = new List<Rezervacija>();
            try
            {
                string paths = Path.GetFullPath(path + "rezervacije.txt");

                FileStream stream = new FileStream(paths, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tok = line.Split(';');
                    string[] dtok = tok[2].Split('/');
                    DateTime date = new DateTime(int.Parse(dtok[2]), int.Parse(dtok[1]), int.Parse(dtok[0]));

                    Rezervacija r = new Rezervacija(int.Parse(tok[0]), int.Parse(tok[1]), date, int.Parse(tok[3]));
                    r.Stanje = (StanjeRezervacije)Enum.Parse(typeof(StanjeRezervacije), tok[4]);
                    re.Add(r);
                }

                reader.Close();
                stream.Close();
                try
                {
                    Audit.ReadFromFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "rezervacije.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.ReadFromFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "rezervacije.txt", e.Message);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.Write($"Greska prilikom ucitavanja koncerta:{ e.Message}");
            }
            return re;
        }

        public static void WriteKoncert(Koncert k)
        {
            try
            {
                string paths = Path.GetFullPath(path + "koncerti.txt");
                File.AppendAllText(paths, k.Write());
                try
                {
                    Audit.WriteInFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "koncerti.txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.WriteInFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "koncerti.txt", e.Message);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                Console.Write($"Greska prilikom zapisa koncerta{k.Id}, greska:{ e.Message}");
            }
        }

        public static void WriteKoncerti()
        {
            try
            {
                string paths = Path.GetFullPath(path + "koncerti.txt");
                File.AppendAllText(paths, String.Empty);

                foreach (Koncert k in koncerti)
                {
                    WriteKoncert(k);
                }
            }
            catch (Exception e)
            {
                Console.Write($"Greska prilikom zapisa svih koncerata: {e.Message}");
            }
        }

        public static void WriteKorisnik(Korisnik ko)
        {
            try
            {
                string paths = Path.GetFullPath(path + "korisnici.txt");
                File.AppendAllText(paths, ko.Write());
                try
                {
                    Audit.WriteInFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "korisnici.txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.WriteInFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "korisnici.txt", e.Message);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                Console.Write($"Greska prilikom zapisa korisnika{ko.Username}, greska:{ e.Message}");
            }
        }

        public static void WriteKorisnici()
        {
            try
            {
                string paths = Path.GetFullPath(path + "koncerti.txt");
                File.AppendAllText(paths, String.Empty);

                foreach (Korisnik ko in korisnici)
                {
                    WriteKorisnik(ko);
                }
            }
            catch (Exception e)
            {
                Console.Write($"Greska prilikom zapisa svih koncerata{e.Message}");
            }
        }

        public static void WriteRacuni()
        {
            try
            {
                string paths = Path.GetFullPath(path + "racuni.txt");
                File.WriteAllText(paths, String.Empty);
                File.AppendAllText(paths, Racun.ToString());

                try
                {
                    Audit.WriteInFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "racuni.txt");
                }
                catch (Exception e)
                {
                    try
                    {
                        Audit.WriteInFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "racuni.txt", e.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    Console.Write(e.Message);
                }
            }
            catch (Exception e)
            {
                Console.Write($"Greska prilikom zapisa racuna:{ e.Message}");
            }
        }

        public static void WriteRezervacija(Rezervacija r)
        {
            try
            {
                string paths = Path.GetFullPath(path + "rezervacije.txt");
                File.AppendAllText(paths, r.Write());
                try
                {
                    Audit.WriteInFileSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "rezervacije.txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                try
                {
                    Audit.WriteInFileFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "rezervacije.txt", e.Message);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                Console.Write($"Greska prilikom zapisa korisnika{r.Id}, greska:{ e.Message}");
            }
        }

        public static void WriteRezervacije()
        {
            try
            {
                string paths = Path.GetFullPath(path + "koncerti.txt");
                File.AppendAllText(paths, String.Empty);

                foreach (Rezervacija r in rezervacije)
                {
                    WriteRezervacija(r);
                }
            }
            catch (Exception e)
            {
                Console.Write($"Greska prilikom zapisa svih rezervacija{e.Message}");
            }
        }

    }
}
