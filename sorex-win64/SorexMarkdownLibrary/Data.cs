using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SorexMarkdownLibrary;

internal class Data : INotifyPropertyChanged {
    private string _markdown = "";
    public string Markdown { // must be public to be visible in Xaml ("{Binding Markdown}")
        get { return _markdown; }
        set {
            if (_markdown != value) {
                _markdown = value;
                FirePropertyChanged();
            }
        }
    }

    protected void FirePropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler? PropertyChanged;
}
