import 'package:flutterbook/contacts/contactdb.dart';
import '../basemodel.dart';

class ContactModel extends BaseModel {

  void loadData() async {
    this.entityList = await ContactDbWorker.instance.loadAll();
    notifyListeners();
  }

  void setStackIndex(int n) {
    this.index = n;
    notifyListeners();
  }

  void rebuild() {
    notifyListeners();
  }
}

final ContactModel contactsModel = new ContactModel();
