import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:fluttergame/inputctrl.dart';
import 'package:fluttergame/gamecore.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    SystemChrome.setEnabledSystemUIOverlays([SystemUiOverlay.bottom]); // full-screen
    return MaterialApp(title: "FlutterGame", home: GameScreen());
  }
}

class GameScreen extends StatefulWidget {
  @override
  _GameScreenState createState() => _GameScreenState();
}

class _GameScreenState extends State<GameScreen> with TickerProviderStateMixin {
  GameCore core;
  InputController ctrl;

  @override
  Widget build(BuildContext context) {
    if (core == null) {
      core = new GameCore(context, this, this);
      ctrl = new InputController(core.player);
    }
    final List<Widget> children = [
      Positioned(
        left: 0,
        top: 0,
        child: Container(
          width: core.screenWidth,
          height: core.screenHeight,
          decoration: BoxDecoration(
            image: DecorationImage(
              image: AssetImage("assets/background.png"),
              fit: BoxFit.cover,
            ),
          ),
        ),
      ),
      Positioned(
        left: 4,
        top: 2,
        child: Text(
          'Score: ${core.score.toString().padLeft(3, "0")}',
          style: TextStyle(color: Colors.white, fontSize: 18, fontWeight: FontWeight.w900)
        ),
      ),
      Positioned(
        left: 120,
        top: 2,
        width: core.screenWidth - 124,
        height: 22,
        child: LinearProgressIndicator(
          value: core.player.energy,
          backgroundColor: Colors.white,
          valueColor: const AlwaysStoppedAnimation(Colors.red),
        ),
      ),
      core.crystal.draw(),
    ];

    // order of adding widgets to the List matters!
    for (int i=0; i<3; ++i) {
      children.add(core.fish[i].draw());
      children.add(core.robots[i].draw());
      children.add(core.aliens[i].draw());
      children.add(core.asteroids[i].draw());
    }
    children.add(core.planet.draw());
    children.add(core.player.draw());
    for (int i=0; i<core.explosions.length; ++i) {
      children.add(core.explosions[i].draw());
    }

    return Scaffold(
      body: GestureDetector(
        onPanStart: ctrl.onPanStart,
        onPanUpdate: ctrl.onPanUpdate,
        onPanEnd: ctrl.onPanEnd,
        child: Stack(children: children),
      ),
    );
  }
}
