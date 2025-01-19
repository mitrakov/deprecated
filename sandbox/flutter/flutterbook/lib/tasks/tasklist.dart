import 'package:flutter/material.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:flutterbook/tasks/task.dart';
import 'package:flutterbook/tasks/taskdb.dart';
import 'package:flutterbook/tasks/taskmodel.dart';
import 'package:intl/intl.dart';
import 'package:scoped_model/scoped_model.dart';

class TaskList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<TaskModel>(
      model: taskModel,
      child: ScopedModelDescendant<TaskModel>(
        builder: (context, child, model) {
          return Scaffold(
            floatingActionButton: FloatingActionButton(
              child: Icon(Icons.add),
              onPressed: () {
                model.entityBeingEdited = new Task(-1, "", null, "false");
                model.index = 1;
              },
            ),
            body: ListView.builder(
              padding: EdgeInsets.fromLTRB(0, 10, 0, 0),
              itemCount: taskModel.entityList.length,
              itemBuilder: (context, idx) {
                final Task task = model.entityList[idx];
                String dueDateStr;
                if (task.dueDate != null) {
                  List dateParts = task.dueDate.split("-");
                  DateTime dueDate = new DateTime(int.parse(dateParts[0]), int.parse(dateParts[1]), int.parse(dateParts[2]));
                  dueDateStr = DateFormat.yMMMMd("en_US").format(dueDate);
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
                        onTap: () => _delete(context, task),
                      )
                    ],
                    child: ListTile(
                      leading: Checkbox(
                          value: task.completed == "true",
                        onChanged: (value) async {
                            task.completed = value.toString();
                            await TaskDbWorker.instance.update(task);
                            taskModel.loadData();
                        },
                      ),
                      title: Text("${task.description}",
                        style: task.completed == "true"
                            ? TextStyle(color: Theme.of(context).disabledColor, decoration: TextDecoration.lineThrough)
                            : TextStyle(color: Theme.of(context).textTheme.headline6.color)
                      ),
                      subtitle: task.dueDate == null ? null : Text(dueDateStr,
                          style: task.completed == "true"
                              ? TextStyle(color: Theme.of(context).disabledColor, decoration: TextDecoration.lineThrough)
                              : TextStyle(color: Theme.of(context).textTheme.headline6.color)
                      ),
                      onTap: () async {
                        if (task.completed == "true") return;
                        taskModel.entityBeingEdited = await TaskDbWorker.instance.read(task.id);
                        taskModel.date = taskModel.entityBeingEdited.dueDate == null ? null : dueDateStr;
                        taskModel.index = 1;
                      },
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

  void _delete(BuildContext context, Task task) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) {
        return AlertDialog(
          title: Text(task.id.toString()),
          content: Text(task.description),
          actions: [
            FlatButton(onPressed: () => Navigator.pop(context), child: Text("Cancel")),
            FlatButton(
                onPressed: () async {
                  await TaskDbWorker.instance.delete(task);
                  Navigator.pop(context);
                  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                    content: Text("Task ${task.description} has been removed"),
                    backgroundColor: Colors.red,
                    duration: Duration(seconds: 2),
                  ));
                  taskModel.loadData();
                },
                child: Text("Delete")
            )
          ],
        );
      },
    );
  }
}
