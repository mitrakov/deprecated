import 'package:flutter/material.dart';
import 'package:flutter_calendar_carousel/classes/event.dart';
import 'package:flutter_calendar_carousel/classes/event_list.dart';
import 'package:flutter_calendar_carousel/flutter_calendar_carousel.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:flutterbook/appointments/appointment.dart';
import 'package:flutterbook/appointments/appointmentdb.dart';
import 'package:flutterbook/appointments/appointmentmodel.dart';
import 'package:intl/intl.dart';
import 'package:scoped_model/scoped_model.dart';

class AppointmentList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    EventList<Event> _events = new EventList();
    appointmentModel.entityList.forEach((e) {
      final Appointment appt = e;
      final parts = appt.apptDate.split("-");
      final date = new DateTime(int.parse(parts[0]), int.parse(parts[1]), int.parse(parts[2]));
      _events.add(date, new Event(date: date, icon: Container(decoration: BoxDecoration(color: Colors.blue))));
    });



    return ScopedModel<AppointmentModel>(
      model: appointmentModel,
      child: ScopedModelDescendant<AppointmentModel>(
        builder: (context, child, model) {
          return Scaffold(
            floatingActionButton: FloatingActionButton(
              child: Icon(Icons.add),
              onPressed: () {
                final now = DateTime.now();
                model.entityBeingEdited = new Appointment(-1, "", "", "${now.year}-${now.month}-${now.day}", null);
                model.date = DateFormat.yMMMMd("en_US").format(now);
                model.index = 1;
              },
            ),
            body: Column(
              children: [
                Expanded(
                  child: Container(
                    margin: EdgeInsets.symmetric(horizontal: 10),
                    child: CalendarCarousel(
                      thisMonthDayBorderColor: Colors.grey,
                      daysHaveCircularBorder: false,
                      markedDatesMap: _events,
                      onDayPressed: (date, events) => _showAppointment(context, date),
                    ),
                  )
                ),
              ],
            ),
          );
        },
      )
    );
  }

  void _showAppointment(BuildContext context, DateTime date) {
    showModalBottomSheet(context: context, builder: (context) {
      return ScopedModel<AppointmentModel>(
        model: appointmentModel,
        child: ScopedModelDescendant<AppointmentModel>(
          builder: (context, child, model) {
            return Scaffold(
              body: Container(
                child: Padding(
                  padding: EdgeInsets.all(10),
                  child: GestureDetector(
                    child: Column(
                      children: [
                        Text(DateFormat.yMMMMd("en_US").format(date),
                          textAlign: TextAlign.center,
                          style: TextStyle(color: Theme.of(context).accentColor, fontSize: 24)
                        ),
                        Divider(),
                        Expanded(
                          child: ListView.builder(
                            itemCount: model.entityList.length,
                            itemBuilder: (context, idx) {
                              final Appointment appt = model.entityList[idx];
                              if (appt.apptDate != "${date.year}-${date.month}-${date.day}")
                                return Container(height: 0);
                              String time = "";
                              if (appt.apptTime != null) {
                                final parts = appt.apptTime.split("-");
                                final at = TimeOfDay(hour: int.parse(parts[0]), minute: int.parse(parts[1]));
                                time = at.format(context);
                              }
                              return Slidable(
                                actionExtentRatio: 0.25,
                                child: Container(
                                  margin: EdgeInsets.only(bottom: 8),
                                  color: Colors.grey.shade300,
                                  child: ListTile(
                                    title: Text("${appt.apptDate} ${appt.apptTime}"),
                                    subtitle: Text(appt.description),
                                    onTap: () => _editAppointment(context, appt),
                                  ),
                                ),
                                actionPane: Text("Action Pane"),
                                secondaryActions: [
                                  IconSlideAction(
                                    caption: "Delete",
                                    color: Colors.red,
                                    icon: Icons.delete,
                                    onTap: () => _delete(context, appt),
                                  )
                                ],
                              );
                            },
                          )
                        )
                      ],
                    ),
                  ),
                ),
              ),
            );
          }
        )
      );
    });
  }

  void _editAppointment(BuildContext context, Appointment appt) async {
    appointmentModel.entityBeingEdited = await AppointmentDbWorker.instance.read(appt.id);
    if (appointmentModel.entityBeingEdited.apptDate == null)
      appointmentModel.date = null;
    else {
      final parts = appointmentModel.date.split("-");
      final date = new DateTime(int.parse(parts[0]), int.parse(parts[1]), int.parse(parts[2]));
      appointmentModel.date = DateFormat.yMMMd("en_US").format(date);
    }
    if (appointmentModel.entityBeingEdited.apptTime == null)
      appointmentModel.time = null;
    else {
      final parts = appointmentModel.time.split("-");
      final time = new TimeOfDay(hour: int.parse(parts[0]), minute: int.parse(parts[1]));
      appointmentModel.time = time.format(context);
    }
    appointmentModel.index = 1;
    Navigator.pop(context);
  }

  void _delete(BuildContext context, Appointment appointment) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) {
        return AlertDialog(
          title: Text(appointment.id.toString()),
          content: Text(appointment.description),
          actions: [
            FlatButton(onPressed: () => Navigator.pop(context), child: Text("Cancel")),
            FlatButton(
                onPressed: () async {
                  await AppointmentDbWorker.instance.delete(appointment);
                  Navigator.pop(context);
                  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                    content: Text("Appointment ${appointment.description} has been removed"),
                    backgroundColor: Colors.red,
                    duration: Duration(seconds: 2),
                  ));
                  appointmentModel.loadData();
                },
                child: Text("Delete")
            )
          ],
        );
      },
    );
  }
}
