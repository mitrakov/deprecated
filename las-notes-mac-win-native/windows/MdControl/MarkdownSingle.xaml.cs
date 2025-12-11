using System.Windows.Controls;

namespace MdControl;

public partial class MarkdownSingle : UserControl {
    public MarkdownSingle() => InitializeComponent();

    public string Markdown {
        set {
            if (DataContext is Data data)
                data.Markdown = value;
        }
    }
}
