using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SAOResoForm.Converter
{
    /// <summary>
    /// Converte la data di scadenza in un colore di sfondo:
    /// - Rosso se scaduto
    /// - Arancione se mancano meno di 6 mesi
    /// - Trasparente altrimenti
    /// </summary>
    public class DataScadenzaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return Brushes.Transparent;

            string dataScadenzaStr = value.ToString();

            // Prova a parsare la data
            if (!DateTime.TryParse(dataScadenzaStr, out DateTime dataScadenza))
            {
                // Se non riesce a parsare, prova il formato yyyy-MM-dd
                if (!DateTime.TryParseExact(dataScadenzaStr, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dataScadenza))
                {
                    return Brushes.Transparent;
                }
            }

            DateTime oggi = DateTime.Now.Date;
            DateTime seiMesiDaOggi = oggi.AddMonths(6);

            // Scaduto (rosso)
            if (dataScadenza < oggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 204, 204)); // Rosso chiaro
            }

            // In scadenza entro 6 mesi (arancione)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(255, 229, 204)); // Arancione chiaro
            }

            // OK (trasparente o bianco)
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}