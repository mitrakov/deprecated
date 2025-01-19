import 'dart:math';
import 'package:flutter/material.dart';
import 'package:audioplayers/audio_cache.dart';
import 'package:fluttergame/gameobject.dart';
import 'package:fluttergame/enemy.dart';
import 'package:fluttergame/player.dart';

class GameCore {
  final State<dynamic> state;
  final Random random = new Random();
  final double screenWidth;
  final double screenHeight;
  final AnimationController gameLoopCtrl;
  /*final*/ Animation gameLoopAnimation;
  /*final*/ GameObject crystal;
  /*final*/ GameObject planet;
  /*final*/ Player player;
  final List<Enemy> fish = [];
  final List<Enemy> robots = [];
  final List<Enemy> aliens = [];
  final List<Enemy> asteroids = [];
  final List<GameObject> explosions = [];
  final AudioCache audioCache = new AudioCache();

  int score = 0;

  GameCore(BuildContext context, TickerProvider ticker, this.state):
        screenWidth = MediaQuery.of(context).size.width,
        screenHeight = MediaQuery.of(context).size.height,
        gameLoopCtrl = new AnimationController(vsync: ticker, duration: Duration(milliseconds: 1000))
  {
    audioCache.loadAll(["delivery.wav", "explosion.wav", "fill.wav", "thrust.wav"]);
    crystal = new GameObject(screenWidth, screenHeight, "crystal", 32, 30, 4, 6, 0, 0, 0, null);
    planet = new GameObject(screenWidth, screenHeight, "planet", 64, 64, 1, 0, 0, 0, 0, null);
    player = new Player(screenWidth, screenHeight, "player", 40, 34, 2, 6, 2, 0, 0);

    for (int i = 0; i < 3; ++i) {
      fish.add(new Enemy(screenWidth, screenHeight, "fish", 48, 48, 2, 6, 1, 1, 0, 0));
      robots.add(new Enemy(screenWidth, screenHeight, "robot", 48, 48, 2, 6, 1, 1, 0, 0));
      aliens.add(new Enemy(screenWidth, screenHeight, "alien", 48, 48, 2, 6, 1, 1, 0, 0));
      asteroids.add(new Enemy(screenWidth, screenHeight, "asteroid", 48, 48, 2, 6, 1, 1, 0, 0));
    }

    gameLoopAnimation = Tween(begin: 0, end: 17).animate(CurvedAnimation(parent: gameLoopCtrl, curve: Curves.linear));
    gameLoopAnimation.addStatusListener((status) {
      if (status == AnimationStatus.completed) {
        // restart animation
        gameLoopCtrl.reset();
        gameLoopCtrl.forward();
      }
    });
    gameLoopAnimation.addListener(gameLoop);

    resetGame(true);
    gameLoopCtrl.forward(); // start game!
  }

  void gameLoop() {
    crystal.animate();
    for (int i = 0; i < 3; i++) {
      fish[i].move();
      fish[i].animate();
      robots[i].move();
      robots[i].animate();
      aliens[i].move();
      aliens[i].animate();
      asteroids[i].move();
      asteroids[i].animate();
    }
    player.move();
    player.animate();
    for (int i = 0; i < explosions.length; ++i) { // don't use foreach! iterators are unsafe here!
      explosions[i].animate();
    }

    if (collision(crystal))
      transferEnergy(true);
    else if (collision(planet))
      transferEnergy(false);
    else if (player.energy > 0 && player.energy < 1)
      player.energy = 0;

    for (int i = 0; i < 3; ++i) {
      if (collision(fish[i]) || collision(robots[i]) || collision(aliens[i]) || collision(asteroids[i])) {
        audioCache.play("explosion.wav");
        player.visible = false;
        final explosion = new GameObject(screenWidth, screenHeight, "explosion", 50, 50, 5, 4, 0, player.x, player.y, () {
          resetGame(false);
        });
        explosions.add(explosion);
        score -= 50;
        if (score < 0) score = 0;
      }
    }

    state.setState(() {});
  }

  bool collision(GameObject obj) {
    if (!player.visible || !obj.visible) return false;

    final left1 = player.x;
    final right1 = left1 + player.width;
    final top1 = player.y;
    final bottom1 = top1 + player.height;
    final left2 = obj.x;
    final right2 = left2 + obj.width;
    final top2 = obj.y;
    final bottom2 = top2 + obj.height;

    if (bottom1 < top2) return false;
    if (top1 > bottom2) return false;
    if (right1 < left2) return false;
    if (left1 > right2) return false;
    return true;
  }

  void transferEnergy(bool isCrystalOrPlanet) {
    if (isCrystalOrPlanet && player.energy < 1) {
      if (player.energy == 0)
        audioCache.play("fill.wav");
      player.energy += .01;
      if (player.energy >= 1) {
        player.energy = 1;
        randomlyPositionObject(crystal);
      }
    } else if (player.energy > 0) {
      if (player.energy >= 1)
        audioCache.play("delivery.wav");
      player.energy -= .01;
      if (player.energy <= 0) {
        player.energy = 0;
        audioCache.play("explosion.wav");
        score += 100;
        final Function callback = () {resetGame(true);};
        for (var i = 0; i < 3; ++i) {
          fish[i].visible = false;
          final explosion1 = new GameObject(screenWidth, screenHeight, "explosion", 50, 50, 5, 4, 0, fish[i].x, fish[i].y, callback);
          explosions.add(explosion1);

          robots[i].visible = false;
          final explosion2 = new GameObject(screenWidth, screenHeight, "explosion", 50, 50, 5, 4, 0, robots[i].x, robots[i].y, callback);
          explosions.add(explosion2);

          aliens[i].visible = false;
          final explosion3 = new GameObject(screenWidth, screenHeight, "explosion", 50, 50, 5, 4, 0, aliens[i].x, aliens[i].y, callback);
          explosions.add(explosion3);

          asteroids[i].visible = false;
          final explosion4 = new GameObject(screenWidth, screenHeight, "explosion", 50, 50, 5, 4, 0, asteroids[i].x, asteroids[i].y, callback);
          explosions.add(explosion4);
        }
      }
    }
  }

  void resetGame(bool resetEnemies) {
    player.energy = 0;
    player.x = (screenWidth / 2) - (player.width / 2);
    player.y = screenHeight - player.height - 24;
    player.moveX = 0;
    player.moveY = 0;
    player.orientationChanged();

    crystal.y = 34;
    randomlyPositionObject(crystal);

    planet.y = screenHeight - planet.height - 10;
    randomlyPositionObject(planet);

    if (resetEnemies) {
      final List<double> xFish = [70, 192, 312];
      final List<double> xRobots = [64, 192, 320];
      final List<double> xAliens = [44, 192, 340];
      final List<double> xAsteroids = [24, 192, 360];
      for (int i = 0; i < 3; ++i) {
        fish[i].x = xFish[i];
        robots[i].x = xRobots[i];
        aliens[i].x = xAliens[i];
        asteroids[i].x = xAsteroids[i];
        fish[i].y = 110;
        robots[i].y = fish[i].y + 120;
        aliens[i].y = robots[i].y + 130;
        asteroids[i].y = aliens[i].y + 140;
        fish[i].visible = true;
        robots[i].visible = true;
        aliens[i].visible = true;
        asteroids[i].visible = true;
      }
    }

    explosions.clear();
    player.visible = true;
  }

  void randomlyPositionObject(GameObject obj) {
    obj.x = random.nextInt(screenWidth.toInt() - obj.width).toDouble();
    if (collision(obj))
      randomlyPositionObject(obj);
  }
}
