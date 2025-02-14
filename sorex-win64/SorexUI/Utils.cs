namespace SorexUI;

internal static class Extensions {
    internal static T IfNull<T>(this T? @this, Func<T> @default) {
        return @this != null ? @this! : @default();
    }

    internal static Image BytesToImage(byte[] blob) {
        using var s = new MemoryStream(blob);
        return new Bitmap(s, false);
    }
}
