class Task {
  int id;
  String description;
  String dueDate;
  String completed = "false";

  Task(this.id, this.description, this.dueDate, this.completed);

  @override
  String toString() {
    return 'Note{id: $id, description: $description, dueDate: $dueDate, completed: $completed}';
  }
}
