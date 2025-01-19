import 'package:flutter/material.dart';
import 'package:flutterchat/appdrawer.dart';
import 'package:flutterchat/model.dart';
import 'package:scoped_model/scoped_model.dart';

class UserList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ScopedModel<FlutterChatModel>(
      model: model,
      child: ScopedModelDescendant<FlutterChatModel>(builder: (context, child, model) {
        return Scaffold(
          drawer: AppDrawer(),
          appBar: AppBar(title: Text("User list")),
          body: GridView.builder(
            itemCount: model.userList.length,
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(crossAxisCount: 3),
            itemBuilder: (BuildContext context, int index) {
              final user = model.userList[index];
              return Padding(padding: EdgeInsets.all(10), child: Card(
                child: Padding(padding: EdgeInsets.all(10), child: GridTile(
                  child: Center(child: Padding(padding: EdgeInsets.only(bottom: 20), child: Image.asset("assets/user.jpg"))),
                  footer: Text(user, textAlign: TextAlign.center),
                ),),
              ));
            },
          ),
        );
      })
    );
  }
}
