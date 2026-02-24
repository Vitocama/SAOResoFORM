using Org.BouncyCastle.Crypto.Generators;
using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAOResoForm.Service.Repository.Credenziali
{
    public class CredenzialiService : ICredenziali
    {


        public bool VerificaCredenziali(string utente, string password)
        {
            var db = new tblContext();
            var account = db.AccountUtenti.FirstOrDefault(u => u.Utente == utente);
            if (account == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, account.Password);
        }

        public AccountUtenti GetUtente(string utente)
        {
            var db = new tblContext();

            return db.AccountUtenti
                .FirstOrDefault(u => u.Utente == utente);
        }
    }
}
