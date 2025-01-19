class Contact {
  int id;
  String name;
  String phone;
  String email;
  String birthday;

  Contact(this.id, this.name, this.phone, this.email, this.birthday);

  @override
  String toString() {
    return 'Contact{id: $id, name: $name, phone: $phone, email: $email, birthday: $birthday}';
  }
}
