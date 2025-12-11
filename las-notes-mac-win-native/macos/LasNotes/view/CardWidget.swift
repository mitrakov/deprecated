import SwiftUI

struct CardWidget: View {
    let caption: String
    let text: String
    var onClick: () -> () = {}
    @State private var hover = false

    var body: some View {
        HStack {
            Image("database")
                .resizable()
                .frame(width: 48, height: 48)
                .aspectRatio(contentMode: .fit)

            VStack(alignment: .leading) {
                Text(caption)
                    .font(.title)
                    .accessibilityAddTraits(.isHeader)
                    .padding(.vertical, 4)
                    .padding(.trailing, 16)

                Text(text)
                    .font(.subheadline)
                    .padding(.bottom, 4)
                    .padding(.trailing, 16)

                Rectangle().frame(width: 400, height: 0)
            }
        }
        .contentShape(RoundedRectangle(cornerRadius: 12)) // to say "onTapGesture" to handle full area
        .clipShape(RoundedRectangle(cornerRadius: 12))
        .overlay(RoundedRectangle(cornerRadius: 12).stroke(Color(hex: "2a2ef8"), lineWidth: 1).shadow(radius: 5))
        .background(.blue.opacity(hover ? 0.1 : 0))
        .padding(2)
        .onTapGesture {onClick()}
        .onHover {hover = $0}
    }

    func onClick(_ f: @escaping () -> Void) -> Self {
        var this = self
        this.onClick = f
        return this
    }
}

#Preview {
    CardWidget(caption: "mydatabase.db", text: "/Users/user/workspace/artem/mitrakov/mydatabase.db")
}
