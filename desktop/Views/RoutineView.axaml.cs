using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Konek.Client;
using Konek.Desktop.ViewModels;
using ReactiveUI;

namespace Konek.Desktop.Views;

public partial class RoutineView : ReactiveUserControl<RoutineViewModel>
{
    public RoutineView()
    {
        InitializeComponent();
        this.WhenActivated(d => d(App.MainWindow.ViewModel!.RoutineViewModel!.ShowAddEffectDialog.RegisterHandler(DoShowAddEffectDialogAsync)));
    }

    private async Task DoShowAddEffectDialogAsync(InteractionContext<AddEffectViewModel, Effect?> interaction)
    {
        var dialog = new AddEffectWindow
        {
            DataContext = interaction.Input,
        };

        var result = await dialog.ShowDialog<Effect?>(App.MainWindow);
        interaction.SetOutput(result);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}