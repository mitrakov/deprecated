import 'package:flutter/material.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:flutterbook/notes/note.dart';
import 'package:flutterbook/notes/notesmodel.dart';
import 'package:flutterbook/notes/notesdb.dart';
import 'package:scoped_model/scoped_model.dart';

class NotesList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<NotesModel>(
      model: notesModel,
      child: ScopedModelDescendant<NotesModel>(
        builder: (context, child, model) {
          return Scaffold(
            floatingActionButton: FloatingActionButton(
              child: Icon(Icons.add),
              onPressed: () {
                model.entityBeingEdited = Note(-1, "", "", "red");
                model.index = 1;
                model.color = null;
              },
            ),
            body: ListView.builder(
              itemCount: notesModel.entityList.length,
              itemBuilder: (context, idx) {
                final Note note = model.entityList[idx];
                Color colour;
                switch(note.color) {
                  case "red": colour = Colors.red; break;
                  case "green": colour = Colors.green; break;
                  case "blue": colour = Colors.blue; break;
                  case "yellow": colour = Colors.yellow; break;
                  case "brown": colour = Colors.brown; break;
                  case "orange": colour = Colors.orange; break;
                  default: colour = Colors.white;
                }
                return Container(
                  padding: EdgeInsets.fromLTRB(20, 20, 20, 0),
                  child: Slidable(
                    actionPane: Text("Action Pane"),
                    actionExtentRatio: .25,
                    secondaryActions: [
                      IconSlideAction(
                        icon: Icons.delete,
                        caption: "Delete",
                        color: Colors.red,
                        onTap: () => _delete(context, note),
                      )
                    ],
                    child: Card(
                      elevation: 8,
                      color: colour,
                      child: ListTile(
                        title: Text(note.title),
                        subtitle: Text(note.text),
                        onTap: () async {
                          model.entityBeingEdited = await NotesDbWorker.instance.read(note.id);
                          model.color = model.entityBeingEdited.color;
                          model.index = 1;
                        },
                      ),
                    )
                  )
                );
              },
            ),
          );
        },
      )
    );
  }

  void _delete(BuildContext context, Note note) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) {
        return AlertDialog(
          title: Text(note.title),
          content: Text(note.text),
          actions: [
            FlatButton(onPressed: () => Navigator.pop(context), child: Text("Cancel")),
            FlatButton(
                onPressed: () async {
                  await NotesDbWorker.instance.delete(note);
                  Navigator.pop(context);
                  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                    content: Text("Note ${note.title} has been removed"),
                    backgroundColor: Colors.red,
                    duration: Duration(seconds: 2),
                  ));
                  notesModel.loadData();
                },
                child: Text("Delete")
            )
          ],
        );
      },
    );
  }
}
