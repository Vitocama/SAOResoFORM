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
    /// - Arancione se mancano meno di 6 mesi
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

            // 🔴 Scaduto (rosso)
            if (dataScadenza < oggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 204, 204)); // Rosso chiaro
            }

            // 🟠 In scadenza entro 6 mesi (arancione)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 229, 204)); // Arancione chiaro
            }

            // 🔵 Oltre 6 mesi (azzurro)
            return new SolidColorBrush(Color.FromRgb(108, 157, 201)); // #6C9DC9
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}