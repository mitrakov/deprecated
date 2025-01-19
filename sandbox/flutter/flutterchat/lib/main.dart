import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutterchat/createroom.dart';
import 'package:flutterchat/home.dart';
import 'package:flutterchat/lobby.dart';
import 'package:flutterchat/logindialog.dart';
import 'package:flutterchat/model.dart';
import 'package:flutterchat/room.dart';
import 'package:flutterchat/userlist.dart';
import 'package:path_provider/path_provider.dart';
import 'package:path/path.dart';
import 'package:scoped_model/scoped_model.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  startMeUp();
  runApp(FlutterChat());
}

void startMeUp() async {
  final docsDir = await getApplicationDocumentsDirectory();
  final credentialsFile = new File(join(docsDir.path, "credentials"));
  final exists = await credentialsFile.exists();

  model.docsDir = docsDir;
  if (exists) {
    final credentials = await credentialsFile.readAsString();
    final credParts = credentials.split("============");
    LoginDialog.validateWithStoredCredentials(credParts[0], credParts[1]);
  } else {
    await showDialog(context: model.rootBuildContext, barrierDismissible: false, builder: (context) => new LoginDialog());
  }
}

class FlutterChat extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(home: Scaffold(body: new FlutterChatMain()));
  }
}

class FlutterChatMain extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    model.rootBuildContext = context;
    return ScopedModel<FlutterChatModel>(model: model, child: ScopedModelDescendant<FlutterChatModel>(
      builder: (context, child, model) {
        return MaterialApp(
          initialRoute: "/",
          routes: {
            "/lobby": (context) => Lobby(),
            "/room": (context) => model.currentRoomEnabled ? Room() : Lobby(),
            "/users": (context) => UserList(),
            "/createRoom": (context) => CreateRoom(),
          },
          home: Home(),
        );
      },
    ));
  }
}
