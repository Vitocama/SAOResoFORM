using System;
using System.Security.Cryptography;
using System.Text;




namespace SAOResoForm.Helpers
{
    /// <summary>
    /// Cifra e decifra stringhe sensibili usando DPAPI (Windows Data Protection API).
    /// DataProtectionScope.LocalMachine = i dati cifrati funzionano SOLO su questo PC.
    /// </summary>
    public static class Credenziali
    {
        // ── Cifra ────────────────────────────────────────────────────────────────
        /// <summary>
        /// Cifra una stringa in chiaro e restituisce la versione Base64 cifrata.
        /// Usare UNA VOLTA SOLA per generare le costanti da incollare nel Service.
        /// </summary>
        public static string Cifra(string testoChinaro)
        {
            if (string.IsNullOrEmpty(testoChinaro))
                throw new ArgumentNullException(nameof(testoChinaro));

            byte[] dati = Encoding.UTF8.GetBytes(testoChinaro);
            byte[] cifrati = ProtectedData.Protect(dati, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(cifrati);
        }

        // ── Decifra ──────────────────────────────────────────────────────────────
        /// <summary>
        /// Decifra una stringa Base64 cifrata e restituisce il testo originale.
        /// </summary>
        public static string Decifra(string testoCifrato)
        {
            if (string.IsNullOrEmpty(testoCifrato))
                throw new ArgumentNullException(nameof(testoCifrato));

            byte[] cifrati = Convert.FromBase64String(testoCifrato);
            byte[] dati = ProtectedData.Unprotect(cifrati, null, DataProtectionScope.LocalMachine);
            return Encoding.UTF8.GetString(dati);
        }

        // ── Genera costanti (usa una volta sola) ─────────────────────────────────
        /// <summary>
        /// Utility per generare le stringhe cifrate da incollare in CartellaRiservataService.
        /// Chiamare una sola volta dal pannello admin, poi rimuovere la chiamata.
        /// Esempio: GeneraEStampa(@"SERVER\utenteApp", "PasswordSegreta");
        /// </summary>
        public static (string UtenteCifrato, string PasswordCifrata) GeneraCredenziali(
            string utente, string password)
        {
            return (Cifra(utente), Cifra(password));
        }
    }
}