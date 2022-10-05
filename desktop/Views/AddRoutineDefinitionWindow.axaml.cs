using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Konek.Client;
using Konek.Desktop.ViewModels;
using ReactiveUI;

namespace Konek.Desktop.Views;

public partial class AddRoutineDefinitionWindow : ReactiveWindow<AddRoutineDefinitionViewModel>
{
    public AddRoutineDefinitionWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.SubmitCommand.Subscribe(Close)));
        this.WhenActivated(d => d(ViewModel!.ShowAddEffectDialog.RegisterHandler(DoShowAddEffectDialogAsync)));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task DoShowAddEffectDialogAsync(InteractionContext<AddEffectViewModel, Effect?> interaction)
    {
        var dialog = new AddEffectWindow
        {
            DataContext = interaction.Input,
        };

        var result = await dialog.ShowDialog<Effect?>(this);
        interaction.SetOutput(result);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}