using System;
using System.Reactive;
using System.Threading.Tasks;
using Konek.Client;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class AddRoutineDefinitionViewModel : ViewModelBase
{
    private string _name = "";

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ReactiveCommand<Unit, RoutineDefinition> SubmitCommand { get; }

    private readonly IRoutineDefinitionClient _routineDefinitionClient;

    public AddRoutineDefinitionViewModel(IRoutineDefinitionClient routineDefinitionClient)
    {
        SubmitCommand = ReactiveCommand.CreateFromTask(Submit);
        _routineDefinitionClient = routineDefinitionClient;
    }

    private async Task<RoutineDefinition> Submit()
    {
        return await _routineDefinitionClient.AddAsync(Name, Array.Empty<Effect>());
    }
}