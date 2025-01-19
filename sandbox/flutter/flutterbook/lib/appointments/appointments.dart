import 'package:flutter/material.dart';
import 'package:flutterbook/appointments/appointmententry.dart';
import 'package:flutterbook/appointments/appointmentlist.dart';
import 'package:flutterbook/appointments/appointmentmodel.dart';
import 'package:scoped_model/scoped_model.dart';

class Appointments extends StatelessWidget {
  Appointments() {
    appointmentModel.loadData();
  }

  @override
  Widget build(BuildContext context) {
    return ScopedModel<AppointmentModel>(
      model: appointmentModel,
      child: ScopedModelDescendant<AppointmentModel>(
        builder: (context, child, model) {
          return IndexedStack(
            index: model.index,
            children: [
              AppointmentList(),
              AppointmentEntry(),
            ],
          );
        },
      ),
    );
  }
}
