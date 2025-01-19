class Note {
  int id;
  String title;
  String text;
  String color;

  Note(this.id, this.title, this.text, this.color);

  @override
  String toString() {
    return 'NotesModel{id: $id, title: $title, text: $text, _color: $color}';
  }
}
