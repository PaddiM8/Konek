using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Konek.Client;

namespace Konek.Desktop.Views.Controls.RoutineBuilder;

public class EffectOffsetConverter : IValueConverter
{
    public static readonly EffectOffsetConverter Instance = new();
    private static readonly double _secondsPerDay = new TimeSpan(1, 0, 0, 0).TotalSeconds;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Effect effect)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return 100 * (effect.StartTime.TotalSeconds / _secondsPerDay);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new NotSupportedException();
}