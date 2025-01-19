import 'dart:io';
import 'package:flutterbook/utils.dart';
import 'package:path/path.dart';
import 'package:flutter/material.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:flutterbook/contacts/contact.dart';
import 'package:flutterbook/contacts/contactdb.dart';
import 'package:flutterbook/contacts/contactmodel.dart';
import 'package:intl/intl.dart';
import 'package:scoped_model/scoped_model.dart';

class ContactsList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<ContactModel>(
      model: contactsModel,
      child: ScopedModelDescendant<ContactModel>(
        builder: (context, child, model) {
          return Scaffold(
            floatingActionButton: FloatingActionButton(
              child: Icon(Icons.add),
              onPressed: () {
                final file = File(join(Utils.docsDir.path, "avatar"));
                if (file.existsSync())
                  file.deleteSync();
                model.entityBeingEdited = new Contact(-1, "", "", "", null);
                model.date = null;
                model.index = 1;
              },
            ),
            body: ListView.builder(
              itemCount: contactsModel.entityList.length,
              itemBuilder: (context, index) {
                final Contact contact = model.entityList[index];
                final file = File(join(Utils.docsDir.path, contact.id.toString()));
                final exists = file.existsSync();
                return Column(
                  children: [
                    Slidable(
                      actionExtentRatio: 0.25,
                      child: ListTile(
                        leading: CircleAvatar(
                          backgroundColor: Colors.indigoAccent,
                          foregroundColor: Colors.white,
                          backgroundImage: exists ? FileImage(file) : null,
                          child: exists ? null : Text(contact.name.substring(0, 1).toUpperCase()),
                        ),
                        title: Text(contact.name),
                        subtitle: contact.phone == null ? null : Text(contact.phone),
                        onTap: () async {
                          final File file = File(join(Utils.docsDir.path, "avatar"));
                          if (file.existsSync())
                            file.deleteSync();
                          model.entityBeingEdited = await ContactDbWorker.instance.read(contact.id);
                          if (model.entityBeingEdited.birthday == "") {
                            model.date = null;
                          } else {
                            final List parts = model.entityBeingEdited.birthday.split("-");
                            final birthday = new DateTime(int.parse(parts[0]), int.parse(parts[1]), int.parse(parts[2]));
                            model.date = DateFormat.yMMMMd("en_US").format(birthday);
                            model.index = 1;
                          }
                        },
                      ),
                      actionPane: Text("Action Pane"),
                      secondaryActions: [
                        IconSlideAction(
                          caption: "Delete",
                          color: Colors.red,
                          icon: Icons.delete,
                          onTap: () => _delete(context, contact),
                        )
                      ],
                    ),
                    Divider(),
                  ],
                );
              },
            ),
          );
        },
      )
    );
  }



  Future _delete(BuildContext context, Contact contact) {
    return showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) {
        return AlertDialog(
          title: Text("Delete contact"),
          content: Text("Are you sure to delete ${contact.name}?"),
          actions: [
            FlatButton(onPressed: () => Navigator.pop(context), child: Text("Cancel")),
            FlatButton(
                onPressed: () async {
                  final File file = File(join(Utils.docsDir.path, contact.id.toString()));
                  if (file.existsSync())
                    file.deleteSync();
                  await ContactDbWorker.instance.delete(contact);
                  Navigator.pop(context);
                  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                    content: Text("Contact ${contact.name} has been removed"),
                    backgroundColor: Colors.red,
                    duration: Duration(seconds: 2),
                  ));
                  contactsModel.loadData();
                },
                child: Text("Delete")
            )
          ],
        );
      },
    );
  }
}
