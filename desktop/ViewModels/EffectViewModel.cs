using System;
using System.Reactive;
using System.Threading.Tasks;
using Konek.Client;
using Konek.Desktop.Views.Dialogs;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class EffectViewModel : ViewModelBase
{
    public string Title { get; }

    public string SubmitText { get; }

    public bool HasDelete { get; }

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

    public ReactiveCommand<Unit, DialogResult<Effect>> SubmitCommand { get; }

    public ReactiveCommand<Unit, DialogResult<Effect>> DeleteCommand { get; }

    private readonly Effect _effect;

    public EffectViewModel(Effect? effect = null)
    {
        if (effect == null)
        {
            Title = "Add Effect";
            SubmitText = "Add";
            HasDelete = false;
        }
        else
        {
            Title = "Edit Effect";
            SubmitText = "Edit";
            HasDelete = true;
            StartTime = effect.StartTime.ToString();
            EndTime = effect.EndTime.ToString();
            Brightness = effect.Brightness;
            Temperature = effect.Temperature;
        }

        _effect = effect ?? new();

        SubmitCommand = ReactiveCommand.Create(Submit);
        DeleteCommand = ReactiveCommand.CreateFromTask(Delete);
    }

    private DialogResult<Effect> Submit()
    {
        var startTimeSplit = StartTime.Split(":");
        var endTimeSplit = EndTime.Split(":");

        _effect.StartTime = new TimeSpan(int.Parse(startTimeSplit[0]), int.Parse(startTimeSplit[1]), 0);
        _effect.EndTime = new TimeSpan(int.Parse(endTimeSplit[0]), int.Parse(endTimeSplit[1]), 0);
        _effect.Brightness = (byte)Brightness;
        _effect.Temperature = (byte)Temperature;

        return new DialogResult<Effect>(_effect, DialogResultStatus.Submit);
    }

    private async Task<DialogResult<Effect>> Delete()
    {
        var confirmationBox = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(
                "Confirmation",
                "Are you sure you want to delete the effect?",
                ButtonEnum.YesNo
            );
        var result = await confirmationBox.Show();

        var status = result == ButtonResult.Yes
            ? DialogResultStatus.Delete
            : DialogResultStatus.None;

        return new DialogResult<Effect>(null, status);
    }
}