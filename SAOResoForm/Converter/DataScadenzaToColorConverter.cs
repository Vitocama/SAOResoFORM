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
                return new SolidColorBrush(Color.FromRgb(255, 69, 69)); // Giallo
            }

            // 🟡 In scadenza entro 6 mesi (giallo)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 255, 153)); // Giallo chiaro
            }

            // 🔵 Oltre 6 mesi (azzurro)
            return new SolidColorBrush(Color.FromRgb(108, 157, 201));  
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}