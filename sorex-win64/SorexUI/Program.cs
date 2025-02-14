namespace SorexUI;

// build:
// 1) raise Version, AssemblyVersion and FileVersion in SorexUI.csproj
// 2) run: dotnet publish --self-contained --output temp
// 3) compile installer.iss with InnoSetup (=> check Output folder)
internal static class Program {
    [STAThread]
    internal static void Main() {
        ApplicationConfiguration.Initialize();
        Application.Run(new view.MainForm(new()));
    }
}
