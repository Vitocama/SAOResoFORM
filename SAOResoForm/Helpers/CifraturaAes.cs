using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SAOResoForm.Helpers
{
    /// <summary>
    /// Cifra e decifra file con AES-256-CBC.
    /// Struttura file cifrato: [salt 16 byte][iv 16 byte][dati cifrati]
    /// </summary>
    public static class CifraturaAes
    {
        private const int ITERAZIONI = 100000;
        private const int KEY_SIZE = 32; // 256 bit
        private const int SALT_SIZE = 16;
        private const int IV_SIZE = 16;

        // ── Cifra file ────────────────────────────────────────────────────────────
        public static void CifraFile(string percorsoInput, string percorsoOutput, string password)
        {
            byte[] dati = File.ReadAllBytes(percorsoInput);
            byte[] cifrati = CifraBytes(dati, password);
            File.WriteAllBytes(percorsoOutput, cifrati);
        }

        // ── Decifra file ──────────────────────────────────────────────────────────
        public static byte[] DecifraFile(string percorso, string password)
        {
            byte[] cifrati = File.ReadAllBytes(percorso);
            return DecifraBytes(cifrati, password);
        }

        // ── Decifra file in testo ─────────────────────────────────────────────────
        public static string DecifraFileTesto(string percorso, string password)
        {
            byte[] dati = DecifraFile(percorso, password);
            return Encoding.UTF8.GetString(dati);
        }

        // ── Cifra testo e salva ───────────────────────────────────────────────────
        public static void CifraFileTesto(string percorso, string testo, string password)
        {
            byte[] dati = Encoding.UTF8.GetBytes(testo);
            byte[] cifrati = CifraBytes(dati, password);
            File.WriteAllBytes(percorso, cifrati);
        }

        // ── Verifica password ─────────────────────────────────────────────────────
        /// <summary>
        /// Tenta di decifrare il file sentinella per verificare la password.
        /// </summary>
        public static bool VerificaPassword(string percorsoSentinella, string password)
        {
            try
            {
                string contenuto = DecifraFileTesto(percorsoSentinella, password);
                return contenuto == "OK";
            }
            catch
            {
                return false;
            }
        }

        // ── Crea file sentinella ──────────────────────────────────────────────────
        /// <summary>
        /// Crea un file sentinella cifrato usato per verificare la password.
        /// Chiamare una volta sola alla prima configurazione.
        /// </summary>
        public static void CreaSentinella(string percorsoSentinella, string password)
        {
            CifraFileTesto(percorsoSentinella, "OK", password);
        }

        // ── Cifra bytes ───────────────────────────────────────────────────────────
        public static byte[] CifraBytes(byte[] dati, string password)
        {
            byte[] salt = new byte[SALT_SIZE];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            byte[] key = DerivaChiave(password, salt);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.GenerateIV();

                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length);
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dati, 0, dati.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }

        // ── Decifra bytes ─────────────────────────────────────────────────────────
        public static byte[] DecifraBytes(byte[] cifrati, string password)
        {
            byte[] salt = new byte[SALT_SIZE];
            byte[] iv = new byte[IV_SIZE];

            Array.Copy(cifrati, 0, salt, 0, SALT_SIZE);
            Array.Copy(cifrati, SALT_SIZE, iv, 0, IV_SIZE);

            byte[] key = DerivaChiave(password, salt);
            int offset = SALT_SIZE + IV_SIZE;
            byte[] datiCifr = new byte[cifrati.Length - offset];
            Array.Copy(cifrati, offset, datiCifr, 0, datiCifr.Length);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(datiCifr, 0, datiCifr.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        // ── Deriva chiave da password ─────────────────────────────────────────────
        private static byte[] DerivaChiave(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERAZIONI))
                return pbkdf2.GetBytes(KEY_SIZE);
        }
    }
}