import SwiftUI
import SDWebImageSwiftUI

struct Home: View {
    let window = NSScreen.main!.visibleFrame
    @State var search = ""
    var columns = Array(repeating: GridItem(.flexible(), spacing: 15), count: 4)
    @StateObject var imageData = ImageViewModel()
    
    var body: some View {
        HStack {
            SideBar()
            
            VStack {
                HStack(spacing: 12) {
                    // search bar
                    HStack(spacing: 15) {
                        Image(systemName: "magnifyingglass").foregroundColor(.gray)
                        TextField("Search", text: $search).textFieldStyle(PlainTextFieldStyle())
                    }
                    .padding(.vertical, 10)
                    .padding(.horizontal)
                    .background(BlurWindow())
                    .cornerRadius(10)
                    
                    Button(action: {}, label: {
                        Image(systemName: "slider.vertical.3")
                            .foregroundColor(.gray)
                            .padding(10)
                            .background(Color.white)
                            .cornerRadius(10)
                            .shadow(color: Color.black.opacity(0.1), radius: 5, x: 5, y: 5)
                            .shadow(color: Color.black.opacity(0.1), radius: 5, x: -5, y: 5)
                    }).buttonStyle(PlainButtonStyle())
                    
                    Button(action: {}, label: {
                        Image(systemName: "plus")
                            .foregroundColor(.white)
                            .padding(10)
                            .background(Color.black)
                            .cornerRadius(10)
                    }).buttonStyle(PlainButtonStyle())
                }
                
                // scroll view
                GeometryReader { reader in
                    ScrollView {
                        LazyVGrid(columns: columns, spacing: 15, content: {
                            // getting images
                            ForEach(imageData.images.indices, id: \.self) { i in
                                ZStack {
                                    WebImage(url: URL(string: imageData.images[i].download_url)) { image in
                                        image
                                            .resizable()
                                            .aspectRatio(contentMode: .fill)
                                            .frame(width: (reader.frame(in: .global).width - 45) / 4, height: 150)
                                            .cornerRadius(15)
                                    } placeholder: {
                                        ProgressView()
                                    }
                                    .indicator(.activity) // Activity Indicator
                                    .transition(.fade(duration: 0.5)) // Fade Transition with duration
                                    .scaledToFit()
                                    .frame(width: (reader.frame(in: .global).width - 45) / 4, height: 150)
                                    
                                    //Color.black.opacity(imageData.images[i].onHover ?? false ? 0.2 : 0)
                                    
                                    VStack {
                                        HStack {
                                            Spacer(minLength: 0)
                                            Button(action: {}, label: {
                                                Image(systemName: "hand.thumbsup.fill")
                                                    .foregroundColor(.gray)
                                                    .padding(8)
                                                    .background(Color.white)
                                                    .cornerRadius(10)
                                            }).buttonStyle(PlainButtonStyle())
                                            Button(action: {saveImage(i: i)}, label: {
                                                Image(systemName: "folder.fill")
                                                    .foregroundColor(.blue)
                                                    .padding(8)
                                                    .background(Color.white)
                                                    .cornerRadius(10)
                                            }).buttonStyle(PlainButtonStyle())
                                        }
                                        .padding(10)
                                        
                                        Spacer()
                                    }
                                    .opacity(imageData.images[i].onHover ?? false ? 1 : 0)
                                }
                                .onHover(perform: { hover in
                                    withAnimation {
                                        imageData.images[i].onHover = hover
                                    }
                                })
                            }
                        })
                        .padding(.vertical)
                    }
                }
                
                Spacer()
            }.padding()
        }
        .ignoresSafeArea(.all, edges: .all)
        .frame(width: window.width / 1.5, height: window.height - 100)
        .background(Color.white.opacity(0.6))
        .background(BlurWindow())
    }
    
    func saveImage(i: Int) {
        let manager = SDWebImageDownloader(config: .default)
        manager.downloadImage(with: URL(string: imageData.images[i].download_url)!) { (image, _, _, _) in
            guard let imageOriginal = image else {return}
            let data = imageOriginal.sd_imageData(as: .JPEG)
            let panel = NSSavePanel()
            panel.canCreateDirectories = true
            panel.nameFieldStringValue = "\(imageData.images[i].id).jpg"
            panel.begin { response in
                if response.rawValue == NSApplication.ModalResponse.OK.rawValue {
                    do {
                        try data?.write(to: panel.url!, options: .atomicWrite)
                        print("success!")
                    } catch {
                        print(error.localizedDescription)
                    }
                }
            }
        }
        
        
    }
}

struct SideBar: View {
    @Namespace var animation
    @State var selected: String = "Home"
    var body: some View {
        HStack(spacing: 0) {
            VStack(spacing: 22) {
                Group {
                    HStack {
                        Image("logo")
                            .resizable()
                            .aspectRatio(contentMode: .fill)
                            .frame(width: 45, height: 45)
                        Text("TommyNotes").fontWeight(.semibold).foregroundColor(.black)
                        Spacer(minLength: 0)
                    }.padding(.top, 35).padding(.leading, 10)
                    
                    TabButton(image: "house.fill", title: "Home", selected: $selected, animation: animation)
                    TabButton(image: "clock.fill", title: "Recents", selected: $selected, animation: animation)
                    TabButton(image: "person.2.fill", title: "Following", selected: $selected, animation: animation)
                    
                    HStack {
                        Text("Insights").fontWeight(.semibold).foregroundColor(.gray)
                        Spacer()
                    }.padding()
                    
                    TabButton(image: "message.fill", title: "Messages", selected: $selected, animation: animation)
                    TabButton(image: "bell.fill", title: "Notifications", selected: $selected, animation: animation)
                    
                }
                
                Spacer(minLength: 0)
                
                VStack(spacing: 8) {
                    Image("business").resizable().aspectRatio(contentMode: .fit)
                    Button(action: {}, label: {
                        Text("Business Tools").fontWeight(.semibold).foregroundColor(.blue)
                    }).buttonStyle(PlainButtonStyle())
                    
                    Text("Unlock our new buisness tools at your convinence!")
                        .foregroundColor(.gray)
                        .multilineTextAlignment(.center)
                        .padding(.horizontal, 10)
                    
                }
                
                Spacer(minLength: 0)
                
                // profile view
                HStack(spacing: 10) {
                    Image("profile")
                        .resizable()
                        .aspectRatio(contentMode: .fill)
                        .frame(width: 75, height: 75)
                        .clipShape(Circle())
                    
                    VStack(alignment: .leading, spacing: 8, content: {
                        Text("Justine").font(.caption).fontWeight(.semibold).foregroundColor(.black)
                        Text("Last login 2024-09-20").font(.caption2).foregroundColor(.gray)
                    })
                    
                    Spacer(minLength: 0)
                    Image(systemName: "chevron.right").foregroundColor(.gray)
                }
                .padding(.vertical, 10)
                .padding(.horizontal, 8)
                .background(Color.white)
                .cornerRadius(10)
                .shadow(color: Color.black.opacity(0.1), radius: 5, x: 5, y: 5)
                .shadow(color: Color.black.opacity(0.1), radius: 5, x: -5, y: 5)
                .padding(.bottom, 20)
            }
            
            Divider().offset(x: -2)
        }.frame(width: 240)
    }
}

extension NSTextField {
    open override var focusRingType: NSFocusRingType {
        get {.none}
        set {}
    }
}

#Preview {
    Home()
}
