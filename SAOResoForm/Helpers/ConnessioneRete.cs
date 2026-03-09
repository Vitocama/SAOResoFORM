using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SAOResoForm.Helpers
{
    /// <summary>
    /// Apre una connessione autenticata a una cartella di rete condivisa.
    /// Implementa IDisposable: usare sempre con "using" per chiudere la connessione.
    /// </summary>
    public class ConnessioneRete : IDisposable
    {
        // ── Win32 API ────────────────────────────────────────────────────────────
        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetAddConnection2(
            ref NETRESOURCE lpNetResource,
            string          lpPassword,
            string          lpUserName,
            int             dwFlags);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetCancelConnection2(
            string lpName,
            int    dwFlags,
            bool   fForce);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NETRESOURCE
        {
            public int    dwScope;
            public int    dwType;
            public int    dwDisplayType;
            public int    dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        private const int RESOURCETYPE_DISK = 1;

        // ── Stato ────────────────────────────────────────────────────────────────
        private readonly string _percorso;
        private          bool   _disposed;

        // ── Costruttore ──────────────────────────────────────────────────────────
        /// <summary>
        /// Connette al percorso di rete con le credenziali fornite.
        /// </summary>
        /// <param name="percorso">Es. \\SERVER\CartellaRiservata</param>
        /// <param name="utente">Es. SERVER\utenteApp  oppure  DOMINIO\utenteApp</param>
        /// <param name="password">Password in chiaro (decifrata da Credenziali)</param>
        /// <exception cref="Win32Exception">Se la connessione fallisce</exception>
        public ConnessioneRete(string percorso, string utente, string password)
        {
            _percorso = percorso;

            var risorsa = new NETRESOURCE
            {
                dwType       = RESOURCETYPE_DISK,
                lpRemoteName = percorso
            };

            int esito = WNetAddConnection2(ref risorsa, password, utente, 0);

            if (esito != 0)
                throw new Win32Exception(esito,
                    $"Impossibile connettersi a '{percorso}'. Codice errore: {esito}");
        }

        // ── IDisposable ──────────────────────────────────────────────────────────
        public void Dispose()
        {
            if (_disposed) return;
            WNetCancelConnection2(_percorso, 0, true);
            _disposed = true;
        }
    }
}
