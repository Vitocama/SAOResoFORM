using BCrypt.Net;
using SAOResoForm.LoginControl;
using SAOResoForm.Models;
using System;
using System.Linq;
using System.Windows;

namespace SAOResoForm.Service.IdentityService
{
    public class Identity : IIdentity
    {
        private const int WorkFactor = 11;

        public bool Autenticato(string utente, string password)
        {
            if (string.IsNullOrWhiteSpace(utente) || string.IsNullOrWhiteSpace(password))
                return false;

            using (var db = new tblContext())
            {
                var account = db.AccountUtenti
                    .FirstOrDefault(a => a.Utente == utente);

                if (account == null)
                    return false;

                if (string.IsNullOrEmpty(account.Password))
                    return false;

                bool passwordCorretta = false;

                if (IsBCryptHash(account.Password))
                {
                    passwordCorretta = BCrypt.Net.BCrypt.Verify(password, account.Password);
                }
                else
                {
                    if (account.Password == password)
                    {
                        passwordCorretta = true;

                        account.Password = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
                        db.SaveChanges();
                    }
                }
                SessionManager.CurrentUser = utente;
                SessionManager.Ruolo = account.Ruolo; // campo della tabella AccountUtenti

                return passwordCorretta;
            }
        }

        public bool CambiaPassword(string utente, string nuovaPassword)
        {
            if (string.IsNullOrWhiteSpace(utente) || string.IsNullOrWhiteSpace(nuovaPassword))
                return false;

            using (var db = new tblContext())
            {
                var account = db.AccountUtenti
                    .FirstOrDefault(a => a.Utente == utente);

                if (account == null)
                    return false;

                account.Password = BCrypt.Net.BCrypt.HashPassword(nuovaPassword, WorkFactor);
                db.SaveChanges();

                return true;
            }
        }

        public bool CreaUtente(string utente, string password)
        {
            if (string.IsNullOrWhiteSpace(utente) || string.IsNullOrWhiteSpace(password))
                return false;

            using (var db = new tblContext())
            {
                if (db.AccountUtenti.Any(a => a.Utente == utente))
                    return false;

                var nuovoUtente = new AccountUtenti
                {
                    Utente = utente,
                    Password = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor)
                };

                db.AccountUtenti.Add(nuovoUtente);
                db.SaveChanges();

                return true;
            }
        }

        private bool IsBCryptHash(string value)
        {
            return value.StartsWith("$2a$") ||
                   value.StartsWith("$2b$") ||
                   value.StartsWith("$2y$");
        }

        public void logout()
        {
            SessionManager.CurrentUser = null;

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public static class SessionManager
        {
            public static string CurrentUser { get; set; }
            public static string Ruolo { get; set; }
        }

    }
}