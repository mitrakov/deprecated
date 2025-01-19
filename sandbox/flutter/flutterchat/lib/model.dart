import 'dart:io';

import 'package:flutter/material.dart';
import 'package:scoped_model/scoped_model.dart';

class FlutterChatModel extends Model {
  static final String DEFAULT_ROOM_NAME = "Not in a room";

  BuildContext rootBuildContext;
  Directory docsDir = Directory.current;
  String greeting = "";
  String userName = "";
  String currentRoomName = DEFAULT_ROOM_NAME;
  List<String> currentRoomUserList = [];
  bool currentRoomEnabled = false;
  List<Map<String, String>> currentRoomMessages = [];
  List<Map<dynamic, dynamic>> roomList = [];
  List<String> userList = [];
  bool creatorFunctionsEnabled = false;
  Map<String, bool> roomInvites = {};

  void setGreeting(String greeting) {
    this.greeting = greeting;
    notifyListeners();
  }

  void setUserName(String userName) {
    this.userName = userName;
    notifyListeners();
  }

  void setCurrentRoom(String currentRoomName) {
    this.currentRoomName = currentRoomName;
    notifyListeners();
  }

  void setCreatorFunctionEnabled(bool creatorFunctionsEnabled) {
    this.creatorFunctionsEnabled = creatorFunctionsEnabled;
    notifyListeners();
  }

  void setCurrentRoomEnabled(bool currentRoomEnabled) {
    this.currentRoomEnabled = currentRoomEnabled;
    notifyListeners();
  }

  void addMessage(String userName, String message) {
    currentRoomMessages.add({"userName": userName, "message": message});
    notifyListeners();
  }

  void setRoomList(Map<dynamic, dynamic> roomList) {
    List<Map<dynamic, dynamic>> rooms = [];
    for (String roomName in roomList.keys) {
      final room = roomList[roomName];
      rooms.add(room);
    }
    this.roomList = rooms;
    notifyListeners();
  }

  void setUserList(Map<dynamic, dynamic> userList) {
    List<String> users = [];
    print("setUserList: $userList");
    for (String userName in userList.keys) {
      final user = userList[userName];
      users.add(user["userName"]);
    }
    this.userList = users;
    notifyListeners();
  }

  void setCurrentRoomUserList(Map<dynamic, dynamic> userList) {
    List<String> users = [];
    for (String userName in userList.keys) {
      final user = userList[userName];
      users.add(user["userName"]);
    }
    this.currentRoomUserList = users;
    notifyListeners();
  }

  void addRoomInvite(String roomName) {
    roomInvites[roomName] = true;
    notifyListeners();
  }

  void removeRoomInvite(String roomName) {
    roomInvites[roomName] = false;
    notifyListeners();
  }

  void clearCurrentRoomMessages() {
    currentRoomMessages.clear();
    notifyListeners();
  }
}

final FlutterChatModel model = new FlutterChatModel();
