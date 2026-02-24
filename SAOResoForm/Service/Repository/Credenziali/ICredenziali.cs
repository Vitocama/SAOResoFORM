using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOResoForm.Service
{
    public interface ICredenziali
    {
        bool VerificaCredenziali(string utente, string password);
        AccountUtenti GetUtente(string utente);
    }
}
