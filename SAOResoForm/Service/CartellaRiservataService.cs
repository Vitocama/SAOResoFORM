using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace SAOResoForm.Services
{
    /// <summary>
    /// Accede alla cartella riservata tramite impersonation di utenteApp.
    /// La cartella è protetta da ACL — solo utenteApp può accedervi.
    ///
    /// SETUP (una volta sola):
    ///   1. Decommentare la riga GeneraPasswordCifrata() in VerificaConnessione
    ///   2. Avviare il programma — copiare il valore dalla MessageBox
    ///   3. Incollarlo in PASSWORD_CIFRATA
    ///   4. Ricommentare la riga
    /// </summary>
    public class CartellaRiservataService
    {
        // ── Configurazione ────────────────────────────────────────────────────────
        private const string PERCORSO = @"C:\Users\Hirad\Desktop\tbl";
        private const string DOMINIO = "Acer";
        private const string UTENTE = "utenteApp";
        private const string PASSWORD_CIFRATA = "INCOLLA_QUI_VALORE_GENERATO";

        // ── Win32 ─────────────────────────────────────────────────────────────────
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(
            string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider,
            out Microsoft.Win32.SafeHandles.SafeAccessTokenHandle phToken);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        // ── SETUP — genera password cifrata (una volta sola) ──────────────────────
        public static void GeneraPasswordCifrata(string passwordInChiaro)
        {
            byte[] dati = Encoding.UTF8.GetBytes(passwordInChiaro);
            byte[] cifrati = ProtectedData.Protect(dati, null, DataProtectionScope.LocalMachine);
            string valore = Convert.ToBase64String(cifrati);
            System.Windows.MessageBox.Show(
                $"PASSWORD_CIFRATA:\n{valore}",
                "Copia questo valore in PASSWORD_CIFRATA");
        }

        // ── Decifra password ──────────────────────────────────────────────────────
        private static string DecifraPassword()
        {
            byte[] cifrati = Convert.FromBase64String(PASSWORD_CIFRATA);
            byte[] dati = ProtectedData.Unprotect(cifrati, null, DataProtectionScope.LocalMachine);
            return Encoding.UTF8.GetString(dati);
        }

        // ── Impersonation ─────────────────────────────────────────────────────────
        private WindowsImpersonationContext Impersona()
        {
            string password = DecifraPassword();

            bool ok = LogonUser(UTENTE, DOMINIO, password,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                out var token);

            if (!ok)
                throw new Win32Exception(Marshal.GetLastWin32Error(),
                    "Impossibile autenticare utenteApp.");

            return new WindowsIdentity(token.DangerousGetHandle()).Impersonate();
        }

        // ── Verifica connessione ──────────────────────────────────────────────────
        public bool VerificaConnessione()
        {
            try
            {
                // ── SOLO LA PRIMA VOLTA: decommentare, avviare, copiare il valore ──
                // GeneraPasswordCifrata("Password123!");

                using (var ctx = Impersona())
                {
                    return Directory.Exists(PERCORSO);
                }
            }
            catch
            {
                return false;
            }
        }

        // ── Leggi file ────────────────────────────────────────────────────────────
        public byte[] LeggiFile(string nomeFile)
        {
            using (var ctx = Impersona())
            {
                string percorsoCompleto = Path.Combine(PERCORSO, nomeFile);
                if (!File.Exists(percorsoCompleto))
                    throw new FileNotFoundException($"File non trovato: {nomeFile}");
                return File.ReadAllBytes(percorsoCompleto);
            }
        }

        // ── Leggi testo ───────────────────────────────────────────────────────────
        public string LeggiTesto(string nomeFile)
        {
            using (var ctx = Impersona())
            {
                string percorsoCompleto = Path.Combine(PERCORSO, nomeFile);
                if (!File.Exists(percorsoCompleto))
                    throw new FileNotFoundException($"File non trovato: {nomeFile}");
                return File.ReadAllText(percorsoCompleto, System.Text.Encoding.UTF8);
            }
        }

        // ── Scrivi file ───────────────────────────────────────────────────────────
        public void ScriviFile(string nomeFile, byte[] contenuto)
        {
            if (contenuto == null || contenuto.Length == 0)
                throw new ArgumentNullException(nameof(contenuto));
            using (var ctx = Impersona())
            {
                File.WriteAllBytes(Path.Combine(PERCORSO, nomeFile), contenuto);
            }
        }

        // ── Scrivi testo ──────────────────────────────────────────────────────────
        public void ScriviTesto(string nomeFile, string contenuto)
        {
            using (var ctx = Impersona())
            {
                File.WriteAllText(Path.Combine(PERCORSO, nomeFile),
                    contenuto, System.Text.Encoding.UTF8);
            }
        }

        // ── Elimina file ──────────────────────────────────────────────────────────
        public void EliminaFile(string nomeFile)
        {
            using (var ctx = Impersona())
            {
                string percorsoCompleto = Path.Combine(PERCORSO, nomeFile);
                if (!File.Exists(percorsoCompleto))
                    throw new FileNotFoundException($"File non trovato: {nomeFile}");
                File.Delete(percorsoCompleto);
            }
        }

        // ── Lista file ────────────────────────────────────────────────────────────
        public IEnumerable<string> ListaFile(string pattern = "*.*")
        {
            using (var ctx = Impersona())
            {
                return Directory.GetFiles(PERCORSO, pattern);
            }
        }
    }
}