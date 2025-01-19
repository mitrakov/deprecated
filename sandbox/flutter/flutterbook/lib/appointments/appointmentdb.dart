import 'package:flutterbook/appointments/appointment.dart';
import 'package:sqflite/sqflite.dart';

class AppointmentDbWorker {
  AppointmentDbWorker._();
  static final AppointmentDbWorker instance = AppointmentDbWorker._();

  Database _db;

  Future<Database> get database async {
    if (_db == null)
      _db = await init();
    return _db;
  }

  Future<Database> init() {
    return openDatabase(
      "db/appt.db",
      version: 1,
      onCreate: (db, version) {
        db.execute("CREATE TABLE IF NOT EXISTS appointment (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, description TEXT, appt_date TEXT, appt_time TEXT);");
      }
    );
  }

  Future<int> create(Appointment appointment) async {
    final db = await database;
    return db.insert("appointment", _toMap(appointment));
  }

  Future<Appointment> read(int id) async {
    final db = await database;
    final list = await db.query("appointment", where: "id=?", whereArgs: [id]);
    return _fromMap(list.first);
  }

  Future<int> update(Appointment appointment) async {
    final db = await database;
    return db.update("appointment", _toMap(appointment), where: "id=?", whereArgs: [appointment.id]);
  }

  Future<int> delete(Appointment appointment) async {
    final db = await database;
    return db.delete("appointment", where: "id=?", whereArgs: [appointment.id]);
  }

  Future<List<Appointment>> loadAll() async {
    final db = await database;
    final list = await db.query("appointment");
    return list.map(_fromMap).toList();
  }

  Appointment _fromMap(Map<String, dynamic> m) {
    return new Appointment(m["id"], m["title"], m["description"], m["appt_date"], m["appt_time"]);
  }

  Map<String, dynamic> _toMap(Appointment appointment) {
    return {"title": appointment.title, "description" : appointment.description, "appt_date" : appointment.apptDate, "appt_time" : appointment.apptTime};
  }
}
