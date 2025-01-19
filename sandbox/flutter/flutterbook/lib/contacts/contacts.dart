import 'package:flutter/material.dart';
import 'package:flutterbook/contacts/contactentry.dart';
import 'package:flutterbook/contacts/contactlist.dart';
import 'package:flutterbook/contacts/contactmodel.dart';
import 'package:scoped_model/scoped_model.dart';

class Contacts extends StatelessWidget {
  Contacts() {
    contactsModel.loadData();
  }

  @override
  Widget build(BuildContext context) {
    return ScopedModel<ContactModel>(
      model: contactsModel,
      child: ScopedModelDescendant<ContactModel>(
        builder: (context, child, model) {
          return IndexedStack(
            index: model.index,
            children: [
              ContactsList(),
              ContactEntry(),
            ],
          );
        },
      ),
    );
  }
}
