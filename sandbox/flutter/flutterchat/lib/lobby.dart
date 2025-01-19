import 'package:flutter/material.dart';
import 'package:flutterchat/connector.dart';
import 'package:flutterchat/appdrawer.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class Lobby extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(
        model: model,
        child: ScopedModelDescendant<FlutterChatModel>(builder: (context, child, model) {
          return Scaffold(
            drawer: AppDrawer(),
            appBar: AppBar(title: Text("Lobby")),
            floatingActionButton: FloatingActionButton(child: Icon(Icons.add, color: Colors.white), onPressed: () {
              Navigator.pushNamed(context, "/createRoom");
            }),
            body: model.roomList.isEmpty
              ? Center(child: Text("There are no rooms yet."))
              : ListView.builder(
                itemCount: model.roomList.length,
                itemBuilder: (context, index) {
                  final room = model.roomList[index];
                  final String roomName = room["roomName"];
                  final bool isPrivate = room["private"];
                  return Column(children: [
                    ListTile(
                      leading: Image.asset("assets/${isPrivate ? "private" : "public"}.jpg"),
                      title: Text(roomName),
                      subtitle: Text(room["description"]),
                      onTap: () {
                        if (isPrivate && !model.roomInvites.containsKey(roomName) && room["creator"] != model.userName) {
                          ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                            backgroundColor: Colors.red,
                            duration: Duration(seconds: 2),
                            content: Text("Sorry, you can't enter a private room without an invite")
                          ));
                        } else {
                          connector.join(model.userName, roomName, (status, roomInfo) {
                            switch (status) {
                              case "joined":
                                model.setCurrentRoom(roomInfo["roomName"]);
                                model.setCurrentRoomUserList(roomInfo["users"]);
                                model.setCurrentRoomEnabled(true);
                                model.clearCurrentRoomMessages();
                                model.setCreatorFunctionEnabled(roomInfo["creator"] == model.userName);
                                Navigator.pushNamed(context, "/room");
                                break;
                              case "full":
                                ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                                  backgroundColor: Colors.red,
                                  duration: Duration(seconds: 2),
                                  content: Text("Sorry, that room is full")
                                ));
                                break;
                            }
                          });
                        }
                      },
                    ),
                  ]);
                }),
          );
        })
    );
  }
}
