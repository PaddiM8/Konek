using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Konek.Client;
using Microsoft.Extensions.DependencyInjection;
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

    public ReactiveCommand<Unit, RoutineDefinition?> SubmitCommand { get; }

    private readonly IRoutineDefinitionClient _routineDefinitionClient;

    public AddRoutineDefinitionViewModel(IRoutineDefinitionClient routineDefinitionClient, IServiceProvider services)
    {
        SubmitCommand = ReactiveCommand.CreateFromTask(Submit);
        _routineDefinitionClient = routineDefinitionClient;
    }

    private async Task<RoutineDefinition?> Submit()
    {
        return await _routineDefinitionClient.AddAsync(Name, Array.Empty<Effect>());
    }
}