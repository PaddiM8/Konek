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

    private readonly IGroupClient _groupClient;

    private readonly ILampClient _lampClient;

    private readonly IRoutineDefinitionClient _routineDefinitionClient;

    public MainWindowViewModel(
        LightControlViewModel lightControlViewModel,
        RoutineDefinitionViewModel routineDefinitionViewModel,
        IGroupClient groupClient,
        ILampClient lampClient,
        IRoutineDefinitionClient routineDefinitionClient,
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
            var lamp = await ShowAddLampDialog.Handle(
                ActivatorUtilities.CreateInstance<AddLampViewModel>(services)
            );

            if (lamp != null)
                Lamps.Add(lamp);
        });
        AddRoutineCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            try
            {
                var routineDefinition = await ShowAddRoutineDefinitionDialog.Handle(
                    ActivatorUtilities.CreateInstance<AddRoutineDefinitionViewModel>(services)
                );

                if (routineDefinition != null)
                    Routines.Add(routineDefinition);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });

        _groupClient = groupClient;
        _lampClient = lampClient;
        _routineDefinitionClient = routineDefinitionClient;

        RxApp.TaskpoolScheduler.Schedule(Load);
    }

    private async void Load()
    {
        try
        {
            Groups.AddRange(await _groupClient.GetAsync());
            Lamps.AddRange(await _lampClient.GetAsync());
            Routines.AddRange(await _routineDefinitionClient.GetAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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