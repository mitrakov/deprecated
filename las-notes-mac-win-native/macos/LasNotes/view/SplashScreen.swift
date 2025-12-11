import SwiftUI

struct SplashScreen: View {
    @EnvironmentObject var vm: MainViewModel

    var body: some View {
        VStack {
            HStack {
                Button("splash-new", action: vm.newFile)
                    .buttonStyle(BigButtonStyle())
                    .padding(.horizontal, 20)
                    .help(String("⌘⇧N"))

                Button("splash-open", action: vm.openFile)
                    .buttonStyle(BigButtonStyle())
                    .padding(.horizontal, 20)
                    .help(String("⌘O"))
            }
            ScrollView {
                ForEach(vm.getRecentFiles(), id: \.self) { path in
                    CardWidget(caption: (path as NSString).lastPathComponent, text: path)
                        .onClick { vm.openFile(path) }
                }
            }
        }
        .frame(height: 780)
        .padding(10)
        .border(.black)
        .background(Color(hex: "99ecfbff"))
        .shadow(color: .black.opacity(0.2), radius: 5, x: 5, y: 5)
    }
}

struct BigButtonStyle: ButtonStyle {
    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .frame(width: 155, height: 90)
            .font(Font.system(size: 16))
            .frame(minWidth: 50)
            .padding(.horizontal, 10)
            .multilineTextAlignment(.center)
            .foregroundColor(configuration.isPressed ? .white : .black)
            .background(configuration.isPressed ? Color(hex: "2caaff") : .white)
            .clipShape(RoundedRectangle(cornerRadius: 12))
            .overlay(RoundedRectangle(cornerRadius: 12).stroke(.blue, lineWidth: 1).shadow(radius: 1))
    }
}

#Preview {
    SplashScreen().environmentObject(MainViewModel())
}
