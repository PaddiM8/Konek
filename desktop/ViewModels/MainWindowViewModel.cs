using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using Konek.Client;
using Konek.Desktop.ViewModels.Entries;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public DashboardViewModel DashboardViewModel { get; } = new();

    private LightControlViewModel _lightControlViewModel = null!;

    public LightControlViewModel LightControlViewModel
    {
        get => _lightControlViewModel;
        set => this.RaiseAndSetIfChanged(ref _lightControlViewModel, value);
    }

    private RoutineViewModel _routineViewModel = null!;

    public RoutineViewModel RoutineViewModel
    {
        get => _routineViewModel;
        set => this.RaiseAndSetIfChanged(ref _routineViewModel, value);
    }

    public ObservableCollection<StaticPageEntry> StaticPages { get; } = new(new []
    {
        new StaticPageEntry("Dashboard", 0, IsSelected: true),
    });

    public ObservableCollection<GroupEntry> Groups { get; } = new();

    public ObservableCollection<LampEntry> Lamps { get; } = new();

    public ObservableCollection<RoutineDefinitionEntry> Routines { get; } = new();

    public Interaction<AddLampViewModel, Lamp?> ShowAddLampDialog { get; } = new();

    public Interaction<AddRoutineDefinitionViewModel, RoutineDefinition?> ShowAddRoutineDefinitionDialog { get; } = new();

    private int _selectedView;

    public int SelectedView
    {
        get => _selectedView;
        set => this.RaiseAndSetIfChanged(ref _selectedView, value);
    }

    public ICommand AddLampCommand { get; }

    public ICommand AddRoutineCommand { get; }

    private readonly IServiceProvider _services;

    private readonly IGroupClient _groupClient;

    private readonly ILampClient _lampClient;

    private readonly IRoutineDefinitionClient _routineDefinitionClient;

    public MainWindowViewModel(
        IGroupClient groupClient,
        ILampClient lampClient,
        IRoutineDefinitionClient routineDefinitionClient,
        IServiceProvider services
    )
    {
        AddLampCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var lamp = await ShowAddLampDialog.Handle(
                ActivatorUtilities.CreateInstance<AddLampViewModel>(services)
            );

            if (lamp != null)
                Lamps.Add(new LampEntry(lamp));
        });
        AddRoutineCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var routineDefinition = await ShowAddRoutineDefinitionDialog.Handle(
                ActivatorUtilities.CreateInstance<AddRoutineDefinitionViewModel>(services)
            );

            if (routineDefinition != null)
                Routines.Add(new RoutineDefinitionEntry(routineDefinition));
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
            Groups.AddRange((await _groupClient.GetAllAsync()).Select(x => new GroupEntry(x)));
            Lamps.AddRange((await _lampClient.GetAllAsync()).Select(x => new LampEntry(x)));
            Routines.AddRange((await _routineDefinitionClient.GetAllAsync())
                .Select(x => new RoutineDefinitionEntry(x)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void SelectStaticPage(StaticPageEntry staticPageEntry)
    {
        SelectedView = staticPageEntry.Index;
    }

    public void SelectGroup(Group group)
    {
        LightControlViewModel = ActivatorUtilities.CreateInstance<LightControlViewModel>(_services, group.GroupId, group.Name);
        SelectedView = StaticPages.Last().Index + 1;
    }

    public void SelectLamp(Lamp lamp)
    {
        LightControlViewModel = ActivatorUtilities.CreateInstance<LightControlViewModel>(_services, lamp.LampId, lamp.Name);
        SelectedView = StaticPages.Last().Index + 1;
    }

    public void SelectRoutineDefinition(RoutineDefinition routineDefinition)
    {
        RoutineViewModel = ActivatorUtilities.CreateInstance<RoutineViewModel>(_services, routineDefinition);
        SelectedView = StaticPages.Last().Index + 2;
    }
}