using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Konek.Client;
using Konek.Desktop.ViewModels;
using ReactiveUI;

namespace Konek.Desktop.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.ShowAddLampDialog.RegisterHandler(DoShowAddLampDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShowAddRoutineDefinitionDialog.RegisterHandler(DoShowAddRoutineDefinitionDialogAsync)));
    }

    private async Task DoShowAddLampDialogAsync(InteractionContext<AddLampViewModel, Lamp?> interaction)
    {
        var dialog = new Dialogs.AddLampDialog
        {
            DataContext = interaction.Input,
        };

        var result = await dialog.ShowDialog<Lamp?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowAddRoutineDefinitionDialogAsync(InteractionContext<AddRoutineDefinitionViewModel, RoutineDefinition?> interaction)
    {
        var dialog = new Dialogs.AddRoutineDefinitionDialog
        {
            DataContext = interaction.Input,
        };

        var result = await dialog.ShowDialog<RoutineDefinition?>(this);
        interaction.SetOutput(result);
    }
}