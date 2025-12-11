namespace LasNotes;

public partial class SplashScreen : UserControl {
    public SplashScreen() => InitializeComponent();

    public void AddCards(IEnumerable<CardWidget> cards) => listbox.AddCards(cards);

    public void AddHandlers(EventHandler onNewClick, EventHandler onOpenClick) {
        buttonNew.Click += onNewClick;
        buttonOpen.Click += onOpenClick;
    }
}
