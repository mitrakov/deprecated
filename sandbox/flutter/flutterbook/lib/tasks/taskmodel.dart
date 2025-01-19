import 'package:flutterbook/tasks/taskdb.dart';

import '../basemodel.dart';

class TaskModel extends BaseModel {
  void loadData() async {
    this.entityList = await TaskDbWorker.instance.loadAll();
    notifyListeners();
  }

  void setStackIndex(int n) {
    this.index = n;
    notifyListeners();
  }
}

final TaskModel taskModel = new TaskModel();
