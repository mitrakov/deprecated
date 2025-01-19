import 'package:flutter/material.dart';
import 'package:flutterbook/notes/notesmodel.dart';
import 'package:flutterbook/notes/notesentry.dart';
import 'package:flutterbook/notes/noteslist.dart';
import 'package:scoped_model/scoped_model.dart';

class Notes extends StatelessWidget {
  Notes() {
    notesModel.loadData();
  }

  @override
  Widget build(BuildContext context) {
    return ScopedModel<NotesModel>(
      model: notesModel,
      child: ScopedModelDescendant<NotesModel>(
        builder: (context, child, model) {
          return IndexedStack(
            index: model.index,
            children: [
              NotesList(),
              NotesEntry(),
            ],
          );
        },
      ),
    );
  }
}
