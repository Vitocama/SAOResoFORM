using System;

namespace SAOResoForm.Service.IdentityService
{
    public interface IIdentity
    {
        bool Autenticato(string utente, string password);
        bool CambiaPassword(string utente, string nuovaPassword);
    }
}