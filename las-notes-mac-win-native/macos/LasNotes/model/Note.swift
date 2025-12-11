/// Case class to represent a note
/// - Parameters:
///   - id: note ID
///   - data: markdown string
///   - tags: comma-separated tags
///   - isDeleted: soft-deleted marker
struct Note: Identifiable {
    let id: Int64
    let data: String
    let tags: String
    let isDeleted: Bool
}
