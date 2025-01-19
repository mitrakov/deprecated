//  next 9:35

import SwiftUI
import SDWebImageSwiftUI

struct Home: View {
    let screen = NSScreen.main!.visibleFrame
    @State var selected: String = "house.fill"
    @Namespace var animation
    
    var body: some View {
        VStack {
            ZStack(alignment: Alignment(horizontal: .center, vertical: .bottom)) {
                HStack {
                    Text("Tommynotes")
                        .font(.largeTitle)
                        .fontWeight(.heavy)
                        .foregroundColor(Color("fb"))
                    Spacer()
                    
                    Button {} label: {
                        HStack {
                            Image("logo")
                                .resizable()
                                .aspectRatio(contentMode: .fill)
                                .frame(width: 35, height: 35)
                                .clipShape(Circle())
                            Text("Tommy Soft")
                                .foregroundColor(.black)
                                .fontWeight(.bold)
                        }
                    }
                    .buttonStyle(PlainButtonStyle())
                    
                    Button {} label: {
                        Image(systemName: "magnifyingglass")
                            .font(.system(size: 16, weight: .bold))
                            .foregroundColor(.black)
                            .padding(10)
                            .background(Color.gray.opacity(0.2))
                            .clipShape(Circle())
                    }
                    .buttonStyle(PlainButtonStyle())
                    .padding(.leading, 8)
                    
                    Button {} label: {
                        Image("messenger")
                            .resizable()
                            .aspectRatio(contentMode: .fit)
                            .frame(width: 20, height: 20)
                            .padding(10)
                            .background(Color.gray.opacity(0.2))
                            .clipShape(Circle())
                    }
                    .buttonStyle(PlainButtonStyle())

                }
                .padding(.vertical)
                .padding(.leading, 10)
                .padding(.trailing)
                .padding(.top, 12)
                .background(Color.white)
                .shadow(color: Color.black.opacity(0.08), radius: 5, x: 0, y: 5)
                
                // Tab view
                HStack {
                    TabButton(image: "house.fill", selected: $selected, animation: animation)
                    TabButton(image: "play.tv", selected: $selected, animation: animation)
                    TabButton(image: "person.circle", selected: $selected, animation: animation)
                    TabButton(image: "person.3.fill", selected: $selected, animation: animation)
                    TabButton(image: "bell", selected: $selected, animation: animation)
                    TabButton(image: "line.horizontal.3", selected: $selected, animation: animation)
                }
            }
            
            HStack {
                // side tabs views
                SideTabView()
                //.background(Color.white)
                
                PostView()
                
                // Side contact views
                ContactView()
                //.background(Color.white)
            }.frame(maxHeight: .infinity)
        }
        .ignoresSafeArea(.all, edges: .all)
        .frame(width: screen.width / 1.4, height: screen.height - 200)
        .background(Color.gray.opacity(0.2))
        .preferredColorScheme(.light)
    }
}

#Preview {
    Home()
}

struct TabButton: View {
    let image: String
    @Binding var selected: String
    var animation: Namespace.ID
    
    var body: some View {
        Button(action: {
            withAnimation {
                selected = image
            }
        }, label: {
            VStack(spacing: 0) {
                Image(systemName: image)
                    .font(.title)
                    .foregroundColor(selected == image ? Color("fb") : Color.gray.opacity(0.7))
                    .frame(height: 40)
                
                ZStack {
                    Capsule()
                        .fill(Color.clear)
                        .frame(width: 65, height: 3)
                        .matchedGeometryEffect(id: "Tab", in: animation)
                    
                    if selected == image {
                        Capsule()
                            .fill(Color("fb"))
                            .frame(width: 65, height: 3)
                            .matchedGeometryEffect(id: "Tab", in: animation)
                    }
                }
            }
        })
        .buttonStyle(PlainButtonStyle())
    }
}

struct SideTabButton: View {
    let image: String
    let title: String
    let color: String
    var isSystem: Bool
    
