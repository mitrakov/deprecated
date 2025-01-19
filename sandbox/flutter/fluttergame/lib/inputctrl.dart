import 'package:flutter/material.dart';
import 'package:fluttergame/player.dart';

class InputController {
  static final int MOVE_SENSITIVITY = 20;

  double anchorX = 0;
  double anchorY = 0;

  final Player player;

  InputController(this.player);

  void onPanStart(DragStartDetails details) {
    anchorX = details.globalPosition.dx;
    anchorY = details.globalPosition.dy;
    player.moveX = 0;
    player.moveY = 0;
  }

  void onPanUpdate(DragUpdateDetails details) {
    if (details.globalPosition.dx < anchorX - MOVE_SENSITIVITY) {
      player.moveX = -1;
      player.orientationChanged();
    } else if (details.globalPosition.dx > anchorX + MOVE_SENSITIVITY) {
      player.moveX = 1;
      player.orientationChanged();
    } else {
      player.moveX = 0;
      player.orientationChanged();
    }

    if (details.globalPosition.dy < anchorY - MOVE_SENSITIVITY) {
      player.moveY = -1;
      player.orientationChanged();
    } else if (details.globalPosition.dy > anchorY + MOVE_SENSITIVITY) {
      player.moveY = 1;
      player.orientationChanged();
    } else {
      player.moveY = 0;
      player.orientationChanged();
    }
  }

  void onPanEnd(DragEndDetails details) {
    player.moveX = 0;
    player.moveY = 0;
  }
}
