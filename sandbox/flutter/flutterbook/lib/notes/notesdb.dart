import 'package:flutterbook/notes/note.dart';
import 'package:sqflite/sqflite.dart';

class NotesDbWorker {
  NotesDbWorker._();
  static final NotesDbWorker instance = NotesDbWorker._();

  Database _db;

  Future<Database> get database async {
    if (_db == null)
      _db = await init();
    return _db;
  }

  Future<Database> init() {
    return openDatabase(
      "db/notes.db",
      version: 1,
      onCreate: (db, version) {
        db.execute("CREATE TABLE IF NOT EXISTS notes (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, content TEXT, color TEXT);");
      }
    );
  }

  Future<int> create(Note note) async {
    final db = await database;
    return db.insert("notes", _toMap(note));
  }

  Future<Note> read(int id) async {
    final db = await database;
    final list = await db.query("notes", where: "id=?", whereArgs: [id]);
    return _fromMap(list.first);
  }

  Future<int> update(Note note) async {
    final db = await database;
    return db.update("notes", _toMap(note), where: "id=?", whereArgs: [note.id]);
  }

  Future<int> delete(Note note) async {
    final db = await database;
    return db.delete("notes", where: "id=?", whereArgs: [note.id]);
  }

  Future<List<Note>> loadAll() async {
    final db = await database;
    final list = await db.query("notes");
    return list.map(_fromMap).toList();
  }

  Note _fromMap(Map<String, dynamic> m) {
    return new Note(m["id"], m["title"], m["content"], m["color"]);
  }

  Map<String, dynamic> _toMap(Note note) {
    return {"title" : note.title, "content" : note.text, "color" : note.color};
  }
}
