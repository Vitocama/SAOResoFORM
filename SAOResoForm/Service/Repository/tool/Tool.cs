using SAOResoForm.Models;

       using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace SAOResoForm.Service.Repository.tool
{
    public class Tool : ITool
    {

public void CreaCartella(Personale item)
    {
        try
        {
            // Percorso base
            string percorsoBase = @"C:\SAO";

            // Verifica se la cartella base esiste, altrimenti creala
            if (!Directory.Exists(percorsoBase))
            {
                Directory.CreateDirectory(percorsoBase);
            }

            // Crea il nome della cartella: "Cognome Nome Matricola"
            string nomeCartella = $"{item.Cognome}_{item.Nome}_{item.Matricola}";

            // Rimuovi caratteri non validi dal nome della cartella
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                nomeCartella = nomeCartella.Replace(c.ToString(), "");
            }

            // Percorso completo della cartella
            string percorsoCompleto = Path.Combine(percorsoBase, nomeCartella);

            // Verifica se la cartella esiste già
            if (Directory.Exists(percorsoCompleto))
            {
                MessageBox.Show($"La cartella esiste già:\n{percorsoCompleto}",
                    "Informazione",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            // Crea la cartella
            Directory.CreateDirectory(percorsoCompleto);

            MessageBox.Show($"Cartella creata con successo:\n{percorsoCompleto}",
                "Successo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("Permessi insufficienti per creare la cartella in C:\\SAO.\n" +
                            "Esegui l'applicazione come Amministratore.",
                "Errore Permessi",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Errore durante la creazione della cartella:\n{ex.Message}",
                "Errore",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
}
