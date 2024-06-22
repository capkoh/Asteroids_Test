using Game.Collisions.Implementation;
using Game.Objects;
using Game.Statistics;
using Game.Views;
using UnityEngine;

namespace Game
{
    public class Logic
    {
        private Ship _ship;
        private Bullets _bullets;
        private Asteroids _asteroids;
        private CollisionsProcessor _collisionsProcessor;
        private Saucer _saucer;
        private SaucerController _saucerController;
        private Laser _laser;
        private Rect _cameraRect;
        private StatisticsStorage _statisticsStorage;

        public Ship Ship => _ship;
        public StatisticsStorage StatisticsStorage => _statisticsStorage;

        public Logic(Camera camera, Prefabs prefabs)
        {
            _cameraRect.min = camera.ViewportToWorldPoint(Vector2.zero);
            _cameraRect.max = camera.ViewportToWorldPoint(Vector2.one);

            _statisticsStorage = new StatisticsStorage();
            _bullets = new Bullets(prefabs.BulletPrefab, _cameraRect);
            _laser = new Laser(prefabs.LaserPrefab, _cameraRect);
            _ship = new Ship(prefabs.ShipPrefab, _cameraRect, _bullets, _laser);
            _asteroids = new Asteroids(prefabs.AsteroidPrefab, _cameraRect);
            _saucer = new Saucer(prefabs.SaucerPrefab, _cameraRect, _bullets);
            _saucerController = new SaucerController(_cameraRect, _saucer, _ship);
            _collisionsProcessor = new CollisionsProcessor(_ship, _bullets, _asteroids, _saucer, _statisticsStorage);
        }

        public void Restart()
        {
            _statisticsStorage.Reset();
            _saucer.Deactivate();
            _bullets.Reset();
            _ship.Activate();
        }

        public void Update()
        {
            _ship.Update();
            _bullets.Update();
            _asteroids.Update();
            _saucerController.Update();
            _collisionsProcessor.Update();
        }
    }
}