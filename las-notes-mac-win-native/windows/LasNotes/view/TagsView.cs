using MdControl;

namespace LasNotes;

public class TagsView : Panel {
    private TableLayoutPanel? p;
    private readonly CollectionHash<Button> hash = new(b => b.Text);

    public void AddButtons(IEnumerable<Button> buttons) {
        if (!hash.NeedUpdate(buttons)) return;

        if (p != null) {
            Controls.Remove(p);
            p.Dispose();
        }

        p = new() {
            AutoScroll = true,
            ColumnCount = 1,
            RowCount = buttons.Count() + 1,
            Dock = DockStyle.Fill,
            Padding = new(0, 0, SystemInformation.VerticalScrollBarWidth, 0), // https://stackoverflow.com/a/6555682/2212849
        };

        p.ColumnStyles.Add(new(SizeType.Percent, 100));
        foreach (var b in buttons) {
            b.AutoSize = true;
            b.Dock = DockStyle.Fill;
            b.TextAlign = ContentAlignment.MiddleLeft;
            p.Controls.Add(b);
        }

        Controls.Add(p);
    }
}
