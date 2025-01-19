import 'package:flutter/material.dart';
import 'package:flutterchat/connector.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class AppDrawer extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(
        model: model,
        child: ScopedModelDescendant<FlutterChatModel>(builder: (context, child, model) {
          return Drawer(
            child: Column(children: [
              Container(
                decoration: BoxDecoration(image: DecorationImage(fit: BoxFit.cover, image: AssetImage("assets/drawback.jpg"))),
                child: ListTile(
                  title: Padding(
                    padding: EdgeInsets.fromLTRB(0, 30, 0, 15),
                    child: Center(
                      child: Text(model.userName, style: TextStyle(color: Colors.white, fontSize: 24)),
                    ),
                  ),
                  subtitle: Center(child: Text(model.currentRoomName, style: TextStyle(color: Colors.white, fontSize: 16))),
                ),
              ),
              Padding(
                padding: EdgeInsets.fromLTRB(0, 20, 0, 0),
                child: ListTile(
                  leading: Icon(Icons.list),
                  title: Text("Lobby"),
                  onTap: () {
                    Navigator.of(context).pushNamedAndRemoveUntil("/lobby", ModalRoute.withName("/"));
                    connector.listRooms((roomList) {
                      model.setRoomList(roomList);
                    });
                  },
                ),
              ),
              Padding(
                padding: EdgeInsets.fromLTRB(0, 20, 0, 0),
                child: ListTile(
                  leading: Icon(Icons.room),
                  title: Text("Current room"),
                  onTap: () {
                    Navigator.of(context).pushNamedAndRemoveUntil("/room", ModalRoute.withName("/"));
                  },
                ),
              ),
              Padding(
                padding: EdgeInsets.fromLTRB(0, 20, 0, 0),
                child: ListTile(
                  leading: Icon(Icons.supervised_user_circle_sharp),
                  title: Text("User List"),
                  onTap: () {
                    Navigator.of(context).pushNamedAndRemoveUntil("/users", ModalRoute.withName("/"));
                    connector.listUsers((Map<dynamic, dynamic> userList) {
                      model.setUserList(userList);
                    });
                  },
                ),
              )
            ]),
          );
        })
    );
  }
}
