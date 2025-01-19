import 'package:flutter/material.dart';
import 'package:flutterchat/connector.dart';
import 'package:flutterchat/appdrawer.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class CreateRoom extends StatefulWidget {
  @override
  _CreateRoomState createState() => _CreateRoomState();
}

class _CreateRoomState extends State<CreateRoom> {
  final GlobalKey<FormState> _key = new GlobalKey();
  String _title = "";
  String _description = "";
  bool _isPrivate = false;
  double _maxPeople = 25;

  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(model: model, child: ScopedModelDescendant<FlutterChatModel>(
      builder: (context, child, model) {
        return Scaffold(
          resizeToAvoidBottomInset: false,
          appBar: AppBar(title: Text("Create Room")),
          drawer: AppDrawer(),
          bottomNavigationBar: Padding(
            padding: EdgeInsets.symmetric(horizontal: 10),
            child: SingleChildScrollView(child: Row(children: [
              FlatButton(
                child: Text("Cancel"),
                onPressed: () {
                  FocusScope.of(context).requestFocus(FocusNode());
                  Navigator.of(context).pop();
                },
              ),
              Spacer(),
              FlatButton(
                child: Text("OK"),
                onPressed: () {
                  if (!_key.currentState.validate()) return;
                  _key.currentState.save();
                  connector.create(_title, _description, _maxPeople.truncate(), _isPrivate, (String status, Map<dynamic, dynamic> roomList) {
                    if (status == "created") {
                      model.setRoomList(roomList);
                      FocusScope.of(context).requestFocus(FocusNode());
                      Navigator.of(context).pop();
                    } else {
                      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                        backgroundColor: Colors.red,
                        duration: Duration(seconds: 2),
                        content: Text("Sorry, that room already exists")
                      ));
                    }
                  });
                },
              ),
            ]))
          ),
          body: Form(
            key: _key,
            child: ListView(
              children: [
                ListTile(
                  leading: Icon(Icons.subject),
                  title: TextFormField(
                    decoration: InputDecoration(hintText: "Name"),
                    validator: (String value) {
                      if (value.isEmpty || value.length > 14) {
                        return "Please enter a name no more that 14 characters long";
                      } else return null;
                    },
                    onSaved: (String value) {
                      setState(() {
                        _title = value;
                      });
                    },
                  ),
                ),
                ListTile(
                  leading: Icon(Icons.description),
                  title: TextFormField(
                    decoration: InputDecoration(hintText: "Description"),
                    onSaved: (String value) {
                      setState(() {
                        _description = value;
                      });
                    },
                  ),
                ),
                ListTile(
                  title: Row(children: [
                    Text("Max\npeople"),
                    Slider(
                      min: 2,
                      max: 99,
                      value: _maxPeople,
                      onChanged: (value) {
                        setState(() {
                          _maxPeople = value;
                        });
                      }
                    )
                  ]),
                  trailing: Text(_maxPeople.toStringAsFixed(0)),
                ),
                ListTile(
                  title: Row(children: [
                    Text("Private room"),
                    Switch(value: _isPrivate, onChanged: (value) {
                      setState(() {
                        _isPrivate = value;
                      });
                    })
                  ],),
                )
              ],
            ),
          ),
        );
      },
    ));
  }
}
