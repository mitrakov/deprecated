namespace LasNotes;

public partial class CardWidget : UserControl {
    protected readonly EventHandler? onClickDelegeate;
    private bool mouseInside = false;
    private readonly Color highlightColour = Color.FromArgb(207, 227, 246);

    public CardWidget(string caption, string text, EventHandler onClick) {
        InitializeComponent();
        image.Image = Utils.BytesToImage(resources.GetObject("database") as byte[] ?? []);
        header.Text = caption;
        this.text.Text = text;
        onClickDelegeate = onClick;
        DoubleBuffered = true; // to prevent flicker
        WireMouseEvents(this);
    }

    public string Caption { get {return header.Text;}}

    public override string Text { get {return text.Text;}}

    private void WireMouseEvents(Control container) {
        // make all children act like their parent
        foreach (Control c in container.Controls) {
            c.Click += (s, e) => OnClick(e);
            c.MouseMove += (s, e) => OnMouseMove(e);
            c.MouseLeave += (s, e) => OnMouseLeave(e);

            WireMouseEvents(c);
        };
    }

    private void OnClick(object sender, EventArgs e) => onClickDelegeate?.Invoke(sender, e);

    private void OnMouseMove(object sender, MouseEventArgs e) {
        mouseInside = true;
        BackColor = highlightColour;
    }

    private void OnMouseLeave(object sender, EventArgs e) {
        mouseInside = false;
        BackColor = Color.AliceBlue;
    }

    private void OnPaint(object sender, PaintEventArgs e) {
        var colour = mouseInside ? Color.MediumBlue : Color.SlateBlue;
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
            colour, 2, ButtonBorderStyle.Solid,
            colour, 2, ButtonBorderStyle.Solid,
            colour, 2, ButtonBorderStyle.Solid,
            colour, 2, ButtonBorderStyle.Solid
        );
    }

    public override int GetHashCode() => header.Text.GetHashCode() + text.Text.GetHashCode();
}
