using System.Collections.Generic;

namespace Konek.Desktop.Views.Dialogs;

public enum DialogResultStatus
{
    None,
    Submit,
    Delete,
}

public record DialogResult<T>(T? Value, DialogResultStatus Status);