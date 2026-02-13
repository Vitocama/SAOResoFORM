using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SAOResoForm.Converter
{
    /// <summary>
    /// Converte la data di scadenza in un colore di testo:
    /// - Rosso scuro se scaduto
    /// - Marrone scuro se in scadenza (per sfondo giallo)
    /// - Nero altrimenti
    /// </summary>
    public class DataScadenzaToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return Brushes.Black;

            string dataScadenzaStr = value.ToString();

            if (!DateTime.TryParse(dataScadenzaStr, out DateTime dataScadenza))
            {
                if (!DateTime.TryParseExact(dataScadenzaStr, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dataScadenza))
                {
                    return Brushes.Black;
                }
            }

            DateTime oggi = DateTime.Now.Date;
            DateTime seiMesiDaOggi = oggi.AddMonths(6);

            // 🔴 Scaduto (nero)
            if (dataScadenza < oggi)
            {
                return new SolidColorBrush(Color.FromRgb(0, 0, 0)); // Rosso scuro
            }

            // 🟤 In scadenza entro 6 mesi (marrone scuro - leggibile su giallo)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(153, 76, 0)); // Marrone scuro
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}