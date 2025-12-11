namespace LasNotes;

// build:
// 1) raise Version, AssemblyVersion, FileVersion, InfoVersion in LasNotes.csproj
// 2) run in project root: dotnet publish --self-contained --output "Las Notes"
// 3) sign LasNotes.exe and LasNotes.dll with Rutoken:
//    signtool sign /v /a /tr http://timestamp.globalsign.com/tsa/r6advanced1 /td SHA256 /fd SHA256 '.\Las Notes.exe'
//    signtool sign /v /a /tr http://timestamp.globalsign.com/tsa/r6advanced1 /td SHA256 /fd SHA256 '.\Las Notes.dll'
//    signtool verify /v '.\Las Notes.exe'
//    signtool verify /v '.\Las Notes.dll'
// 4) update MyAppVersion in _installer/inno-setup-installer.iss
// 5) compile _installer/inno-setup-installer.aip with Inno-Setup 6.4.0
// 6) sign _installer/LasNotes-SetupFiles/lasnotes-win64-x.y.z.exe with Rutoken:
//    signtool sign /v /a /tr http://timestamp.globalsign.com/tsa/r6advanced1 /td SHA256 /fd SHA256 .\lasnotes-win64-1.0.0.exe
//    signtool verify /v .\lasnotes-win64-1.0.0.exe
internal static class Program {
    [STAThread]
    internal static void Main(string[] args) {
        // update Properties/launchSettings.json with "en", "es", "ru" to debug other locales
        if (args.Length == 1) Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(args[0]);
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(new()));
    }
}
