using SAOResoForm.Dati;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SAOResoForm.Converter
{
    // =========================
    // REPARTO
    // =========================
    public class CodUfficioToRepartoConverter : IValueConverter
    {
        private static readonly Cod_UUOO cod_UUOO = new Cod_UUOO();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !int.TryParse(value.ToString(), out int codice))
                return string.Empty;

            string chiave = cod_UUOO.reparti.FirstOrDefault(x => x.Value == codice).Key;

            if (string.IsNullOrEmpty(chiave))
                return string.Empty;

            return chiave.Split('-')[0].Trim(); // prima parte = Reparto
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // =========================
    // SEZIONE
    // =========================
    public class CodUfficioToSezioneConverter : IValueConverter
    {
        private static readonly Cod_UUOO cod_UUOO = new Cod_UUOO();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !int.TryParse(value.ToString(), out int codice))
                return string.Empty;

            string chiave = cod_UUOO.reparti.FirstOrDefault(x => x.Value == codice).Key;

            if (string.IsNullOrEmpty(chiave))
                return string.Empty;

            var parti = chiave.Split('-').Select(p => p.Trim()).ToArray();

            return parti.Length > 1 ? parti[1] : string.Empty; // seconda parte = Sezione
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // =========================
    // NUCLEO
    // =========================
    public class CodUfficioToNucleoConverter : IValueConverter
    {
        private static readonly Cod_UUOO cod_UUOO = new Cod_UUOO();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !int.TryParse(value.ToString(), out int codice))
                return string.Empty;

            string chiave = cod_UUOO.reparti.FirstOrDefault(x => x.Value == codice).Key;

            if (string.IsNullOrEmpty(chiave))
                return string.Empty;

            var parti = chiave.Split('-').Select(p => p.Trim()).ToArray();

            return parti.Length > 2 ? parti[2] : string.Empty; // terza parte = Nucleo
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
