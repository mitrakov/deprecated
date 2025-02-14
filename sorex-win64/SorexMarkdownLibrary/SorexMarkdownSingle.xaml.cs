using System.Windows.Controls;

namespace SorexMarkdownLibrary;

public partial class SorexMarkdownSingle : UserControl {
    public SorexMarkdownSingle() => InitializeComponent();

    public string Markdown {
        set {
            if (DataContext is Data data)
                data.Markdown = value;
        }
    }
}
