using BCrypt.Net;
using SAOResoForm.Models;
using System.Linq;

namespace SAOResoForm.Service.IdentityService
{
    public class Identity : IIdentity
    {
        public bool Autenticato(string utente, string password)
        {
            using (var db = new tblContext())
            {
                var account = db.AccountUtenti.FirstOrDefault(a => a.Utente == utente);
                if (account == null)
                    return false;

                // Password in chiaro → verifica diretta e migra all'hash
                if (!account.Password.StartsWith("$2"))
                {
                    if (account.Password != password)
                        return false;

                    account.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    db.SaveChanges();
                    return true;
                }

                // Password già hashata → verifica BCrypt
                return BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
        }

        public bool CambiaPassword(string utente, string nuovaPassword)
        {
            using (var db = new tblContext())
            {
                var account = db.AccountUtenti.FirstOrDefault(a => a.Utente == utente);
                if (account == null)
                    return false;

                account.Password = BCrypt.Net.BCrypt.HashPassword(nuovaPassword);
                db.SaveChanges();
                return true;
            }
        }
    }
}