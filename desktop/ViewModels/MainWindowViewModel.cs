using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using Konek.Client;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public LightControlViewModel LightControlViewModel { get; }

    public RoutineDefinitionViewModel RoutineDefinitionViewModel { get; }

    public ObservableCollection<Group> Groups { get; } = new();

    public ObservableCollection<Lamp> Lamps { get; } = new();

    public ObservableCollection<RoutineDefinition> Routines { get; } = new();

    public Interaction<AddLampViewModel, Lamp?> ShowAddLampDialog { get; } = new();

    private int _selectedView;

    public int SelectedView
    {
        get => _selectedView;
        set => this.RaiseAndSetIfChanged(ref _selectedView, value);
    }

    public ICommand SelectGroupCommand { get; }

    public ICommand SelectLampCommand { get; }

    public ICommand SelectRoutineCommand { get; }

    public ICommand AddLampCommand { get; }

    private readonly ILampClient _lampClient;

    public MainWindowViewModel(
        LightControlViewModel lightControlViewModel,
        RoutineDefinitionViewModel routineDefinitionViewModel,
        ILampClient lampClient,
        IServiceProvider services
    )
    {
        LightControlViewModel = lightControlViewModel;
        RoutineDefinitionViewModel = routineDefinitionViewModel;
        SelectGroupCommand = ReactiveCommand.Create<Group>(SelectGroup);
        SelectLampCommand = ReactiveCommand.Create<Lamp>(SelectLamp);
        SelectRoutineCommand = ReactiveCommand.Create<Routine>(SelectRoutine);
        AddLampCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowAddLampDialog.Handle(
                ActivatorUtilities.CreateInstance<AddLampViewModel>(services)
            );
        });

        _lampClient = lampClient;

        RxApp.TaskpoolScheduler.Schedule(Load);
    }

    private async void Load()
    {
        Lamps.AddRange(await _lampClient.GetAsync());
    }

    public void SelectGroup(Group group)
    {
        LightControlViewModel.Name = group.Name;
        SelectedView = 0;
    }

    public void SelectLamp(Lamp lamp)
    {
        LightControlViewModel.Name = lamp.Name;
        SelectedView = 0;
    }

    public void SelectRoutine(Routine routine)
    {
        SelectedView = 1;
    }
}