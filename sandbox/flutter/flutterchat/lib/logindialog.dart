import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutterchat/connector.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';
import 'package:path/path.dart';

class LoginDialog extends StatelessWidget {
  static final GlobalKey<FormState> _key = new GlobalKey();

  String _userName;
  String _password;

  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(model: model, child: ScopedModelDescendant<FlutterChatModel>(
      builder: (context, child, model) {
        return AlertDialog(
          content: Container(
            height: 220,
            child: Form(key: _key, child: Column(
              children: [
                Text(
                  "Enter userName and password",
                  textAlign: TextAlign.center,
                  style: TextStyle(color: Theme.of(model.rootBuildContext).accentColor, fontSize: 18)
                ),
                SizedBox(height: 3),
                TextFormField(
                  validator: (value) {
                    if (value.isEmpty || value.length > 10) {
                      return "Please enter a userName no more than 10 characters long";
                    } else return null;
                  },
                  onSaved: (value) => _userName = value,
                  decoration: InputDecoration(hintText: "Username", labelText: "Username!"),
                ),
                TextFormField(
                  obscureText: true,
                  validator: (value) {
                    if (value.isEmpty) {
                      return "Please enter a password";
                    } else return null;
                  },
                  onSaved: (value) => _password = value,
                  decoration: InputDecoration(hintText: "Password", labelText: "Password!"),
                )
              ],
            )),
          ),
          actions: [
            FlatButton(child: Text("Log in"), onPressed: () {
              if (_key.currentState.validate()) {
                _key.currentState.save();
                connector.connectToServer(() {
                  connector.validate(_userName, _password, (status) {
                    switch (status) {
                      case "ok":
                        model.setUserName(_userName);
                        Navigator.of(model.rootBuildContext).pop();
                        model.setGreeting("Welcome back, $_userName!");
                        break;
                      case "fail":
                        ScaffoldMessenger.of(model.rootBuildContext).showSnackBar(SnackBar(
                          backgroundColor: Colors.red,
                          duration: Duration(seconds: 2),
                          content: Text("Sorry, userName is already taken"),
                        ));
                        break;
                      case "created":
                        final credentialsFile = new File(join(model.docsDir.path, "credentials"));
                        credentialsFile.writeAsString("$_userName============$_password");
                        model.setUserName(_userName);
                        Navigator.of(model.rootBuildContext).pop();
                        model.setGreeting("Welcome, $_userName!");
                        break;
                    }
                  });
                });
              }
            })
          ],
        );
      },
    ));
  }

  static void validateWithStoredCredentials(String user, String password) {
    connector.connectToServer(() {
      connector.validate(user, password, (status) {
        switch(status) {
          case "ok":
          case "created":
            model.setUserName(user);
            model.setGreeting("Welcome back, $user!!!");
            break;
          case "failed":
            showDialog(
                context: model.rootBuildContext,
                barrierDismissible: false,
                builder: (context) => AlertDialog(
                  title: Text("Validation failed"),
                  content: Text("Please choose a different userName"),
                  actions: [
                    FlatButton(child: Text("Damn!"), onPressed: () {
                      final credentialsFile = new File(join(model.docsDir.path, "credentials"));
                      credentialsFile.deleteSync();
                      exit(0);
                    })
                  ],
                ),
            );
            break;
        }
      });
    });
  }
}
