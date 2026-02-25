public interface IIdentity
{
    bool Autenticato(string utente, string password);
    bool CambiaPassword(string utente, string nuovaPassword);
    bool CreaUtente(string utente, string password);
    void logout();
}