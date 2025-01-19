import 'package:flutter/material.dart';
import 'package:flutterchat/appdrawer.dart';
import 'package:flutterchat/connector.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class Room extends StatefulWidget {
  @override
  _RoomState createState() => _RoomState();
}

class _RoomState extends State<Room> {
  final ScrollController scrollController = new ScrollController();
  final TextEditingController textController = new TextEditingController(text: "");
  bool _isExpanded = false;
  String _message = "";

  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(
        model: model,
        child: ScopedModelDescendant<FlutterChatModel>(builder: (context, child, model) {
          return Scaffold(
            drawer: AppDrawer(),
            appBar: AppBar(
              title: Text("User list"),
              actions: [
                PopupMenuButton(
                  onSelected: (String value) {
                    switch (value) {
                      case "invite":
                      case "kick":
                        _inviteOrKick(context, value);
                        break;
                      case "leave":
                        connector.leave(model.userName, model.currentRoomName, () {
                          model.removeRoomInvite(model.currentRoomName);
                          model.setCurrentRoomUserList({});
                          model.setCurrentRoom(FlutterChatModel.DEFAULT_ROOM_NAME);
                          model.setCurrentRoomEnabled(false);
                          Navigator.of(context).pushNamedAndRemoveUntil("/", ModalRoute.withName("/"));
                        });
                        break;
                      case "close":
                        connector.close(model.currentRoomName, () {
                          Navigator.of(context).pushNamedAndRemoveUntil("/", ModalRoute.withName("/"));
                        });
                        break;
                    }
                  },
                  itemBuilder: (context) {
                    return <PopupMenuEntry<String>>[
                      PopupMenuItem(value: "leave", child: Text("Leave Room")),
                      PopupMenuItem(value: "invite", child: Text("Invite user")),
                      PopupMenuDivider(),
                      PopupMenuItem(value: "close", child: Text("Close Room")),
                      PopupMenuItem(value: "kick", child: Text("Kick user")),
                    ];
                  }
                ),
              ],
            ),
            body: Column(children: [
              ExpansionPanelList(
                expansionCallback: (index, expanded) {
                  setState(() {
                    _isExpanded = !_isExpanded;
                  });
                },
                children: [
                  ExpansionPanel(
                    isExpanded: _isExpanded,
                    headerBuilder: (context, expanded) => Text("Users in room"),
                    body: Padding(padding: EdgeInsets.only(bottom: 10), child: Builder(builder: (context) {
                      final List<Widget> userList = [];
                      for (var user in model.currentRoomUserList) { // TODO make func style!
                        userList.add(Text(user));
                      }
                      return Column(children: userList);
                    }))
                  )
                ],
              ),
              Container(height: 10),
              Expanded(
                child: ListView.builder(
                  controller: scrollController,
                  itemCount: model.currentRoomMessages.length,
                  itemBuilder: (BuildContext context, int index) {
                    final message = model.currentRoomMessages[index];
                    return ListTile(title: Text(message["message"]), subtitle: Text(message["userName"]));
                  }
                ),
              ),
              Divider(),
              Row(children: [
                Flexible(child: TextField(
                  controller: textController,
                  onChanged: (String text) {
                    setState(() {
                      _message = text;
                    });
                  },
                  decoration: new InputDecoration.collapsed(hintText: "Enter message"),
                )),
                Container(
                  margin: EdgeInsets.fromLTRB(2, 0, 2, 0),
                  child: IconButton(
                    icon: Icon(Icons.send),
                    color: Colors.blue,
                    onPressed: () {
                      connector.post(model.userName, model.currentRoomName, _message, (status) {
                        if (status == "ok") {
                          model.addMessage(model.userName, _message);
                          scrollController.jumpTo(scrollController.position.maxScrollExtent);
                        }
                      });
                    },
                  ),
                )
              ])
            ]),
          );
        })
    );
  }

  void _inviteOrKick(BuildContext context, String command) {
    connector.listUsers((Map<dynamic, dynamic> userList) {
      model.setUserList(userList);
      showDialog(context: context, builder: (BuildContext context) {
        return AlertDialog(
          title: Text("Select user to $command"),
          content: Container(width: double.maxFinite, height: double.maxFinite/2, child: ListView.builder(
            itemCount: command == "invite" ? model.userList.length : model.currentRoomUserList.length,
            itemBuilder: (BuildContext context, int index) {
              final user = command == "invite" ? model.userList[index] : model.currentRoomUserList[index];
              if (user == model.userName) return Container();
              else return Container(
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.all(Radius.circular(15)),
                  border: Border(bottom:  BorderSide(), top: BorderSide(), left: BorderSide(), right: BorderSide()),
                  gradient: LinearGradient(
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    stops: [.1, .2, .3, .4, .5, .6, .7, .8, .9],
                    colors: [
                      Color.fromRGBO(250, 250, 0, .75),
                      Color.fromRGBO(250, 220, 0, .75),
                      Color.fromRGBO(250, 190, 0, .75),
                      Color.fromRGBO(250, 160, 0, .75),
                      Color.fromRGBO(250, 130, 0, .75),
                      Color.fromRGBO(250, 110, 0, .75),
                      Color.fromRGBO(250, 80, 0, .75),
                      Color.fromRGBO(250, 50, 0, .75),
                      Color.fromRGBO(250, 0, 0, .75),
                    ]
                  )
                ),
                margin: EdgeInsets.only(top: 10),
                child: ListTile(title: Text(user), onTap: () {
                  switch (command) {
                    case "invite":
                      connector.invite(user, model.currentRoomName, model.userName, () => Navigator.of(context).pop());
                      break;
                    case "kick":
                      connector.kick(user, model.currentRoomName, () => Navigator.of(context).pop());
                      break;
                  }
                }),
              );
            },
          )),
        );
      });
    });
  }
}
