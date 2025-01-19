import 'package:flutter/material.dart';
import 'package:flutterbook/tasks/taskdb.dart';
import 'package:flutterbook/tasks/taskmodel.dart';
import 'package:flutterbook/utils.dart';
import 'package:scoped_model/scoped_model.dart';

class TaskEntry extends StatelessWidget {
  final _titleCtrl = TextEditingController();
  final _key = GlobalKey<FormState>();

  TaskEntry() {
    _titleCtrl.addListener(() => taskModel.entityBeingEdited.description = _titleCtrl.text);
  }

  @override
  Widget build(BuildContext context) {
    _titleCtrl.text = taskModel.entityBeingEdited?.description;
    return ScopedModel<TaskModel>(
        model: taskModel,
        child: ScopedModelDescendant<TaskModel>(
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
                      leading: Icon(Icons.today),
                      title: Text("Due Date"),
                      subtitle: Text(taskModel.date ?? ""),
                      trailing: IconButton(
                        icon: Icon(Icons.edit),
                        color: Colors.blue,
                        onPressed: () async {
                          final date = await Utils.pickDate(context, taskModel.entityBeingEdited.dueDate);
                          if (date != null) {
                            taskModel.date = date;
                            taskModel.entityBeingEdited.dueDate = date;
                          }
                        },
                      ),
                    ),
                  ],
                ),
              ),
            );
          },
        ));
  }

  void _save(BuildContext context, TaskModel model) async {
    if (!_key.currentState.validate()) return;
    if (model.entityBeingEdited.id == -1) {
      print("Creating ${model.entityBeingEdited}");
      await TaskDbWorker.instance.create(model.entityBeingEdited);
    } else {
      print("Updating ${model.entityBeingEdited}");
      await TaskDbWorker.instance.update(model.entityBeingEdited);
    }
    taskModel.loadData();
    model.setStackIndex(0);
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      backgroundColor: Colors.grey,
      duration: Duration(seconds: 2),
      content: Text("Task saved")
    ));
  }
}
