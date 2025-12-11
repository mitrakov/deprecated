import SwiftUI

struct Toolbar: View {
    @Binding var txt: String
    @Binding var range: [NSRange]
    @FocusState.Binding var focus: FocusField?
    let like: Bool

    var body: some View {
        HStack {
            Button(String("H1")) { prepend("#", "Header") } // "String()" to avoid l10n
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-h1")

            Button(String("h2")) { prepend("##", "Header") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-h2")

            Button(String("h3")) { prepend("###", "Header") }
                .buttonStyle(ToolbarBtnStyle(fontSize: 10, topPadding: 2))
                .help("toolbar-h3")

            Button(String("B")) { surround("**", "text", "**") }
                .buttonStyle(ToolbarBtnStyle(fontSize: 14, bold: true))
                .help("toolbar-bold")

            Button(String("I")) { surround("*", "text", "*") }
                .buttonStyle(ToolbarBtnStyle(italic: true))
                .help("toolbar-italic")

            Button(String("Ꞩ")) { surround("~~", "text", "~~") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-strike")

            Button(String("``")) { surround("`", "text", "`") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-ticks")

            Button(String("⋮")) { prepend("*", "item") }
                .buttonStyle(ToolbarBtnStyle(fontSize: 20))
                .help("toolbar-bullet-list")

            Button(String("1.")) { prepend("1.", "item") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-num-list")

            Button(String("❝❞")) { prepend(">", "quote") }
                .buttonStyle(ToolbarBtnStyle(fontSize: 10))
                .help("toolbar-quote")

            Button(String("🔗")) { surround("[", "Link", "](https://lasnotes.com) ") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-link")

            Button(String("</>")) { surround("```\n", "Code", "\n```\n", forceNewline: true) }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-code")

            Button(String("⸺")) { append("\n---"); }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-rule")

            Button(String("𝍖")) { append("|  |  |  |\n|:-|--|-:|\n|  |  |  |\n|  |  |  |") }
                .buttonStyle(ToolbarBtnStyle(fontSize: 22))
                .help("toolbar-table")

            Button(String("🖼")) { append("![](https://lasnotes.com/picture/)") }
                .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-image")

            Button(String("❓")) {
                NSWorkspace.shared.open(URL(string: "https://www.markdownguide.org/cheat-sheet/")!)
                focus = .editor
            }   .buttonStyle(ToolbarBtnStyle())
                .help("toolbar-more")

            Spacer()

            Button {
                NSWorkspace.shared.open(URL(string: "https://lasnotes.com/donate")!)
                focus = .editor
            } label: {
                Image(like ? "heart2" : "heart1")
                    .resizable()
                    .frame(width: 24, height: 24)
                    .aspectRatio(contentMode: .fit)
            }
            .buttonStyle(PlainButtonStyle())
            .help("toolbar-donate")
        }
        .padding(.horizontal, 6)
        .padding(4)
        .background(Color(hex: "d3d3d3"))
    }

    private func prepend(_ str: String, _ placeholder: String) {
        var idx = range.first?.location ?? 0
        let s = emptyLine(txt, idx) ? "\(str) \(placeholder)\n" : "\(str) "

        while (idx > 0 && txt[idx - 1] != "\n") {idx -= 1} // move caret left

        txt = txt.insert(idx, s)
        range = [NSRange(location: idx + s.count, length: 0)]
        focus = .editor
    }

    private func surround(_ str1: String, _ placeholder: String, _ str2: String, forceNewline: Bool = false) {
        var idx = range.first?.location ?? 0
        var len = range.first?.length ?? 0

        while (len > 0 && (txt[idx+len-1]) == " ") {len -= 1} // remove spaces from selection

        if (len == 0) { // no selection => add placeholder
            while (forceNewline && idx < txt.count && txt[idx] != "\n") {idx += 1} // move caret right

            let s = forceNewline && !emptyLine(txt, idx) ? "\n\(str1)\(placeholder)\(str2)" : "\(str1)\(placeholder)\(str2)"
            txt = txt.insert(idx, s)
            idx += s.count
        } else {
            let s = forceNewline && !fullLineSelected(txt, idx, len) ? "\n\(str1)" : str1
            txt = txt.insert(idx, s).insert(idx + s.count + len, str2)
            idx += s.count + len + str2.count
        }

        range = [NSRange(location: idx, length: 0)]
        focus = .editor
    }

    private func append(_ str: String) {
        var idx = range.first?.location ?? 0

        while (idx < txt.count && txt[idx] != "\n") {idx += 1} // move caret right
        let s = emptyLine(txt, idx) ? "\(str)\n" : "\n\(str)\n"

        txt = txt.insert(idx, s)
        range = [NSRange(location: idx + s.count, length: 0)]
        focus = .editor
    }

    private func emptyLine(_ txt: String, _ idx: Int) -> Bool {
        return (idx == txt.count || txt[idx] == "\n") &&
               (idx == 0         || txt[idx - 1] == "\n")
    }

    private func fullLineSelected(_ txt: String, _ idx: Int, _ selLength: Int) -> Bool {
        return selLength > 0 &&
        (idx == 0                     || txt[idx - 1] == "\n") &&
        (idx + selLength == txt.count || txt[idx + selLength] == "\n")
    }
}

struct ToolbarBtnStyle: ButtonStyle {
    var fontSize: CGFloat?
    var bold: Bool = false
    var italic: Bool = false
    var topPadding: CGFloat?

    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .frame(width: 32, height: 26)
            .padding(.top, topPadding ?? 0)
            .padding(.bottom, -(topPadding ?? 0))
            .font(makeFont(bold, italic, fontSize))
            .foregroundColor(configuration.isPressed ? .white : .black)
            .background(configuration.isPressed ? .gray : .white)
            .clipShape(RoundedRectangle(cornerRadius: 6))
            .overlay(RoundedRectangle(cornerRadius: 6).stroke(.black, lineWidth: 0.5).shadow(radius: 1))
    }

    func makeFont(_ bold: Bool = false, _ italic: Bool = false, _ size: CGFloat?) -> Font {
        if (bold) {
            return Font.system(size: size ?? 12).bold()
        } else if (italic) {
            return Font.system(size: size ?? 12).italic()
        } else if (size != nil) {
            return Font.system(size: size ?? 12)
        }
        return Font.body
    }
}

#Preview {
    struct Preview: View {
        @State var s = ""
        @State var range: [NSRange] = []
        @FocusState private var f: FocusField?
        var body: some View {
            Toolbar(txt: $s, range: $range, focus: $f, like: true)
        }
    }

    return Preview()
}
