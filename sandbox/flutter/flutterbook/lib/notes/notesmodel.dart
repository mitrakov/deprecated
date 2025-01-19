import '../basemodel.dart';
import 'notesdb.dart';

class NotesModel extends BaseModel {
  String color;

  void loadData() async {
    this.entityList = await NotesDbWorker.instance.loadAll();
    notifyListeners();
  }

  void setColour(String colour) {
    this.color = colour;
    notifyListeners();
  }

  void setStackIndex(int n) {
    this.index = n;
    notifyListeners();
  }
}

final NotesModel notesModel = new NotesModel();
