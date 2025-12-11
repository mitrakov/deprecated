import AppKit
import UniformTypeIdentifiers
import SwiftUI

open class Utils {
    static func showAlert(title: String, text: String, _ style: NSAlert.Style = .informational) {
        let alert = NSAlert()
        alert.messageText = title
        alert.informativeText = text
        alert.alertStyle = style
        alert.addButton(withTitle: "OK")
        alert.runModal()
    }

    static func showYesNoDialog(title: String, text: String) -> Bool {
        let alert = NSAlert()
        alert.messageText = title
        alert.informativeText = text
        alert.alertStyle = .warning
        alert.addButton(withTitle: ~"yes")
        alert.addButton(withTitle: ~"no")
        let cancel = alert.addButton(withTitle: ~"cancel")
        cancel.keyEquivalent = "\u{1b}" // handle ESC button
        
        return alert.runModal() == .alertFirstButtonReturn
    }
    
    static func showInputBox(title: String, text: String, defaultStr: String? = nil, placeholder: String? = nil, _ style: NSAlert.Style = .informational) -> String? {
        let txt = NSTextField(frame: NSRect(x: 0, y: 0, width: 270, height: 25))
        txt.placeholderString = placeholder
        txt.bezelStyle = .roundedBezel
        txt.stringValue = defaultStr ?? ""

        let alert = NSAlert()
        alert.messageText = title
        alert.informativeText = text
        alert.alertStyle = style
        alert.accessoryView = txt
        alert.window.initialFirstResponder = txt // focused by default
        alert.addButton(withTitle: "OK")
        let cancel = alert.addButton(withTitle: ~"cancel")
        cancel.keyEquivalent = "\u{1b}" // handle ESC button

        if alert.runModal() == .alertFirstButtonReturn {
            return txt.stringValue
        }
        return nil
    }

    static func showOpenFileDialog(message: String, _ allowedExtensions: [String]) -> String? {
        let p = NSOpenPanel() // don't use .fileImporter here because of lack of settings
        p.allowedContentTypes = allowedExtensions.map {UTType(filenameExtension: $0)!}
        p.allowsMultipleSelection = false
        p.canChooseFiles = true
        p.canChooseDirectories = false
        p.isExtensionHidden = false
        p.allowsOtherFileTypes = false
        p.message = message

        return p.runModal() == .OK ? p.url?.path : nil
    }
    
    static func showSaveFileDialog(title: String, message: String, nameLabel: String, defaultName: String, _ allowedExtensions: [String]) -> String? {
        let p = NSSavePanel() // don't use fileExporter here because of lack of settings
        p.allowedContentTypes = allowedExtensions.map {UTType(filenameExtension: $0)!}
        p.canCreateDirectories = true
        p.isExtensionHidden = false
        p.allowsOtherFileTypes = false
        p.showsTagField = false
        p.title = title
        p.message = message
        p.nameFieldLabel = "\(nameLabel):"
        p.nameFieldStringValue = defaultName
        
        return p.runModal() == .OK ? p.url?.path : nil
    }
}

prefix operator ~
prefix func ~ (string: String) -> String {
    return NSLocalizedString(string, comment: "")
}

extension String {
    func insert(_ i: Int, _ string: String) -> String {
        guard (0 <= i && i <= self.count) else {return self}
        return String(self.prefix(i)) + string + String(self.suffix(self.count - i))
    }

    func isWhiteSpace() -> Bool {
        self.trimmingCharacters(in: .whitespaces).isEmpty
    }

    /// Converts comma-separated string (or with any other separator) into array of components
    /// - Returns: string components, without leading/trailing spaces, without empty strings
    func splitted(_ separator: Character = ",") -> [String] {
        self.split(separator: separator, omittingEmptySubsequences: true).map(String.init).map{$0.trimmingCharacters(in: CharacterSet.whitespacesAndNewlines)}.filter{!$0.isEmpty}
    }

    subscript(i: Int) -> String {
        guard (0 <= i && i < self.count) else {return ""}
        return String(self[index(startIndex, offsetBy: i)])
    }
}

extension Color {
    init(hex: String) {
        let hex = hex.trimmingCharacters(in: CharacterSet.alphanumerics.inverted)

        var int: UInt64 = 0
        Scanner(string: hex).scanHexInt64(&int)

        let a, r, g, b: UInt64
        switch hex.count {
        case 3: // RGB (12-bit)
            (a, r, g, b) = (255, (int >> 8) * 17, (int >> 4 & 0xF) * 17, (int & 0xF) * 17)
        case 6: // RGB (24-bit)
            (a, r, g, b) = (255, int >> 16, int >> 8 & 0xFF, int & 0xFF)
        case 8: // ARGB (32-bit)
            (a, r, g, b) = (int >> 24, int >> 16 & 0xFF, int >> 8 & 0xFF, int & 0xFF)
        default:
            (a, r, g, b) = (1, 1, 1, 0)
        }

        self.init(.sRGB, red: Double(r) / 255, green: Double(g) / 255, blue:  Double(b) / 255, opacity: Double(a) / 255)
    }
}
