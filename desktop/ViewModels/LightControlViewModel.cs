using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using DynamicData;
using Konek.Client;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class LightControlViewModel : ViewModelBase
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ObservableCollection<Routine> Routines { get; } = new();

    private readonly int _id;

    private readonly ILampClient _lamps;

    public LightControlViewModel(ILampClient lamps, int id, string name)
    {
        _id = id;
        Name = name;
        _lamps = lamps;

        RxApp.TaskpoolScheduler.Schedule(Load);
    }

    private async void Load()
    {
        Routines.Clear();

        var lamp = await _lamps.GetAsync(_id);
        Name = lamp.Name;
        Routines.AddRange(lamp.Routines);
    }
}