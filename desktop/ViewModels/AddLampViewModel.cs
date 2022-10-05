using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using Konek.Client;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class AddLampViewModel : ViewModelBase
{
    public ObservableCollection<RemoteLamp> DetectedLamps { get; } = new();

    private string _name = "";

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private int _selectedLampIndex;

    public int SelectedLampIndex
    {
        get => _selectedLampIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedLampIndex, value);
    }

    public ReactiveCommand<Unit, Lamp?> SubmitCommand { get; }

    private readonly IHubClient _hubClient;

    private readonly ILampClient _lampClient;

    public AddLampViewModel(IHubClient hubClient, ILampClient lampClient)
    {
        SubmitCommand = ReactiveCommand.CreateFromTask(Submit);
        _hubClient = hubClient;
        _lampClient = lampClient;
        RxApp.TaskpoolScheduler.Schedule(LoadDetectedLamps);
    }

    private async void LoadDetectedLamps()
    {
        foreach (var lamp in await _hubClient.GetAsync())
        {
            DetectedLamps.Add(lamp);
        }
    }

    private async Task<Lamp?> Submit()
    {
        return await _lampClient.AddAsync(Name, DetectedLamps[SelectedLampIndex].Id);
    }
}