using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Konek.Client;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class RoutineViewModel : ViewModelBase
{
    public ObservableCollection<Effect> Effects { get; } = new();

    public Interaction<EffectViewModel, Effect?> ShowAddEffectDialog { get; } = new();

    public ICommand AddEffectCommand { get; }

    public ICommand UpdateEffectsCommand { get; }

    private readonly IServiceProvider _services;
    private readonly IRoutineDefinitionClient _routineDefinitionClient;
    private readonly RoutineDefinition _routineDefinition;

    public RoutineViewModel(
        IServiceProvider services,
        IRoutineDefinitionClient routineDefinitionClient,
        RoutineDefinition routineDefinition)
    {
        _services = services;
        _routineDefinitionClient = routineDefinitionClient;
        _routineDefinition = routineDefinition;

        AddEffectCommand = ReactiveCommand.CreateFromTask(AddEffect);
        UpdateEffectsCommand = ReactiveCommand.CreateFromTask(UpdateEffects);
        RxApp.MainThreadScheduler.Schedule(Load);
    }

    private async void Load()
    {
        var routineDefinition = await _routineDefinitionClient.GetAsync(_routineDefinition.RoutineDefinitionId);
        Effects.Clear();
        Effects.AddRange(routineDefinition.Effects);
    }

    private async Task AddEffect()
    {
        try
        {
            var effect = await ShowAddEffectDialog.Handle(
                ActivatorUtilities.CreateInstance<EffectViewModel>(_services)
            );

            if (effect != null)
                Effects.Add(effect);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task UpdateEffects()
    {
        _routineDefinition.Effects = Effects;
        await _routineDefinitionClient.UpdateAsync(_routineDefinition);
    }
}