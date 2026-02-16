using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SAOResoForm.Converter
{
    /// <summary>
    /// Converte la data di scadenza in un colore di sfondo:
    /// - Verde se data assente/vuota
    /// - Rosso se scaduto
    /// - Giallo se mancano meno di 6 mesi
    /// - Azzurro se oltre 6 mesi
    /// </summary>
    public class DataScadenzaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ✅ Data assente o vuota → Verde chiaro
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new SolidColorBrush(Color.FromRgb(144, 238, 144)); // Verde chiaro
            }

            string dataScadenzaStr = value.ToString();

            // ✅ Se la data è "01-01-3000" → Verde (validità infinita)
            if (dataScadenzaStr == "01-01-3000" || dataScadenzaStr == "01/01/3000")
            {
                return new SolidColorBrush(Color.FromRgb(144, 238, 144)); // Verde chiaro
            }

            // Prova a parsare la data
            if (!DateTime.TryParse(dataScadenzaStr, out DateTime dataScadenza))
            {
                // Se non riesce a parsare, prova il formato dd-MM-yyyy
                if (!DateTime.TryParseExact(dataScadenzaStr, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dataScadenza))
                {
                    // ✅ Parsing fallito → Verde chiaro
                    return new SolidColorBrush(Color.FromRgb(144, 238, 144));
                }
            }

            DateTime oggi = DateTime.Now.Date;
            DateTime seiMesiDaOggi = oggi.AddMonths(6);

            // 🔴 Scaduto (ROSSO)
            if (dataScadenza < oggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 69, 69)); // Rosso
            }

            // 🟡 In scadenza entro 6 mesi (GIALLO)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 255, 153)); // Giallo chiaro
            }

            // 🔵 Oltre 6 mesi (AZZURRO)
            return new SolidColorBrush(Color.FromRgb(108, 157, 201));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converte la data di scadenza in un messaggio testuale per la colonna Note
    /// </summary>
    public class DataScadenzaToNoteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ✅ Data assente o vuota
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "Nessuna scadenza";
            }

            string dataScadenzaStr = value.ToString();

            // ✅ Se la data è "01-01-3000" → Validità infinita (ValiditaAnni = 0)
            if (dataScadenzaStr == "01-01-3000" || dataScadenzaStr == "01/01/3000")
            {
                return "MAI";
            }

            // Prova a parsare la data
            if (!DateTime.TryParse(dataScadenzaStr, out DateTime dataScadenza))
            {
                // Se non riesce a parsare, prova il formato dd-MM-yyyy
                if (!DateTime.TryParseExact(dataScadenzaStr, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dataScadenza))
                {
                    return "Data non valida";
                }
            }

            DateTime oggi = DateTime.Now.Date;
            TimeSpan differenza = dataScadenza - oggi;
            int giorniRimanenti = (int)differenza.TotalDays;

            // 🔴 Scaduto
            if (dataScadenza < oggi)
            {
                int giorniScaduti = Math.Abs(giorniRimanenti);
                if (giorniScaduti == 0)
                    return "Scade oggi";
                else if (giorniScaduti == 1)
                    return "Scaduto ieri";
                else if (giorniScaduti < 30)
                    return $"Scaduto {giorniScaduti} giorni fa";
                else if (giorniScaduti < 365)
                {
                    int mesiScaduti = giorniScaduti / 30;
                    return $"Scaduto {mesiScaduti} {(mesiScaduti == 1 ? "mese" : "mesi")} fa";
                }
                else
                {
                    int anniScaduti = giorniScaduti / 365;
                    return $"Scaduto {anniScaduti} {(anniScaduti == 1 ? "anno" : "anni")} fa";
                }
            }

            // 🟢 Valido
            if (giorniRimanenti == 0)
                return "Scade oggi";
            else if (giorniRimanenti == 1)
                return "Scade domani";
            else if (giorniRimanenti < 30)
                return $"Scade tra {giorniRimanenti} giorni";
            else if (giorniRimanenti < 180) // Meno di 6 mesi
            {
                int mesiRimanenti = giorniRimanenti / 30;
                return $"Scade tra {mesiRimanenti} {(mesiRimanenti == 1 ? "mese" : "mesi")}";
            }
            else if (giorniRimanenti < 365)
            {
                int mesiRimanenti = giorniRimanenti / 30;
                return $"Scade tra {mesiRimanenti} mesi";
            }
            else
            {
                int anniRimanenti = giorniRimanenti / 365;
                int mesiRimanenti = (giorniRimanenti % 365) / 30;

                if (mesiRimanenti > 0)
                    return $"Scade tra {anniRimanenti} {(anniRimanenti == 1 ? "anno" : "anni")} e {mesiRimanenti} {(mesiRimanenti == 1 ? "mese" : "mesi")}";
                else
                    return $"Scade tra {anniRimanenti} {(anniRimanenti == 1 ? "anno" : "anni")}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}