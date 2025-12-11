namespace LasNotes;

internal static class Utils {
    internal static T IfNull<T>(this T? @this, Func<T> @default) {
        return @this != null ? @this! : @default();
    }

    internal static Image BytesToImage(byte[] blob) {
        using var s = new MemoryStream(blob);
        return new Bitmap(s, false); // bug in VS Form Designer. If you see an Exception here, check MainForm.resx file in Git
    }

    internal static Icon BytesToIcon(byte[] blob) {
        using var s = new MemoryStream(blob);
        return new Icon(s);
    }
}
