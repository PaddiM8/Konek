using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Konek.Desktop.Models;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public LightControlViewModel LightControlViewModel { get; }

    public RoutineDefinitionViewModel RoutineDefinitionViewModel { get; }

    public ObservableCollection<Group> Groups { get; } = new() { new("Group #1"), new("Group #2") };

    public ObservableCollection<Lamp> Lamps { get; } = new() { new("Lamp #1", "1"), new("Lamp #2", "1") };

    public ObservableCollection<RoutineDefinition> Routines { get; } = new() { new(new List<Effect>()), new(new List<Effect>()) };

    private int _selectedView;

    public int SelectedView
    {
        get => _selectedView;
        set => this.RaiseAndSetIfChanged(ref _selectedView, value);
    }

    public ICommand SelectGroupCommand { get; }

    public ICommand SelectLampCommand { get; }

    public ICommand SelectRoutineCommand { get; }

    public MainWindowViewModel(LightControlViewModel lightControlViewModel, RoutineDefinitionViewModel routineDefinitionViewModel)
    {
        LightControlViewModel = lightControlViewModel;
        RoutineDefinitionViewModel = routineDefinitionViewModel;
        SelectGroupCommand = ReactiveCommand.Create<Group>(SelectGroup);
        SelectLampCommand = ReactiveCommand.Create<Lamp>(SelectLamp);
        SelectRoutineCommand = ReactiveCommand.Create<Routine>(SelectRoutine);
    }

    public void SelectGroup(Group group)
    {
        LightControlViewModel.Name = group.Name;
        SelectedView = 0;
    }

    public void SelectLamp(Lamp lamp)
    {
        SelectedView = 0;
    }

    public void SelectRoutine(Routine routine)
    {
        SelectedView = 1;
    }
}