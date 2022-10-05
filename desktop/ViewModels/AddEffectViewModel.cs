using System;
using System.Reactive;
using System.Threading.Tasks;
using Konek.Client;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class AddEffectViewModel : ViewModelBase
{
    private string _startTime = "";

    public string StartTime
    {
        get => _startTime;
        set => this.RaiseAndSetIfChanged(ref _startTime, value);
    }

    private string _endTime = "";

    public string EndTime
    {
        get => _endTime;
        set => this.RaiseAndSetIfChanged(ref _endTime, value);
    }

    private int _brightness;

    public int Brightness
    {
        get => _brightness;
        set => this.RaiseAndSetIfChanged(ref _brightness, value);
    }

    private int _temperature;

    public int Temperature
    {
        get => _temperature;
        set => this.RaiseAndSetIfChanged(ref _temperature, value);
    }

    public ReactiveCommand<Unit, Effect?> SubmitCommand { get; }

    public AddEffectViewModel()
    {
        SubmitCommand = ReactiveCommand.Create(Submit);
    }

    private Effect? Submit()
    {
        var startTimeSplit = StartTime.Split(":");
        var endTimeSplit = EndTime.Split(":");

        return new Effect
        {
            StartTime = new TimeSpan(int.Parse(startTimeSplit[0]), int.Parse(startTimeSplit[1]), 0),
            EndTime = new TimeSpan(int.Parse(endTimeSplit[0]), int.Parse(endTimeSplit[1]), 0),
            Brightness = (byte)Brightness,
            Temperature = (byte)Temperature,
        };
    }
}