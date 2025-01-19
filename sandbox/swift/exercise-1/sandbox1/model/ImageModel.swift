import SwiftUI

struct ImageModel: Codable, Identifiable {
    var id: String
    var download_url: String
    var onHover: Bool?
}
