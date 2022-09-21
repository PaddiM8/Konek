using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Konek.Desktop.Views;

public partial class RoutineView : UserControl
{
    public RoutineView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}