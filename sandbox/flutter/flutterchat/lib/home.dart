import 'package:flutter/material.dart';
import 'package:flutterchat/appdrawer.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class Home extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(
        model: model,
        child: ScopedModelDescendant<FlutterChatModel>(builder: (context, child, model) {
          return Scaffold(
            drawer: AppDrawer(),
            appBar: AppBar(title: Text("Flutter Chat")),
            body: Center(child: Text(model.greeting)),
          );
        })
    );
  }
}
