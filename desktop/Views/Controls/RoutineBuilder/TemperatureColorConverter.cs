using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Konek.Desktop.Views.Controls.RoutineBuilder;

public class TemperatureColorConverter : IValueConverter
{
    public static readonly TemperatureColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not byte temperature)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return Color.FromRgb(temperature, 127, (byte)(255 - temperature));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new NotSupportedException();
}