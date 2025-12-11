using MdControl;

namespace LasNotes;

public class SplashListbox : Panel {
    private TableLayoutPanel? p;
    private readonly CollectionHash<CardWidget> hash = new(); // CardWidget.GetHashCode() is overriden

    public void AddCards(IEnumerable<CardWidget> cards) {
        if (!hash.NeedUpdate(cards)) return;

        if (p != null) {
            Controls.Remove(p);
            p.Dispose();
        }

        p = new() {
            AutoScroll = true,
            ColumnCount = 1,
            RowCount = cards.Count() + 1,
            Dock = DockStyle.Fill,
            Padding = new(0, 0, SystemInformation.VerticalScrollBarWidth, 0), // https://stackoverflow.com/a/6555682/2212849
        };

        p.ColumnStyles.Add(new(SizeType.Percent, 100));
        foreach (var b in cards) {
            b.AutoSize = true;
            b.Dock = DockStyle.Fill;
            p.Controls.Add(b);
        }

        Controls.Add(p);
    }
}
