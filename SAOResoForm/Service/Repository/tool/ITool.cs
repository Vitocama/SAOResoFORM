using SAOResoForm.Models;
using System;
using System.Collections.Generic;

public interface ITool
{
    void CreaCartella(Personale item);
    string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null);
    string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione, int numeroRandom);
    void ApriDocumento(Attestati attestato);
    void EsportaInCsv(IEnumerable<Attestati> dati);
}