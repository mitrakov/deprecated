import SwiftUI

// workaround for bug: https://stackoverflow.com/q/74585499
struct Textbox: NSViewRepresentable {
    @Binding var stringValue: String
    var placeholder: String
    var onChange: (() -> Void)?
    var onCommit: (() -> Void)?
    var onEnter: (() -> Void)?
    
    func makeNSView(context: Context) -> NSTextField {
        let textField = NSTextField()
        textField.stringValue = stringValue
        textField.placeholderString = placeholder
        textField.delegate = context.coordinator
        textField.alignment = .left
        textField.bezelStyle = .roundedBezel
        return textField
    }
    
    func updateNSView(_ nsView: NSTextField, context: Context) { }
    
    func makeCoordinator() -> Coordinator {
        Coordinator(with: self)
    }
    
    class Coordinator: NSObject, NSTextFieldDelegate {
        let parent: Textbox
        
        init(with parent: Textbox) {
            self.parent = parent
            super.init()
        }

        func controlTextDidChange(_ obj: Notification) {
            guard let textField = obj.object as? NSTextField else { return }
            parent.stringValue = textField.stringValue
            parent.onChange?()
        }
        
        func control(_ control: NSControl, textShouldEndEditing fieldEditor: NSText) -> Bool {
            parent.stringValue = fieldEditor.string
            parent.onCommit?()
            return true
        }
        
        func control(_ control: NSControl, textView: NSTextView, doCommandBy commandSelector: Selector) -> Bool {
            if commandSelector == #selector(NSStandardKeyBindingResponding.insertNewline(_:)) {
                parent.stringValue = textView.string
                parent.onEnter?()
                return true
            }
            return false
        }
    }
}
