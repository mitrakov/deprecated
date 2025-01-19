import 'package:scoped_model/scoped_model.dart';

class BaseModel extends Model {
  int _index = 0;
  List _entityList = [];
  var _entityBeingEdited;
  String _date;

  @override
  String toString() {
    return 'BaseModel{index: $_index, data: $_entityList, entityBeingEdited: $_entityBeingEdited, date: $_date}';
  }

  set date(String value) {
    _date = value;
    notifyListeners();
  }

  set entityBeingEdited(value) {
    _entityBeingEdited = value;
    notifyListeners();
  }

  set index(int value) {
    _index = value;
    notifyListeners();
  }

  set entityList(List value) {
    _entityList = value;
    notifyListeners();
  }

  int get index => _index;

  List get entityList => _entityList;

  get entityBeingEdited => _entityBeingEdited;

  String get date => _date;
}
