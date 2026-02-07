using SAOResoForm.Models;
using System;

namespace SAOResoForm.Service.Repository.tool
{
    public interface ITool
    {
        void CreaCartella(Personale item);
        string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale);
    }
}