using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MdXaml;

namespace MdControl;

public record class ContextMenu(string Markdown, bool IsArchived, string[] Tags, Action OnEdit, Action OnArchive, Action OnRestore, Action OnDelete);

public partial class MarkdownMulti : UserControl {
    private readonly CollectionHash<ContextMenu> hash = new(); // GetHashCode() for record class is synthesized by the Compiler

    public MarkdownMulti() => InitializeComponent();
    private static readonly ComponentResourceManager resources = new (typeof(MarkdownMulti));

    public void SetMarkdown(IEnumerable<ContextMenu> markdowns) {
        if (!hash.NeedUpdate(markdowns)) return;

        MainStackPanel.Children.Clear();
        foreach (var md in markdowns) {
            // markdown viewer
            var viewer = new MarkdownScrollViewer {
                Markdown = md.Markdown,
                MarkdownStyle = MarkdownStyle.GithubLike,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                ClickAction = ClickAction.SafetyDisplayWithRelativePath,
                Opacity = md.IsArchived ? 0.6 : 1,
            };
            viewer.PreviewMouseWheel += MarkdownScrollViewer_PreviewMouseWheel;

            // tags/buttons panel
            var i = 0;
            var tagsAndButtons = new Grid { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top };
            Enumerable.Range(0, md.Tags.Length + (md.IsArchived ? 1 : 3)).ToList() // all tags + 1 or 3 buttons
                .ForEach(t => tagsAndButtons.ColumnDefinitions.Add(new()));

            md.Tags.ToList().ForEach(tag => tagsAndButtons.Children.Add(MakeTag(i++, tag, md.IsArchived)));

            if (md.IsArchived)
                tagsAndButtons.Children.Add(MakeButton(i++, "#90ee90", Brushes.LimeGreen,  "restore.png", "restore-note", md.OnRestore));
            else {
                tagsAndButtons.Children.Add(MakeButton(i++, "#8eb5f7", Brushes.DodgerBlue, "edit.png",    "edit-note", md.OnEdit));
                tagsAndButtons.Children.Add(MakeButton(i++, "#fcc18a", Brushes.DarkOrange, "archive.png", "archive-note", md.OnArchive));
                tagsAndButtons.Children.Add(MakeButton(i++, "#ff655a", Brushes.Red,        "delete.png",  "delete-note", md.OnDelete));
            }

            // main grid
            var mainGrid = new Grid();
            mainGrid.Children.Add(viewer);
            mainGrid.Children.Add(tagsAndButtons);
            MainStackPanel.Children.Add(mainGrid);
        }
    }

    // https://stackoverflow.com/a/16110178/2212849
    private void MarkdownScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
        if (sender is MarkdownScrollViewer sv) {
            var parent = sv.Parent as UIElement;
            parent?.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) { RoutedEvent = MouseWheelEvent });
        }
    }

    private static UIElement MakeTag(int columnIdx, string text, bool transparent) {
        var border = new Border {
            VerticalAlignment = VerticalAlignment.Center,
            BorderBrush = new LinearGradientBrush(new([new(Colors.Purple, 0), new(Colors.Blue, 0.5), new(Colors.Purple, 1)]), 45),
            Child = new TextBlock { Text = $"🏷️  {text}" },
            CornerRadius = new(10),
            BorderThickness = new(0.7),
            Margin = new(4),
            Padding = new(4, 1, 4, 2),
            Opacity = transparent ? 0.6 : 1,
        };
        border.SetValue(Grid.ColumnProperty, columnIdx);

        return border;
    }

    private static UIElement MakeButton(int columnIdx, string hexColour, Brush hoverColour, string imageName, string hintKey, Action onClick) {
        var normColour = new BrushConverter().ConvertFromString(hexColour) as Brush;
        var border = new Border {
            CornerRadius = new(16),
            Background = normColour,
            Margin = new(4),
            Padding = new(6),
            ToolTip = resources.GetString(hintKey),
            Child = new Image {
                Source = new BitmapImage(new Uri($"/MdControl;component/images/{imageName}", UriKind.Relative)),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 18,
                Width = 18,
            },
        };
        border.SetValue(Grid.ColumnProperty, columnIdx);
        border.MouseDown += (s, e) => onClick();
        border.MouseEnter += (s, e) => border.Background = hoverColour;
        border.MouseLeave += (s, e) => border.Background = normColour;

        return border;
    }
}
