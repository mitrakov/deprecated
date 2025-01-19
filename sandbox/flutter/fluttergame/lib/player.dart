import 'package:flutter/material.dart';
import 'package:fluttergame/gameobject.dart';
import 'package:vector_math/vector_math.dart';

class Player extends GameObject {
  int moveX = 0;
  int moveY = 0;
  double energy = 0;
  double angle = 0;

  Player(
      double screenWidth,
      double screenHeight,
      String baseFilename,
      int width,
      int height,
      int numFrames,
      int frameSkip,
      speed,
      double x,
      double y,
    ) : super(screenWidth, screenHeight, baseFilename, width, height, numFrames, frameSkip, speed, x, y, null);

  @override
  Widget draw() {
    return visible
        ? Positioned(left: x, top: y, child: Transform.rotate(angle: angle, child: frames[currentFrame]))
        : Positioned(child: Container());
  }

  void orientationChanged() {
    angle = 0;
    if (moveX == 1 && moveY == -1)  angle = radians(45);
    if (moveX == 1 && moveY == 0)   angle = radians(90);
    if (moveX == 1 && moveY == 1)   angle = radians(135);
    if (moveX == 0 && moveY == 1)   angle = radians(180);
    if (moveX == -1 && moveY == 1)  angle = radians(225);
    if (moveX == -1 && moveY == 0)  angle = radians(270);
    if (moveX == -1 && moveY == -1) angle = radians(315);
  }

  @override
  void move() {
    if (x > 0 && moveX == -1)
      x -= speed;
    if (x < (screenWidth - width) && moveX == 1)
      x += speed;
    if (y > 40 && moveY == -1)
      y -= speed;
    if (y < (screenHeight - height - 10) && moveY == 1)
      y += speed;
  }
}
