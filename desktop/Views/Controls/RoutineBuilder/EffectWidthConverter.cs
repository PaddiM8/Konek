using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Konek.Client;

namespace Konek.Desktop.Views.Controls.RoutineBuilder;

public class EffectWidthConverter : IValueConverter
{
    public static readonly EffectWidthConverter Instance = new();
    private static readonly double _secondsPerDay = new TimeSpan(1, 0, 0, 0).TotalSeconds;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Effect effect)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        var timeSpan = effect.EndTime - effect.StartTime;
        if (timeSpan.TotalSeconds < 0)
            return 100 * ((_secondsPerDay - effect.StartTime.TotalSeconds) / _secondsPerDay);
        if (timeSpan.TotalSeconds == 0)
            return 0;

        return 100 * (timeSpan.TotalSeconds / _secondsPerDay);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new NotSupportedException();
}