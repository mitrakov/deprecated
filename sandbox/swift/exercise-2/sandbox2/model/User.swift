import SwiftUI

struct User: Identifiable {
    let id = UUID().uuidString
    let name: String
    let url: String
}

struct Post: Identifiable {
    var id = UUID().uuidString
    let user: User
    let imageUrl: String
    let title: String
    let likes: String
    let shares: String
    let comments: String
    let postTime: String
}

let users = [
    User(name: "Jesie Samson", url: "https://images.unsplash.com/photo-1517841905240-472988babdf9"),
    User(name: "Bobby Dilman", url: "https://images.unsplash.com/photo-1528892952291-009c663ce843"),
    User(name: "Tommy Stamer", url: "https://images.unsplash.com/photo-1500648767791-00dcc994a43e"),
    User(name: "Timmy Hanson", url: "https://images.unsplash.com/photo-1517841905240-472988babdf9"),
    User(name: "Alice Wegdon", url: "https://images.unsplash.com/photo-1528892952291-009c663ce843"),
    User(name: "Kylie Marman", url: "https://images.unsplash.com/photo-1500648767791-00dcc994a43e"),
]

let posts = [
    Post(user: users[0], imageUrl: "https://images.unsplash.com/photo-1473496169904-658ba7c44d8a", title: "Summer vacation",   likes: "89", shares: "22", comments: "17", postTime: "58"),
    Post(user: users[1], imageUrl: "https://images.unsplash.com/photo-1519150268069-c094cfc0b3c8", title: "Cutties 😍"     ,   likes: "22", shares: "12", comments: "12", postTime: "59"),
    Post(user: users[2], imageUrl: "https://images.unsplash.com/photo-1509316785289-025f5b846b35", title: "Enjoying my trip!", likes: "33", shares: "27", comments: "11", postTime: "60"),
    Post(user: users[3], imageUrl: "https://images.unsplash.com/photo-1519335337423-a3357c2cd12e", title: "Big Surrrrrrrrrrr", likes: "90", shares: "39", comments: "19", postTime: "61"),
    Post(user: users[4], imageUrl: "https://images.unsplash.com/photo-1502945015378-0e284ca1a5be", title: "My New Artwork!!!", likes: "16", shares: "55", comments: "23", postTime: "62"),
]
