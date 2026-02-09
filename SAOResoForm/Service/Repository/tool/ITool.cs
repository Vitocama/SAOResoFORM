using SAOResoForm.Models;
using System;

public interface ITool
{
    void CreaCartella(Personale item);
    string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null);
}