import 'dart:io';
import 'package:path/path.dart';
import 'package:flutter/material.dart';
import 'package:flutterbook/contacts/contactdb.dart';
import 'package:flutterbook/contacts/contactmodel.dart';
import 'package:flutterbook/utils.dart';
import 'package:image_picker/image_picker.dart';
import 'package:scoped_model/scoped_model.dart';

class ContactEntry extends StatelessWidget {
  final _nameCtrl = TextEditingController();
  final _phoneCtrl = TextEditingController();
  final _emailCtrl = TextEditingController();
  final _key = GlobalKey<FormState>();

  ContactEntry() {
    _nameCtrl.addListener(() => contactsModel.entityBeingEdited.name = _nameCtrl.text);
    _phoneCtrl.addListener(() => contactsModel.entityBeingEdited.phone = _phoneCtrl.text);
    _emailCtrl.addListener(() => contactsModel.entityBeingEdited.email = _emailCtrl.text);
  }

  @override
  Widget build(BuildContext context) {
    _nameCtrl.text = contactsModel.entityBeingEdited?.name;
    _phoneCtrl.text = contactsModel.entityBeingEdited?.phone;
    _emailCtrl.text = contactsModel.entityBeingEdited?.email;
    return ScopedModel<ContactModel>(
      model: contactsModel,
      child: ScopedModelDescendant<ContactModel>(
        builder: (context, child, model) {
          File file = File(join(Utils.docsDir.path, "avatar"));
          if (!file.existsSync()) {
            if (model.entityBeingEdited?.id != null) {
              file = File(join(Utils.docsDir.path, model.entityBeingEdited.id.toString()));
            }
          }
          return Scaffold(
            bottomNavigationBar: Padding(
              padding: EdgeInsets.symmetric(horizontal: 10),
              child: Row(
                children: [
                  FlatButton(
                    child: Text("Save"),
                    onPressed: () {
                      _save(context, model);
                    },
                  ),
                  Spacer(),
                  FlatButton(
                    child: Text("Cancel"),
                    onPressed: () {
                      final File file = File(join(Utils.docsDir.path, "avatar"));
                      if (file.existsSync())
                        file.deleteSync();
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
                    title: file.existsSync() ? Image.file(file) : Text("No avatar"),
                    trailing: IconButton(icon: Icon(Icons.edit), color: Colors.blue, onPressed: () => _selectAvatar(context)),
                  ),
                  ListTile(
                    leading: Icon(Icons.person),
                    title: TextFormField(
                      decoration: InputDecoration(hintText: "Name"),
                      controller: _nameCtrl,
                      validator: (value) {
                        if (value.isEmpty) return "Please enter a name";
                        return null;
                      },
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.phone),
                    title: TextFormField(
                      keyboardType: TextInputType.phone,
                      decoration: InputDecoration(hintText: "Phone"),
                      controller: _phoneCtrl,
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.email),
                    title: TextFormField(
                      keyboardType: TextInputType.emailAddress,
                      decoration: InputDecoration(hintText: "Email"),
                      controller: _emailCtrl,
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.today),
                    title: Text("Birthday"),
                    subtitle: Text(model.date == null ? "-" : model.date),
                    trailing: IconButton(
                      icon: Icon(Icons.edit),
                      color: Colors.blue,
                      onPressed: () async {
                        final date = await Utils.pickDate(context, model.entityBeingEdited.birthday);
                        if (date != null) {
                          model.entityBeingEdited.birthday = date;
                          model.date = date;
                        }
                      },
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

  Future<int> _selectAvatar(BuildContext context) async {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          content: SingleChildScrollView(
            child: ListBody(
              children: [
                GestureDetector(
                  child: Text("Take a picture"),
                  onTap: () async {
                    final cameraImg = await new ImagePicker().getImage(source: ImageSource.camera);
                    if (cameraImg != null) {
                      File(cameraImg.path).copySync(join(Utils.docsDir.path, "avatar"));
                      contactsModel.rebuild();
                    }
                    Navigator.of(context).pop();
                  }
                ),
                GestureDetector(
                    child: Text("Select from Gallery"),
                    onTap: () async {
                      final galleryImg = await new ImagePicker().getImage(source: ImageSource.gallery);
                      if (galleryImg != null) {
                        File(galleryImg.path).copySync(join(Utils.docsDir.path, "avatar"));
                        contactsModel.rebuild();
                      }
                      Navigator.of(context).pop();
                    }
                )
              ],
            ),
          ),
        );
      }
    );
  }

  void _save(BuildContext context, ContactModel model) async {
    if (!_key.currentState.validate()) return;

    var id = -1;
    if (model.entityBeingEdited.id == -1) {
      print("Creating ${model.entityBeingEdited}");
      id = await ContactDbWorker.instance.create(model.entityBeingEdited);
    } else {
      print("Updating ${model.entityBeingEdited}");
      await ContactDbWorker.instance.update(model.entityBeingEdited);
      id = model.entityBeingEdited.id;
    }
    final file = File(join(Utils.docsDir.path, "avatar"));
    if (file.existsSync()) {
      file.renameSync(join(Utils.docsDir.path, id.toString()));
    }

    model.loadData();
    model.setStackIndex(0);
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      backgroundColor: Colors.grey,
      duration: Duration(seconds: 2),
      content: Text("Contact saved")
    ));
  }
}