    var body: some View {
        Button {
            
        } label: {
            HStack(spacing: 8) {
                if isSystem {
                    Image(systemName: image)
                        .font(.system(size: 25))
                        .foregroundColor(Color(color))
                        .frame(width: 25)
                } else {
                    Image(image)
                        .resizable()
                        .renderingMode(color == "" ? .original : .template)
                        .aspectRatio(contentMode: .fit)
                        .foregroundColor(Color(color))
                        .frame(width: 25, height: 25)
                }
                
                Text(title).foregroundColor(.black)
            }.padding(.horizontal)
        }
        .buttonStyle(PlainButtonStyle())
        .frame(maxWidth: .infinity, alignment: .leading)

    }
}

struct SideTabView: View {
    let screen = NSScreen.main!.visibleFrame
    var body: some View {
        VStack(spacing: 18) {
            SideTabButton(image: "logo", title: "Tommy Soft", color: "", isSystem: false).padding(.top, 20)
            SideTabButton(image: "shield.checkerboard", title: "Covid Info", color: "covid", isSystem: true)
            SideTabButton(image: "person.2.fill", title: "Friends", color: "friends", isSystem: true)
            SideTabButton(image: "messenger", title: "Messenger", color: "fb", isSystem: false)
            SideTabButton(image: "flag.fill", title: "Pages", color: "friends", isSystem: true)
            SideTabButton(image: "shop", title: "MarketPlace", color: "covid", isSystem: false)
            SideTabButton(image: "tv.fill", title: "Watch", color: "fb", isSystem: true)
            SideTabButton(image: "messenger", title: "Events", color: "friends", isSystem: false)
            
            Spacer()
        }
        .frame(maxWidth: (screen.width / 1.8) / 4, maxHeight: .infinity)
    }
}

struct ContactView: View {
    let screen = NSScreen.main!.visibleFrame
    var body: some View {
        VStack {
            HStack {
                Text("Contacts")
                    .fontWeight(.bold)
                    .foregroundColor(.gray)
                
                Spacer()
                
                Button(action: {}, label: {
                    Image(systemName: "magnifyingglass")
                        .foregroundColor(.gray)
                })
                .buttonStyle(PlainButtonStyle())
                
                Button(action: {}, label: {
                    Image(systemName: "slider.vertical.3")
                        .foregroundColor(.gray)
                })
                .buttonStyle(PlainButtonStyle())
            }
            .padding()
            
            ScrollView {
                ForEach(users) { user in
                    HStack {
                        WebImage(url: URL(string: user.url)!)
                            .resizable()
                            .aspectRatio(contentMode: .fill)
                            .frame(width: 35, height: 35)
                            .clipShape(Circle())
                        
                        Text(user.name)
                            .foregroundColor(.black)
                        
                        Spacer()
                    }
                    .padding(.horizontal)
                }
            }
        }
        .frame(maxWidth: (screen.width / 1.8) / 4, maxHeight: .infinity)
    }
}

struct PostCardView: View {
    let post: Post
    var body: some View {
        VStack {
            HStack {
                WebImage(url: URL(string: post.user.url))
                    .resizable()
                    .aspectRatio(contentMode: .fill)
                    .frame(width: 35, height: 35)
                    .clipShape(Circle())
                
                VStack(alignment: .leading, spacing: 5, content: {
                    Text(post.user.name)
                        .foregroundColor(.black)
                    
                    HStack(spacing: 5) {
                        Text(post.postTime)
                            .foregroundColor(.gray)
                        
                        Circle()
                            .fill(Color.gray)
                            .frame(width: 3, height: 3)
                        
                        Image(systemName: "globe")
                            .foregroundColor(.gray)
                    }
                })
                
                Spacer()
                
                Button(action: {}, label: {
                    Image("menu")
                        .resizable()
                        .aspectRatio(contentMode: .fit)
                        .frame(width: 18, height: 18)
                        .foregroundColor(.black)
                        .rotationEffect(.init(degrees: 2))
                })
                .buttonStyle(PlainButtonStyle())
            }
            .padding(.horizontal, 10)
            
            Text(post.title)
                .foregroundColor(.black)
                .frame(maxWidth: .infinity, alignment: .leading)
                .padding(.horizontal, 10)
            
            WebImage(url: URL(string: post.imageUrl))
                .resizable()
                .aspectRatio(contentMode: .fill)
            
            HStack {
                Image(systemName: "hand.thumbsup.fill")
                    .foregroundColor(Color("fb"))
                
                Text("\(post.likes)")
                
                Spacer()
                
                Text("\(post.comments) comments")
                    .foregroundColor(.gray)
                
                Text("\(post.shares) shares")
                    .foregroundColor(.gray)
            }
            .padding(.horizontal, 10)
            .padding(.top, 5)
            
            Divider()
            
            HStack {
                Button(action: {}, label: {
                    Label(title: {
                        Text("Like")
                    }, icon: {
                        Image(systemName: "hand.thumbsup")
                    })
                    .foregroundColor(.gray)
                    .frame(maxWidth: .infinity)
                })
                .buttonStyle(PlainButtonStyle())
                
                Divider().padding(.vertical, -5)
                
                Button(action: {}, label: {
                    Label(title: {
                        Text("Comment")
                    }, icon: {
                        Image(systemName: "arrow.up.message")
                    })
                    .foregroundColor(.gray)
                    .frame(maxWidth: .infinity)
                })
                .buttonStyle(PlainButtonStyle())
                
                Divider().padding(.vertical, -5)
                
                Button(action: {}, label: {
                    Label(title: {
                        Text("Share")
                    }, icon: {
                        Image(systemName: "square.and.arrow.up")
                    })
                    .foregroundColor(.gray)
                    .frame(maxWidth: .infinity)
                })
                .buttonStyle(PlainButtonStyle())
            }
            .padding(.horizontal, 10)
            .padding(.top, 10)
        }
        .padding(.vertical)
        .background(Color.white)
        .cornerRadius(10)
    }
}

