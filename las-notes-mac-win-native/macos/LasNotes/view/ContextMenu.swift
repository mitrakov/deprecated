import SwiftUI

struct ContextMenu: View {
    let isArchived: Bool
    let tags: [String]
    var onEdit: () -> Void = {}
    var onArchive: () -> Void = {}
    var onRestore: () -> Void = {}
    var onDelete: () -> Void = {}

    @State private var hover1 = false
    @State private var hover2 = false
    @State private var hover3 = false

    var body: some View {
        HStack {
            Spacer()
            ForEach(tags, id: \.self) {
                Label($0, systemImage: "tag")
                    .padding(4)
                    .border(.blue)
                    .cornerRadius(16)
                    .background(Capsule().strokeBorder(.purple))
                    .opacity(isArchived ? 0.6 : 1)
            }
            
            if (isArchived) {
                Button(action: onRestore, label: {
                    Image(systemName: "arrow.uturn.backward.circle")
                        .font(.system(size: 20, weight: .bold))
                        .padding(4)
                        .background(.green.opacity(hover1 ? 1 : 0.5))
                        .clipShape(Circle())
                })
                .buttonStyle(PlainButtonStyle())
                .help("restore-note")
                .onHover { hover1 = $0 }
                .padding(.trailing, 16)
            } else {
                Button(action: onEdit, label: {
                    Image(systemName: "pencil.circle")
                        .font(.system(size: 20, weight: .bold))
                        .padding(4)
                        .background(.blue.opacity(hover1 ? 1 : 0.5))
                        .clipShape(Circle())
                })
                .buttonStyle(PlainButtonStyle())
                .help("edit-note")
                .onHover { hover1 = $0 }

                Button(action: onArchive, label: {
                    Image(systemName: "archivebox.circle")
                        .font(.system(size: 20, weight: .bold))
                        .padding(4)
                        .background(.orange.opacity(hover2 ? 1 : 0.5))
                        .clipShape(Circle())
                })
                .buttonStyle(PlainButtonStyle())
                .help("archive-note")
                .onHover { hover2 = $0 }
                
                Button (action: onDelete, label: {
                    Image(systemName: "trash.slash")
                        .font(.system(size: 16, weight: .bold))
                        .padding(8)
                        .background(.red.opacity(hover3 ? 1 : 0.8))
                        .clipShape(Circle())
                })
                .buttonStyle(PlainButtonStyle())
                .help("delete-note")
                .onHover { hover3 = $0 }
                .padding(.trailing, 16)
            }
        }
    }

    func onEdit(_ f: @escaping () -> Void) -> Self {
        var this = self
        this.onEdit = f
        return this
    }

    func onArchive(_ f: @escaping () -> Void) -> Self {
        var this = self
        this.onArchive = f
        return this
    }

    func onRestore(_ f: @escaping () -> Void) -> Self {
        var this = self
        this.onRestore = f
        return this
    }

    func onDelete(_ f: @escaping () -> Void) -> Self {
        var this = self
        this.onDelete = f
        return this
    }
}

#Preview {
    ContextMenu(isArchived: false, tags: ["One", "Two", "Three"])
}
