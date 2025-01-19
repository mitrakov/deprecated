import 'package:fluttergame/gameobject.dart';

class Enemy extends GameObject {
  final int moveDirection;

  Enemy(
      double screenWidth,
      double screenHeight,
      String baseFilename,
      int width,
      int height,
      int numFrames,
      int frameSkip,
      this.moveDirection,
      speed,
      double x,
      double y,
    ) : super(screenWidth, screenHeight, baseFilename, width, height, numFrames, frameSkip, speed, x, y, null);

  @override
  void move() {
    if (moveDirection == 1) {
      x += speed;
      if (x > screenWidth + width)
        x = -width.toDouble();
    } else {
      x -= speed;
      if (x < -width)
        x = screenWidth + width.toDouble();
    }
  }
}
