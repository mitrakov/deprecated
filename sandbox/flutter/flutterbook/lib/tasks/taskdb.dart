import 'package:sqflite/sqflite.dart';
import 'task.dart';

class TaskDbWorker {
  TaskDbWorker._();
  static final TaskDbWorker instance = TaskDbWorker._();

  Database _db;

  Future<Database> get database async {
    if (_db == null)
      _db = await init();
    return _db;
  }

  Future<Database> init() {
    return openDatabase(
      "db/tasks.db",
      version: 1,
      onCreate: (db, version) {
        db.execute("CREATE TABLE IF NOT EXISTS tasks (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT, due_date TEXT, completed TEXT);");
      }
    );
  }

  Future<int> create(Task task) async {
    final db = await database;
    return db.insert("tasks", _toMap(task));
  }

  Future<Task> read(int id) async {
    final db = await database;
    final list = await db.query("tasks", where: "id=?", whereArgs: [id]);
    return _fromMap(list.first);
  }

  Future<int> update(Task task) async {
    final db = await database;
    return db.update("tasks", _toMap(task), where: "id=?", whereArgs: [task.id]);
  }

  Future<int> delete(Task task) async {
    final db = await database;
    return db.delete("tasks", where: "id=?", whereArgs: [task.id]);
  }

  Future<List<Task>> loadAll() async {
    final db = await database;
    final list = await db.query("tasks");
    return list.map(_fromMap).toList();
  }

  Task _fromMap(Map<String, dynamic> m) {
    return new Task(m["id"], m["description"], m["due_date"], m["completed"]);
  }

  Map<String, dynamic> _toMap(Task task) {
    return {"description" : task.description, "due_date" : task.dueDate, "completed" : task.completed};
  }
}
