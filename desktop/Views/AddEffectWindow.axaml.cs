using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Konek.Desktop.ViewModels;
using ReactiveUI;

namespace Konek.Desktop.Views;

public partial class AddEffectWindow : ReactiveWindow<AddEffectViewModel>
{
    public AddEffectWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.SubmitCommand.Subscribe(Close)));
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