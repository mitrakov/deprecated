using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MdXaml;

namespace SorexMarkdownLibrary;

public record class ContextMenu(string Markdown, bool IsArchived, string[] Tags, Action OnEdit, Action OnArchive, Action OnRestore, Action OnDelete);

public partial class SorexMarkdownMulti : UserControl {
    public SorexMarkdownMulti() => InitializeComponent();

    public void SetMarkdown(IEnumerable<ContextMenu> markdowns) {
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
            Enumerable.Range(0, md.Tags.Length + (md.IsArchived ? 1 : 3)).ToList().ForEach(t => tagsAndButtons.ColumnDefinitions.Add(new())); // all tags + 1 or 3 buttons

            md.Tags.ToList().ForEach(tag => tagsAndButtons.Children.Add(MakeTag(i++, tag, md.IsArchived)));

            if (md.IsArchived)
                tagsAndButtons.Children.Add(MakeButton(i++, "#90ee90", "restore.png", "Restore from archive", md.OnRestore));
            else {
                tagsAndButtons.Children.Add(MakeButton(i++, "#8eb5f7", "edit.png", "Edit note", md.OnEdit));
                tagsAndButtons.Children.Add(MakeButton(i++, "#fcc18a", "archive.png", "Archive note", md.OnArchive));
                tagsAndButtons.Children.Add(MakeButton(i++, "#ff655a", "delete.png", "Delete note", md.OnDelete));
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

    private static UIElement MakeButton(int columnIdx, string hexColour, string imageName, string hint, Action onClick) {
        var border = new Border {
            CornerRadius = new(16),
            Background = new BrushConverter().ConvertFromString(hexColour) as Brush,
            Margin = new(4),
            Padding = new(6),
            ToolTip = hint,
            Child = new Image {
                Source = new BitmapImage(new Uri($"/SorexMarkdownLibrary;component/images/{imageName}", UriKind.Relative)),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 18,
                Width = 18,
            },
        };
        border.SetValue(Grid.ColumnProperty, columnIdx);
        border.MouseDown += (s, e) => onClick();

        return border;
    }
}
