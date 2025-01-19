import 'package:flutterbook/contacts/contact.dart';
import 'package:sqflite/sqflite.dart';

class ContactDbWorker {
  ContactDbWorker._();
  static final ContactDbWorker instance = ContactDbWorker._();

  Database _db;

  Future<Database> get database async {
    if (_db == null)
      _db = await init();
    return _db;
  }

  Future<Database> init() {
    return openDatabase(
      "db/contact.db",
      version: 1,
      onCreate: (db, version) {
        db.execute("CREATE TABLE IF NOT EXISTS contact (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, phone TEXT, email TEXT, birthday TEXT);");
      }
    );
  }

  Future<int> create(Contact contact) async {
    final db = await database;
    return db.insert("contact", _toMap(contact));
  }

  Future<Contact> read(int id) async {
    final db = await database;
    final list = await db.query("contact", where: "id=?", whereArgs: [id]);
    return _fromMap(list.first);
  }

  Future<int> update(Contact contact) async {
    final db = await database;
    return db.update("contact", _toMap(contact), where: "id=?", whereArgs: [contact.id]);
  }

  Future<int> delete(Contact contact) async {
    final db = await database;
    return db.delete("contact", where: "id=?", whereArgs: [contact.id]);
  }

  Future<List<Contact>> loadAll() async {
    final db = await database;
    final list = await db.query("contact");
    return list.map(_fromMap).toList();
  }

  Contact _fromMap(Map<String, dynamic> m) {
    return new Contact(m["id"], m["name"], m["phone"], m["email"], m["birthday"]);
  }

  Map<String, dynamic> _toMap(Contact contact) {
    return {"name": contact.name, "phone" : contact.phone, "email" : contact.email, "birthday" : contact.birthday};
  }
}
