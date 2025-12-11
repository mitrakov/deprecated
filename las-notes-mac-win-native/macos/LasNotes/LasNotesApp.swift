import SwiftUI

let recentFilesKey = "RECENT_FILES"
let isSponsorKey = "IS_SPONSOR"
let showDigestKey = "SHOW_DIGEST"

// build:
// 1. Bump version and build numbers in LasNotes -> Targets -> General
// 2. Product -> Destination -> Any Mac (Apple Silicon, Intel)
// 3. Product -> Archive -> Distribute App -> Direct Distribution (wait for notary service to validate the App)
// 4. After 20-30 seconds, click Distribute App again and export to "_installer/App/"
// 5. Run _installer/build-dmg.sh
@main
internal struct lasNotesApp: App {
    @NSApplicationDelegateAdaptor(AppDelegate.self) private var appDelegate // remove standard MacOS menu items (https://stackoverflow.com/a/70553784/2212849)
    private let vm = MainViewModel()

    @State private var currentText = "" // ideally should be a part of MainView(), but we need access right here as well

    // extra variables for dynamically updating the Scene (wrong: "if vm.isSponsor()", correct: "if self.isSponsor")
    @State private var recentFiles = UserDefaults.standard.stringArray(forKey: recentFilesKey) ?? [] // don't use @AppStorage because it cannot decode arrays
    @State private var isSponsor   = UserDefaults.standard.bool(forKey: isSponsorKey)
    @State private var showDigest  = UserDefaults.standard.bool(forKey: showDigestKey)
    @State private var showSetPin  = false

    var body: some Scene {
        WindowGroup {
            MainView(currentText: $currentText)
                .environmentObject(vm)
                .onReceive(vm.$like) { isSponsor = $0 } // update donation menu
                .onReceive(vm.$currentPath) { path in
                    recentFiles = vm.getRecentFiles()   // update menu with a new Recent Files list
                    showSetPin = path != nil            // update setPinCode menu
                }
        }
        .addFullScreen()
        .onChange(of: isSponsor,  perform: {vm.isSponsor = $0})
        .onChange(of: showDigest, perform: {vm.showDigest = $0})
        .commands {
            CommandGroup(replacing: .systemServices) {} // rm "LasNotes  -> Services"
            CommandGroup(replacing: .appVisibility) {}  // rm "LasNotes  -> Hide, Hide Others, Show All"
            CommandGroup(replacing: .saveItem) {}       // rm "File   -> Close"
            CommandGroup(replacing: .sidebar) {}        // rm "View   -> Enter Full Screen"
            CommandGroup(replacing: .windowSize) {}     // rm "Window -> Minimize, Zoom"
            CommandGroup(replacing: .windowList) {}     // rm "Window -> Bring All to Front"
            CommandGroup(replacing: .appInfo) {
                Button("menu-about", action: onAboutClick).keyboardShortcut("\u{f704}", modifiers: []) // F1, modifiers must be []!
                #if DEBUG
                Button("", action: debug).keyboardShortcut("\u{f70f}", modifiers: []) // F12
                #endif
            }
            CommandGroup(replacing: .help) {
                Button("menu-donate") { NSWorkspace.shared.open(URL(string: "https://lasnotes.com/donate")!) }.keyboardShortcut("\u{f705}", modifiers: []) // F2
                if isSponsor {
                    Toggle("menu-show-digest", isOn: $showDigest)
                } else {
                    Button("menu-donate-done", action: checkDonate)
                }
            }
            CommandGroup(replacing: .newItem) {
                Menu("menu-open-recent") {
                    ForEach(Array(recentFiles.enumerated()), id: \.element) { i, path in
                        Button(path) {vm.openFile(path)}.hotkey(i)
                    }
                }
                Divider()
                Button("menu-new", action: vm.newFile).keyboardShortcut("n", modifiers: [.shift, .command])
                Button("menu-open", action: vm.openFile).keyboardShortcut("o")
                Button("menu-set-pin", action: setPin).disabled(!showSetPin)
                Divider()
                Button("menu-close", action: vm.closeFile).keyboardShortcut("w")
            }
        }
    }
    
    private func onAboutClick() {
        let name    = Bundle.main.infoDictionary?["CFBundleDisplayName"] as? String ?? ""
        let version = Bundle.main.infoDictionary?["CFBundleShortVersionString"] as? String ?? ""
        Utils.showAlert(title: "\(name) v\(version)", text: ~"msg-about")
    }

    private func checkDonate() {
        if let code = Utils.showInputBox(title: ~"menu-donate-txt", text: "", placeholder: ~"menu-donate-placeholder") {
            if code.trimmingCharacters(in: .whitespaces) == "I am a Las Notes sponsor" {
                showDigest = true // vm will be also updated
                isSponsor = true  // vm will be also updated
                Utils.showAlert(title: ~"success", text: ~"msg-donate-valid", .informational)
            } else {
                Utils.showAlert(title: ~"error", text: ~"msg-donate-invalid", .critical)
            }
        }
    }

    private func setPin() {
        guard vm.currentPath != nil else {return}

        let curPin = vm.pinCode // DB call here
        if let pin = Utils.showInputBox(title: ~"dlg-set-pin-hdr", text: ~"dlg-set-pin-txt", defaultStr: curPin, placeholder: "PIN") {
            if pin.isWhiteSpace() {
                vm.pinCode = nil
                Utils.showAlert(title: ~"done", text: ~"msg-pin-unset")
            } else if pin != curPin {
                if (!currentText.isWhiteSpace()) {
                    // DB file will be closed => let's ⌘+C user's text, if any
                    let cb = NSPasteboard.general
                    cb.declareTypes([.string], owner: nil)
                    cb.setString(currentText, forType: .string)
                }
                vm.pinCode = pin
                Utils.showAlert(title: ~"done", text: ~"msg-pin-set")
                vm.closeFile()
            }
        }
    }

    private func debug() {
        Utils.showAlert(title: "Las Notes", text: "Las Notes")
    }
}

extension Scene {
    func addFullScreen() -> some Scene {
        if #available(macOS 13, *) {
            return self.defaultSize(width: NSScreen.main?.frame.width ?? 1200, height: NSScreen.main?.frame.height ?? 800)
        } else {return self}
    }
}

extension View {
    @ViewBuilder
    func hotkey(_ index: Int) -> some View {
        if index < 9 { // only ⌘1, ⌘2, ⌘3, ⌘4, ⌘5, ⌘6, ⌘7, ⌘8 and ⌘9
            self.keyboardShortcut(KeyEquivalent(String(index+1).first!))
        } else {self}
    }
}

final private class AppDelegate: NSObject, NSApplicationDelegate {
    func applicationDidUpdate(_ notification: Notification) {
        // essentially this should be done in "applicationDidFinishLaunching", but once a user clicks "Show Digest", main menu will be re-generated
        // and we should have removed extra items again, so it's better removing them here in "applicationDidUpdate", with if-guard
        let menu = NSApplication.shared.mainMenu!
        if menu.items.count > 4 {
            menu.items.removeSubrange(3 ..< menu.items.count - 1) // remove: View, Windows (keep: File, Edit, Help)
        }
    }
}