struct PostView: View {
    @State var post: String = ""
    
    var body: some View {
        ScrollView {
            VStack {
                // my post view
                VStack {
                    HStack {
                        Image("logo")
                            .resizable()
                            .aspectRatio(contentMode: .fill)
                            .frame(width: 35, height: 35)
                            .clipShape(Circle())
                        
                        TextField("What's up bro?", text: $post)
                            .textFieldStyle(PlainTextFieldStyle())
                    }
                    
                    Divider()
                    
                    HStack {
                        Button(action: {}, label: {
                            Label(title: {
                                Text("Live").foregroundColor(.black)
                            }, icon: {
                                Image(systemName: "video.fill").foregroundColor(.red)
                            }).frame(maxWidth: .infinity)
                        })
                        .buttonStyle(PlainButtonStyle())
                        
                        Divider().padding(.vertical, -5)
                        
                        Button(action: {}, label: {
                            Label(title: {
                                Text("Photos").foregroundColor(.black)
                            }, icon: {
                                Image(systemName: "photo.on.rectangle").foregroundColor(.green)
                            }).frame(maxWidth: .infinity)
                        })
                        .buttonStyle(PlainButtonStyle())
                        
                        Divider().padding(.vertical, -5)
                        
                        Button(action: {}, label: {
                            Label(title: {
                                Text("Room").foregroundColor(.black)
                            }, icon: {
                                Image(systemName: "video.fill.badge.plus").foregroundColor(.purple)
                            }).frame(maxWidth: .infinity)
                        })
                        .buttonStyle(PlainButtonStyle())
                    }
                }
                .padding(10)
                .background(Color.white)
                .cornerRadius(10)
                
                ScrollView(.horizontal, showsIndicators: false, content: {
                    HStack {
                        Button(action: {}, label: {
                            Label(title: {Text("Create Room")}, icon: {
                                Image(systemName: "video.badge.plus")
                                    .foregroundColor(.purple)
                            })
                            .padding(10)
                            .background(Capsule().strokeBorder(Color.purple))
                        })
                        .buttonStyle(PlainButtonStyle())
                        
                        ForEach(users) { user in
                            ZStack(alignment: Alignment(horizontal: .trailing, vertical: .bottom), content: {
                                WebImage(url: URL(string: user.url))
                                    .resizable()
                                    .aspectRatio(contentMode: .fill)
                                    .frame(width: 35, height: 35)
                                    .clipShape(Circle())
                                
                                Circle()
                                    .fill(Color.green)
                                    .frame(width: 10, height: 10)
                            })
                        }
                    }
                    .padding(10)
                })
                .background(Color.white)
                .cornerRadius(10)
                
                ForEach(posts) { post in
                    PostCardView(post: post)
                }
            }
            .padding()
        }
        .frame(maxWidth: .infinity, maxHeight: .infinity)
    }
}
