using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SAOResoForm.Converter
{
    /// <summary>
    /// Converte la data di scadenza in un colore di testo:
    /// - Rosso scuro se scaduto
    /// - Arancione scuro se in scadenza
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

            // Scaduto (rosso scuro)
            if (dataScadenza < oggi)
            {
                return new SolidColorBrush(Color.FromRgb(139, 0, 0)); // Rosso scuro
            }

            // In scadenza entro 6 mesi (arancione scuro)
            if (dataScadenza <= seiMesiDaOggi)
            {
                return new SolidColorBrush(Color.FromRgb(204, 102, 0)); // Arancione scuro
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}