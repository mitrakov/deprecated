import SwiftUI

// based on: https://gist.github.com/unnamedd/6e8c3fbc806b8deb60fa65d6b9affab0
struct LasNotesEditor: NSViewRepresentable {
    @Binding var text: String
    @Binding var selectedRanges: [NSRange]
    let ref = ViewReference()

    func makeNSView(context: Context) -> CustomTextView {
        let textView = CustomTextView(text: text)
        textView.delegate = context.coordinator
        ref.view = textView

        return textView
    }

    func updateNSView(_ view: CustomTextView, context: Context) {
        view.text = text
        view.selectedRanges = selectedRanges.map {NSValue(range: $0)}
    }

    func makeCoordinator() -> Coordinator {
        .init(self)
    }

    func foregroundColor(_ colour: NSColor) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.textColor = colour
        }
        return self
    }

    func backgroundColor(_ colour: NSColor) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.backgroundColor = colour
        }
        return self
    }

    func font(_ font: NSFont) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.font = font
        }
        return self
    }

    func editable(_ value: Bool) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.isEditable = value
        }
        return self
    }

    func selectable(_ value: Bool) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.isSelectable = value
        }
        return self
    }

    func allowsUndo(_ value: Bool) -> Self {
        DispatchQueue.main.async {
            ref.view?.textView.allowsUndo = value
        }
        return self
    }

    func onStartEdit(_ f: @escaping () -> Void) -> Self {
        DispatchQueue.main.async {
            ref.onStart = f
        }
        return self
    }

    func onTextChange(_ f: @escaping (String) -> Void) -> Self {
        DispatchQueue.main.async {
            ref.onUpdate = f
        }
        return self
    }

    func onFinishEdit(_ f: @escaping () -> Void) -> Self {
        DispatchQueue.main.async {
            ref.onEnd = f
        }
        return self
    }
}

final internal class ViewReference {
    weak var view: CustomTextView?
    var onStart: () -> Void = {}
    var onUpdate: (String) -> Void = {_ in}
    var onEnd: () -> Void = {}
}

final internal class Coordinator: NSObject, NSTextViewDelegate {
    let parent: LasNotesEditor

    init(_ parent: LasNotesEditor) {
        self.parent = parent
    }

    func textDidBeginEditing(_ notification: Notification) {
        parent.ref.onStart()
    }

    func textDidChange(_ notification: Notification) {
        guard let textView = notification.object as? NSTextView else { return }

        DispatchQueue.main.asyncAfter(deadline: .now() + 1) {
            self.syncBindings(textView)
        }

        parent.ref.onUpdate(textView.string)
    }

    func textDidEndEditing(_ notification: Notification) {
        parent.ref.onEnd()
    }

    func textViewDidChangeSelection(_ notification: Notification) {
        guard let textView = notification.object as? NSTextView else { return }

        DispatchQueue.main.asyncAfter(deadline: .now() + 1) {
            self.syncBindings(textView)
        }
    }
    
    private func syncBindings(_ textView: NSTextView) {
        // TODO: performance issue, need throttle/debounce
        parent.text = textView.string
        parent.selectedRanges = textView.selectedRanges.map {$0.rangeValue}
    }
}

final internal class CustomTextView: NSView {
    weak var delegate: NSTextViewDelegate?

    var text: String {
        didSet {
            textView.string = text
        }
    }

    var selectedRanges: [NSValue] = [] {
        didSet {
            guard selectedRanges.count > 0 else { return }
            textView.selectedRanges = selectedRanges
        }
    }

    private lazy var scrollView: NSScrollView = {
        let scrollView = NSScrollView()
        scrollView.drawsBackground = true
        scrollView.borderType = .noBorder
        scrollView.hasVerticalScroller = true
        scrollView.hasHorizontalRuler = false
        scrollView.autoresizingMask = [.width, .height]
        scrollView.translatesAutoresizingMaskIntoConstraints = false

        return scrollView
    }()

    lazy var textView: NSTextView = {
        let contentSize = scrollView.contentSize
        let textStorage = NSTextStorage()

        let layoutManager = NSLayoutManager()
        textStorage.addLayoutManager(layoutManager)

        let textContainer = NSTextContainer(containerSize: scrollView.frame.size)
        textContainer.widthTracksTextView = true
        textContainer.containerSize = NSSize(
            width: contentSize.width,
            height: CGFloat.greatestFiniteMagnitude
        )

        layoutManager.addTextContainer(textContainer)

        let textView                     = NSTextView(frame: .zero, textContainer: textContainer)
        textView.autoresizingMask        = .width
        textView.backgroundColor         = NSColor.textBackgroundColor
        textView.delegate                = self.delegate
        textView.drawsBackground         = true
        textView.font                    = .systemFont(ofSize: 12)
        textView.isEditable              = true
        textView.isHorizontallyResizable = false
        textView.isVerticallyResizable   = true
        textView.maxSize                 = NSSize(width: CGFloat.greatestFiniteMagnitude, height: CGFloat.greatestFiniteMagnitude)
        textView.minSize                 = NSSize(width: 0, height: contentSize.height)
        textView.textColor               = NSColor.labelColor
        textView.allowsUndo              = true

        return textView
    }()

    init(text: String) {
        self.text = text
        super.init(frame: .zero)
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) not implemented")
    }

    override func viewWillDraw() {
        super.viewWillDraw()

        setupScrollViewConstraints()
        setupTextView()
    }

    func setupScrollViewConstraints() {
        scrollView.translatesAutoresizingMaskIntoConstraints = false

        addSubview(scrollView)

        NSLayoutConstraint.activate([
            scrollView.topAnchor.constraint(equalTo: topAnchor),
            scrollView.trailingAnchor.constraint(equalTo: trailingAnchor),
            scrollView.bottomAnchor.constraint(equalTo: bottomAnchor),
            scrollView.leadingAnchor.constraint(equalTo: leadingAnchor)
        ])
    }

    func setupTextView() {
        scrollView.documentView = textView
    }
}
