using ReactiveUI;

namespace Konek.Desktop.ViewModels;

public class LightControlViewModel : ViewModelBase
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
}