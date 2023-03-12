using System;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Konek.Desktop.ViewModels;
using ReactiveUI;

namespace Konek.Desktop.Views.Dialogs;

public partial class EffectDialog : ReactiveWindow<EffectViewModel>
{
    public EffectDialog()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.SubmitCommand.Subscribe(Close)));
        this.WhenActivated(d => d(ViewModel!.DeleteCommand.Subscribe(x => {
            if (x.Status != DialogResultStatus.None)
                Close(x);
        })));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}