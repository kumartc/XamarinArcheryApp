using System;
using Xamarin.Forms;

namespace XamarinArcheryApp.ValueConverters
{
  public class DateTimeToStringValueConverter : IValueConverter
  {
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return value is DateTime ? value.ToString() : value;
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      DateTime dateTime;
      return DateTime.TryParse((string)value, out dateTime) ? dateTime : value;
    }
  }
}