using SAOResoForm.Models;
using System;

public interface ITool
{
    void CreaCartella(Personale item);

    // Metodo originale (genera random automaticamente)
    string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null);

    // Metodo overload (usa numero random fornito)
    string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione, int numeroRandom);
}