class Appointment {
  int id;
  String title;
  String description;
  String apptDate;
  String apptTime;

  Appointment(this.id, this.title, this.description, this.apptDate, this.apptTime);

  @override
  String toString() {
    return 'Appointment{id: $id, title: $title, description: $description, apptDate: $apptDate, apptTime: $apptTime}';
  }
}
