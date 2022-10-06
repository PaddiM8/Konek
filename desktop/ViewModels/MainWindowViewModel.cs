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
    private LightControlViewModel _lightControlViewModel = null!;

    public LightControlViewModel LightControlViewModel
    {
        get => _lightControlViewModel;
        set => this.RaiseAndSetIfChanged(ref _lightControlViewModel, value);
    }

    public RoutineDefinitionViewModel RoutineDefinitionViewModel { get; }

    public ObservableCollection<Group> Groups { get; } = new();

    public ObservableCollection<Lamp> Lamps { get; } = new();

    public ObservableCollection<RoutineDefinition> Routines { get; } = new();

    public Interaction<AddLampViewModel, Lamp?> ShowAddLampDialog { get; } = new();

    public Interaction<AddRoutineDefinitionViewModel, RoutineDefinition?> ShowAddRoutineDefinitionDialog { get; } = new();

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

    public ICommand AddRoutineCommand { get; }

    private readonly IServiceProvider _services;

    private readonly IGroupClient _groupClient;

    private readonly ILampClient _lampClient;

    private readonly IRoutineDefinitionClient _routineDefinitionClient;

    public MainWindowViewModel(
        RoutineDefinitionViewModel routineDefinitionViewModel,
        IGroupClient groupClient,
        ILampClient lampClient,
        IRoutineDefinitionClient routineDefinitionClient,
        IServiceProvider services
    )
    {
        RoutineDefinitionViewModel = routineDefinitionViewModel;
        SelectGroupCommand = ReactiveCommand.Create<Group>(SelectGroup);
        SelectLampCommand = ReactiveCommand.Create<Lamp>(SelectLamp);
        SelectRoutineCommand = ReactiveCommand.Create<Routine>(SelectRoutine);
        AddLampCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var lamp = await ShowAddLampDialog.Handle(
                ActivatorUtilities.CreateInstance<AddLampViewModel>(services)
            );

            if (lamp != null)
                Lamps.Add(lamp);
        });
        AddRoutineCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var routineDefinition = await ShowAddRoutineDefinitionDialog.Handle(
                ActivatorUtilities.CreateInstance<AddRoutineDefinitionViewModel>(services)
            );

            if (routineDefinition != null)
                Routines.Add(routineDefinition);
        });

        _services = services;
        _groupClient = groupClient;
        _lampClient = lampClient;
        _routineDefinitionClient = routineDefinitionClient;

        RxApp.TaskpoolScheduler.Schedule(Load);
    }

    private async void Load()
    {
        try
        {
            Groups.AddRange(await _groupClient.GetAllAsync());
            Lamps.AddRange(await _lampClient.GetAllAsync());
            Routines.AddRange(await _routineDefinitionClient.GetAllAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void SelectGroup(Group group)
    {
        LightControlViewModel = ActivatorUtilities.CreateInstance<LightControlViewModel>(_services, group.GroupId, group.Name);
        SelectedView = 0;
    }

    public void SelectLamp(Lamp lamp)
    {
        LightControlViewModel = ActivatorUtilities.CreateInstance<LightControlViewModel>(_services, lamp.LampId, lamp.Name);
        SelectedView = 0;
    }

    public void SelectRoutine(Routine routine)
    {
        SelectedView = 1;
    }
}