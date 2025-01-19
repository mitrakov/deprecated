import 'package:flutter/material.dart';
import 'package:flutterbook/notes/notesmodel.dart';
import 'package:flutterbook/notes/notesdb.dart';
import 'package:scoped_model/scoped_model.dart';

class NotesEntry extends StatelessWidget {
  final _titleCtrl = TextEditingController();
  final _contentCtrl = TextEditingController();
  final _key = GlobalKey<FormState>();


  NotesEntry() {
    _titleCtrl.addListener(() => notesModel.entityBeingEdited.title = _titleCtrl.text);
    _contentCtrl.addListener(() => notesModel.entityBeingEdited.text = _contentCtrl.text);
  }

  @override
  Widget build(BuildContext context) {
    _titleCtrl.text = notesModel.entityBeingEdited?.title;
    _contentCtrl.text = notesModel.entityBeingEdited?.text;
    final width = 15.0;
    return ScopedModel<NotesModel>(
        model: notesModel,
        child: ScopedModelDescendant<NotesModel>(
          builder: (context, child, model) {
            return Scaffold(
              bottomNavigationBar: Padding(
                padding: EdgeInsets.symmetric(horizontal: 10),
                child: Row(
                  children: [
                    FlatButton(
                      child: Text("OK"),
                      onPressed: () {
                        _save(context, model);
                      },
                    ),
                    Spacer(),
                    FlatButton(
                      child: Text("Cancel"),
                      onPressed: () {
                        FocusScope.of(context).requestFocus(FocusNode());
                        model.setStackIndex(0);
                      },
                    ),
                  ],
                ),
              ),
              body: Form(
                key: _key,
                child: ListView(
                  children: [
                    ListTile(
                      leading: Icon(Icons.title),
                      title: TextFormField(
                        decoration: InputDecoration(hintText: "Title"),
                        controller: _titleCtrl,
                        validator: (value) {
                          if (value.isEmpty) return "Please input a title";
                          return null;
                        },
                      ),
                    ),
                    ListTile(
                      leading: Icon(Icons.content_paste),
                      title: TextFormField(
                        keyboardType: TextInputType.multiline,
                        maxLines: 8,
                        decoration: InputDecoration(hintText: "Content"),
                        controller: _contentCtrl,
                        validator: (value) {
                          if (value.isEmpty) return "Please input content";
                          return null;
                        },
                      ),
                    ),
                    ListTile(
                      leading: Icon(Icons.color_lens),
                      title: Row(
                        children: [
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                shape:
                                    Border.all(width: width, color: Colors.red) +
                                    Border.all(width: 6, color: notesModel.color == "red" ? Colors.red : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "red";
                              notesModel.setColour("red");
                            },
                          ),
                          Spacer(),
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                  shape:
                                  Border.all(width: width, color: Colors.green) +
                                      Border.all(width: 6, color: notesModel.color == "green" ? Colors.green : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "green";
                              notesModel.setColour("green");
                            },
                          ),
                          Spacer(),
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                  shape:
                                  Border.all(width: width, color: Colors.blue) +
                                      Border.all(width: 6, color: notesModel.color == "blue" ? Colors.blue : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "blue";
                              notesModel.setColour("blue");
                            },
                          ),
                          Spacer(),
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                  shape:
                                  Border.all(width: width, color: Colors.yellow) +
                                      Border.all(width: 6, color: notesModel.color == "yellow" ? Colors.yellow : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "yellow";
                              notesModel.setColour("yellow");
                            },
                          ),
                          Spacer(),
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                  shape:
                                  Border.all(width: width, color: Colors.brown) +
                                      Border.all(width: 6, color: notesModel.color == "brown" ? Colors.brown : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "brown";
                              notesModel.setColour("brown");
                            },
                          ),
                          Spacer(),
                          GestureDetector(
                            child: Container(
                              decoration: ShapeDecoration(
                                  shape:
                                  Border.all(width: width, color: Colors.orange) +
                                      Border.all(width: 6, color: notesModel.color == "orange" ? Colors.orange : Theme.of(context).canvasColor)
                              ),
                            ),
                            onTap: () {
                              notesModel.entityBeingEdited.color = "orange";
                              notesModel.setColour("orange");
                            },
                          ),
                        ],
                      ),
                    )
                  ],
                ),

              ),
            );
          },
        ));
  }

  void _save(BuildContext context, NotesModel model) async {
    if (!_key.currentState.validate()) return;
    if (model.entityBeingEdited.id == -1) {
      print("Creating ${model.entityBeingEdited}");
      await NotesDbWorker.instance.create(model.entityBeingEdited);
    } else {
      print("Updating ${model.entityBeingEdited}");
      await NotesDbWorker.instance.update(model.entityBeingEdited);
    }
    notesModel.loadData();
    model.setStackIndex(0);
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      backgroundColor: Colors.grey,
      duration: Duration(seconds: 2),
      content: Text("Note saved")
    ));
  }

}
