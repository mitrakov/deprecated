import 'package:flutterbook/utils.dart';
import 'package:flutter/material.dart';
import 'package:flutterbook/appointments/appointments.dart';
import 'package:flutterbook/contacts/contacts.dart';
import 'package:flutterbook/notes/notes.dart';
import 'package:flutterbook/tasks/tasks.dart';
import 'package:path_provider/path_provider.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final docsDir = await getApplicationDocumentsDirectory();
  Utils.docsDir = docsDir;
  runApp(FlutterBook());
}

class FlutterBook extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Book',
      theme: ThemeData(
        primarySwatch: Colors.green,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: DefaultTabController(
        length: 4,
        child: Scaffold(
          appBar: AppBar(
            title: Text("Flutter Book"),
            bottom: TabBar(
              labelColor: Colors.black54,
              tabs: [
                Tab(icon: Icon(Icons.calendar_today), text: "Appointments"),
                Tab(icon: Icon(Icons.contact_mail), text: "Contacts"),
                Tab(icon: Icon(Icons.note), text: "Notes"),
                Tab(icon: Icon(Icons.transfer_within_a_station), text: "Tasks"),
              ],
            ),
          ),
          body: TabBarView(
            children: [
              Appointments(),
              Contacts(),
              Notes(),
              Tasks(),
            ],
          ),
        ),
      ),
    );
  }
}
