import 'dart:convert';
import 'dart:io';
import 'package:adhara_socket_io/adhara_socket_io.dart';
import 'package:adhara_socket_io/manager.dart';
import 'package:adhara_socket_io/socket.dart';
import 'package:flutter/material.dart';
import 'package:flutterchat/model.dart';

class Connector {
  SocketIO _socket;

  void showPleaseWait() {
    showDialog(context: model.rootBuildContext, barrierDismissible: false, builder: (BuildContext context) {
      return Dialog(
        child: Container(
          width: 150,
          height: 150,
          alignment: AlignmentDirectional.center,
          decoration: BoxDecoration(color: Colors.blue[200]),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Center(child: SizedBox(height: 50, width: 50, child: CircularProgressIndicator(value: null, strokeWidth: 10))),
              Container(margin: EdgeInsets.only(top: 20), child: Center(child: Text("Please wait", style: TextStyle(color: Colors.white))))
            ],
          ),
        ),
      );
    });
  }

  void hidePleaseWait() {
    Navigator.of(model.rootBuildContext).pop();
  }

  void connectToServer(Function callback) async {
    final host = Platform.isAndroid ? "10.0.2.2" : "127.0.0.1";    // localhost
    print("Connecting to $host:8080...");
    _socket = await SocketIOManager().createInstance(SocketOptions("http://$host:8080"));
    _socket.onConnect.listen((event) => print("Connected: $event"));
    _socket.on("newUser").listen((event) => newUser(event));
    _socket.on("created").listen((event) => created(event));
    _socket.on("closed").listen((event) => closed(event));
    _socket.on("joined").listen((event) => joined(event));
    _socket.on("left").listen((event) => left(event));
    _socket.on("kicked").listen((event) => kicked(event));
    _socket.on("invited").listen((event) => invited(event));
    _socket.on("posted").listen((event) => posted(event));

    await _socket.connectSync();
    await callback();
  }
  
  void newUser(List<dynamic> data) {
    print("newUser");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    model.setUserList(payload);
  }

  void created(List<dynamic> data) {
    print("created");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    model.setRoomList(payload);
  }

  void closed(List<dynamic> data) {
    print("closed");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    final String roomName = payload["roomName"];

    model.setRoomList(payload["rooms"]);
    if (roomName == model.currentRoomName) {
      model.removeRoomInvite(roomName);
      model.setCurrentRoomUserList({});
      model.setCurrentRoom(FlutterChatModel.DEFAULT_ROOM_NAME);
      model.setCurrentRoomEnabled(false);
      model.setGreeting("The room you were in was closed by its creator");
      Navigator.of(model.rootBuildContext).pushNamedAndRemoveUntil("/", ModalRoute.withName("/"));
    }
  }

  void joined(List<dynamic> data) {
    print("joined");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    if (model.currentRoomName == payload["roomName"]) {
      model.setCurrentRoomUserList(payload["users"]);
    }
  }

  void left(List<dynamic> data) {
    print("left");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    if (model.currentRoomName == payload["roomName"]) {
      model.setCurrentRoomUserList({});
    }
  }

  void kicked(List<dynamic> data) {
    print("kicked");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    final String roomName = payload["roomName"];

    //model.setRoomList(payload);
    if (roomName == model.currentRoomName) {
      model.removeRoomInvite(roomName);
      model.setCurrentRoomUserList({});
      model.setCurrentRoom(FlutterChatModel.DEFAULT_ROOM_NAME);
      model.setCurrentRoomEnabled(false);
      model.setGreeting("Sorry, you have been kicked out of the room '$roomName'");
      Navigator.of(model.rootBuildContext).pushNamedAndRemoveUntil("/", ModalRoute.withName("/"));
    }
  }

  void invited(List<dynamic> data) {
    print("invited");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    final String roomName = payload["roomName"];
    final String inviterName = payload["inviter"];

    model.addRoomInvite(roomName);
    ScaffoldMessenger.of(model.rootBuildContext).showSnackBar(SnackBar(
      backgroundColor: Colors.amber,
      duration: Duration(seconds: 60),
      content: Text("You've been invited to the room '$roomName' by '$inviterName'\n\n"),
      action: SnackBarAction(label: "OK", onPressed: () {}),
    ));
  }

  void posted(List<dynamic> data) {
    print("posted");
    final Map<dynamic, dynamic> payload = data.first;
    print("received: $payload");
    if (model.currentRoomName == payload["roomName"]) {
      model.addMessage(payload["userName"], payload["message"]);
    }
  }

  void validate(String userName, String password, Function(String status) callback) async {
    print("validate");
    showPleaseWait();
    final List<Object> data = await _socket.emitWithAck("validate", [jsonEncode({"userName": userName, "password": password})]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response["status"]);
  }

  void listRooms(Function(Map<dynamic, dynamic> roomList) callback) async {
    print("listRooms");
    showPleaseWait();
    final data = await _socket.emitWithAck("listRooms", ["{}"]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response);
  }

  void listUsers(Function(Map<dynamic, dynamic> userList) callback) async {
    print("listUsers");
    showPleaseWait();
    final data = await _socket.emitWithAck("listUsers", ["{}"]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response);
  }

  void join(String userName, String roomName, Function(String status, Map<dynamic, dynamic> descriptor) callback) async {
    print("join");
    showPleaseWait();
    final data = await _socket.emitWithAck("join", [jsonEncode({"userName": userName, "roomName": roomName})]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response["status"], response["room"]);
  }

  void create(String roomName, String description, int maxPeople, bool private, Function(String status, Map<dynamic, dynamic> roomList) callback) async {
    print("create");
    showPleaseWait();
    final data = await _socket.emitWithAck("create", [jsonEncode({"roomName": roomName, "description": description, "maxPeople": maxPeople, "private": private, "creator": model.userName})]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response["status"], response["rooms"]);
  }

  void leave(String userName, String roomName, Function() callback) async {
    print("leave");
    showPleaseWait();
    final data = await _socket.emitWithAck("leave", [jsonEncode({"userName": userName, "roomName": roomName})]);
    print("received: $data");
    hidePleaseWait();
    callback();
  }

  void close(String roomName, Function() callback) async {
    print("close");
    showPleaseWait();
    final data = await _socket.emitWithAck("close", [jsonEncode({"roomName": roomName})]);
    print("received: $data");
    hidePleaseWait();
    callback();
  }

  void post(String userName, String roomName, String message, Function(String status) callback) async {
    print("post");
    showPleaseWait();
    final data = await _socket.emitWithAck("post", [jsonEncode({"userName": userName, "roomName": roomName, "message": message})]);
    print("received: $data");
    final Map<dynamic, dynamic> response = data.first;
    hidePleaseWait();
    callback(response["status"]);
  }

  void invite(String userName, String roomName, String inviter, Function callback) async {
    print("invite");
    showPleaseWait();
    final data = await _socket.emitWithAck("invite", [jsonEncode({"userName": userName, "roomName": roomName, "inviter": inviter})]);
    print("received: $data");
    hidePleaseWait();
    callback();
  }

  void kick(String userName, String roomName, Function callback) async {
    print("kick");
    showPleaseWait();
    final data = await _socket.emitWithAck("kick", [jsonEncode({"userName": userName, "roomName": roomName})]);
    print("received: $data");
    hidePleaseWait();
    callback();
  }
}

final Connector connector = new Connector();
