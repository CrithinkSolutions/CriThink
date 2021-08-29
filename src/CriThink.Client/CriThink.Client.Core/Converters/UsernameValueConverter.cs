using System;
using MvvmCross.Converters;

namespace CriThink.Client.Core.Converters
{
    public class UsernameValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return $"Hello {value}";
        }
    }
}