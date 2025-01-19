import 'package:flutter/material.dart';

class GameObject {
  final double screenWidth;
  final double screenHeight;
  final String baseFilename;
  final int width;
  final int height;
  final int speed;
  final int numFrames;
  final int frameSkip;
  final List<Image> frames = [];
  final Function animationCallback;

  double x = 0;
  double y = 0;
  bool visible = true;
  int currentFrame = 0;
  int tick = 0;

  GameObject(
      this.screenWidth,
      this.screenHeight,
      this.baseFilename,
      this.width,
      this.height,
      this.numFrames,
      this.frameSkip,
      this.speed,
      this.x,
      this.y,
      this.animationCallback,
    ) {
    for (int i = 0; i<numFrames; ++i) {
      this.frames.add(Image.asset("assets/$baseFilename-$i.png"));
    }
  }

  void animate() {
    tick++;
    if (tick > frameSkip) {
      tick = 0;
      currentFrame++;
    }
    if (currentFrame == numFrames) {
      currentFrame = 0;
      if (animationCallback != null)
        animationCallback();
    }
  }

  Widget draw() {
    return visible ? Positioned(left: x, top: y, child: frames[currentFrame]) : Positioned(child: Container());
  }

  void move() {}
}
