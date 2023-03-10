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