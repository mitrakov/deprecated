import 'package:flutter/material.dart';
import 'package:flutterbook/tasks/taskentry.dart';
import 'package:flutterbook/tasks/tasklist.dart';
import 'package:flutterbook/tasks/taskmodel.dart';
import 'package:scoped_model/scoped_model.dart';

class Tasks extends StatelessWidget {
  Tasks() {
    taskModel.loadData();
  }

  @override
  Widget build(BuildContext context) {
    return ScopedModel<TaskModel>(
      model: taskModel,
      child: ScopedModelDescendant<TaskModel>(
        builder: (context, child, model) {
          return IndexedStack(
            index: model.index,
            children: [
              TaskList(),
              TaskEntry(),
            ],
          );
        },
      ),
    );
  }
}
