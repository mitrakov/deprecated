import 'package:flutter/material.dart';
import 'package:flutterbook/appointments/appointmentdb.dart';
import 'package:flutterbook/appointments/appointmentmodel.dart';
import 'package:flutterbook/utils.dart';
import 'package:scoped_model/scoped_model.dart';

class AppointmentEntry extends StatelessWidget {
  final _titleCtrl = TextEditingController();
  final _descriptionCtrl = TextEditingController();
  final _key = GlobalKey<FormState>();

  AppointmentEntry() {
    _titleCtrl.addListener(() => appointmentModel.entityBeingEdited.title = _titleCtrl.text);
    _descriptionCtrl.addListener(() => appointmentModel.entityBeingEdited.description = _descriptionCtrl.text);
  }

  @override
  Widget build(BuildContext context) {
    _titleCtrl.text = appointmentModel.entityBeingEdited?.title;
    _descriptionCtrl.text = appointmentModel.entityBeingEdited?.description;
    return ScopedModel<AppointmentModel>(
      model: appointmentModel,
      child: ScopedModelDescendant<AppointmentModel>(
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
                    leading: Icon(Icons.description_outlined),
                    title: TextFormField(
                      decoration: InputDecoration(hintText: "Description"),
                      controller: _descriptionCtrl,
                      validator: (value) {
                        if (value.isEmpty) return "Please input description";
                        return null;
                      },
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.today),
                    title: Text("Appointment Date"),
                    subtitle: Text(appointmentModel.date ?? ""),
                    trailing: IconButton(
                      icon: Icon(Icons.edit),
                      color: Colors.blue,
                      onPressed: () async {
                        final date = await Utils.pickDate(context, appointmentModel.entityBeingEdited.apptDate);
                        if (date != null) {
                          appointmentModel.date = date;
                          appointmentModel.entityBeingEdited.apptDate = date;
                        }
                      },
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.alarm),
                    title: Text("Time"),
                    subtitle: Text(appointmentModel.time ?? ""),
                    trailing: IconButton(
                      icon: Icon(Icons.edit),
                      color: Colors.blue,
                      onPressed: () => _selectTime(context),
                    ),
                  ),
                ],
              ),
            ),
          );
        },
      )
    );
  }

  void _selectTime(BuildContext context) async {
    var time = TimeOfDay.now();
    if (appointmentModel.entityBeingEdited.apptTime != null) {
      final parts = appointmentModel.entityBeingEdited.apptTime.split("-");
      time = TimeOfDay(hour: int.parse(parts[0]), minute: int.parse(parts[1]));
    }
    var picked = await showTimePicker(context: context, initialTime: time);
    if (picked != null) {
      appointmentModel.entityBeingEdited.apptTime = "${picked.hour}-${picked.minute}";
      appointmentModel.time = picked.format(context);
    }
  }

  void _save(BuildContext context, AppointmentModel model) async {
    if (!_key.currentState.validate()) return;
    if (model.entityBeingEdited.id == -1) {
      print("Creating ${model.entityBeingEdited}");
      await AppointmentDbWorker.instance.create(model.entityBeingEdited);
    } else {
      print("Updating ${model.entityBeingEdited}");
      await AppointmentDbWorker.instance.update(model.entityBeingEdited);
    }
    model.loadData();
    model.setStackIndex(0);
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      backgroundColor: Colors.grey,
      duration: Duration(seconds: 2),
      content: Text("Appointment saved")
    ));
  }
}
