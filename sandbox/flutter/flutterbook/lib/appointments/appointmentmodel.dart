import 'package:flutterbook/appointments/appointmentdb.dart';
import '../basemodel.dart';

class AppointmentModel extends BaseModel {
  String _time;

  void loadData() async {
    this.entityList = await AppointmentDbWorker.instance.loadAll();
    notifyListeners();
  }

  void setStackIndex(int n) {
    this.index = n;
    notifyListeners();
  }

  String get time => _time;

  set time(String value) {
    this._time = value;
    notifyListeners();
  }
}

final AppointmentModel appointmentModel = new AppointmentModel();
