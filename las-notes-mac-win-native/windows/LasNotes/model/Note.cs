namespace LasNotes;

/// <summary>
/// Case class to represent a note
/// </summary>
/// <param name="Id">note ID</param>
/// <param name="Data">markdown string</param>
/// <param name="Tags">comma-separated tags</param>
/// <param name="IsDeleted">soft-deleted marker</param>
internal record Note(long Id, string Data, string Tags, bool IsDeleted);
