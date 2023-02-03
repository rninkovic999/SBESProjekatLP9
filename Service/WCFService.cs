using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public void DodajKoncert(int id, string naziv, DateTime vremePocetka, string lokacija, double cenaKarte, out int idKoncerta)
        {
            CustomPrincipal pr = Thread.CurrentPrincipal as CustomPrincipal;
            idKoncerta = -1;

            if(pr.IsInRole("Admin"))
            {
                Console.WriteLine("\nDodaj koncert...");
                Koncert koncert = null;
                if(Database.koncerti.Count()>0)
                {
                    koncert = new Koncert(Database.koncerti.Count(), naziv, vremePocetka, lokacija, cenaKarte);
                    idKoncerta = Database.koncerti.Count();
                }
                else
                {
                    koncert = new Koncert(0, naziv, vremePocetka, lokacija, cenaKarte);
                    idKoncerta = 0;
                }
                Database.koncerti.Add(koncert);

                try
                {
                    Audit.AddSuccess("bb", "koncert");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Database.WriteKoncerti();
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed("nn", "Dodaj koncert", "Admin moze da dodaje koncerte");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void IzmeniKoncert(int id, string naziv, DateTime vremePocetka, string lokacija, double cenaKarte)
        {
            CustomPrincipal pr = Thread.CurrentPrincipal as CustomPrincipal;

            if (pr.IsInRole("Admin"))
            {
                Console.WriteLine("\nIzmena koncerta...");

                for(int i=0; i<Database.koncerti.Count(); i++)
                {
                    if(id.Equals(Database.koncerti[i].Id))
                    {
                        Database.koncerti[i].Naziv = naziv;
                        Database.koncerti[i].VremePocetka = vremePocetka;
                        Database.koncerti[i].Lokacija = lokacija;
                        Database.koncerti[i].CenaKarte = cenaKarte;
                    }
                }

                try
                {
                    Audit.ChangeSuccess("NN", "koncert");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Database.WriteKoncerti();
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed("nn", "Dodaj koncert", "Admin moze da menja koncerte");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void NapraviRezervaciju(int idKoncerta, DateTime vremeRezervacije, int kolicinaKarata, out int idRezervacije)
        {
            CustomPrincipal pr = Thread.CurrentPrincipal as CustomPrincipal;
            idRezervacije = -1;

            if (pr.IsInRole("Korisnik") || pr.IsInRole("Dobri"))
            {
                Console.WriteLine("Napraviti rezervaciju");
                Rezervacija r = null;

                for (int i = 0; i < Database.korisnici.Count(); i++)
                {
                    if (Database.korisnici[i].Username.Equals("nn"))
                    {
                        if(Database.rezervacije.Count()>0)
                        {
                            r = new Rezervacija(0, idKoncerta, vremeRezervacije, kolicinaKarata);
                            idRezervacije = 0;
                        }
                        Database.korisnici[i].Rezervacije.Add(r);

                        try
                        {
                            Audit.AddSuccess("nn", "rezervacije");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                Database.rezervacije.Add(r);
                Database.WriteRezervacije();
                Database.WriteKorisnici();
            }
            else
            {
                Audit.AuthorizationFailed("bb", "Napravi rezervaciju", "Rezervacija moze biti napravljena samo od grupe Korisnik ili grupe dobri");
            }
        }

        public void PlatiRezervaciju(int id)
        {
            CustomPrincipal pr = Thread.CurrentPrincipal as CustomPrincipal;

            if(pr.IsInRole("Korisnik")||pr.IsInRole("Dobri"))
            {
                Console.WriteLine("Platiti rezervaciju");
                foreach(Korisnik k in Database.korisnici)
                {
                    if(k.Username.Equals("nn"))
                    {
                        foreach(Rezervacija r in k.Rezervacije)
                        {
                            foreach(Koncert ko in Database.koncerti)
                            {
                                if(ko.Id.Equals(r.Id))
                                {
                                    if(r.Id.Equals(r.Id))
                                    {
                                        k.Stanje -= r.KolicinaKarata * ko.CenaKarte;
                                        try
                                        {
                                            Audit.ChangeSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "stanje");
                                        }
                                        catch(Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Audit.ChangeSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "stanje");
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }

                                    for(int i=0; i<k.Rezervacije.Count(); i++)
                                    {
                                        if(k.Rezervacije[i].Id.Equals(r.Id))
                                        {
                                            k.Rezervacije[i].Stanje = StanjeRezervacije.PLACENA;
                                        }

                                        try
                                        {
                                            Audit.PaySuccess("bb", StanjeRezervacije.POTREBNO_PLATITI.ToString(), StanjeRezervacije.PLACENA.ToString());
                                        }
                                        catch(Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        Database.WriteRezervacije();
                                        Database.WriteKorisnici();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed("bb", "Plati rezervaciju", "Rezervacija je placena od grupe Korisnik ili grupe Dobri.");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
