using Game.Cache;
using Game.Data;
using Game.Objects;
using Game.Statistics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Collisions.Implementation
{
    public class CollisionsProcessor
    {
        private Ship _ship;
        private Saucer _saucer;
        private Bullets _bullets;
        private Asteroids _asteroids;
        private StatisticsStorage _statisticsStorage;

        public CollisionsProcessor(Ship ship, Bullets bullets, Asteroids asteroids, Saucer saucer, StatisticsStorage statisticsStorage)
        {
            _ship = ship;
            _bullets = bullets;
            _asteroids = asteroids;
            _saucer = saucer;
            _statisticsStorage = statisticsStorage;
        }

        public void Update()
        {
            CheckBullets();
            CheckLaser();
            CheckShip();
        }

        private void CheckShip()
        {
            if (!_ship.IsAlive)
            {
                return;
            }

            var asteroid = FindCollision(_ship, _asteroids.Cache);

            if (asteroid != null)
            {
                GameOver();

                return;
            }

            var bullet = FindCollision(_ship, _bullets.Cache);

            if (bullet != null)
            {
                GameOver();

                return;
            }

            if (_saucer.IsAlive && Overlaps(_ship, _saucer))
            {
                GameOver();

                return;
            }
        }

        private void GameOver()
        {
            _ship.Deactivate();
        }

        private static bool Overlaps(ICirceCollidable a, ICirceCollidable b)
        {
            var vector = a.Position - b.Position;
            var radius = a.Radius + b.Radius;

            return vector.sqrMagnitude < radius * radius;
        }

        private static bool Overlaps(ILineCollidable a, ICirceCollidable b)
        {
            var sqrRadius = b.Radius * b.Radius;

            for (int i = 0; i < a.Points.Count;)
            {
                var p0 = a.Points[i++];
                var p1 = a.Points[i++];

                var e0 = p0 - b.Position;
                var e1 = p1 - b.Position;

                if (e0.sqrMagnitude < sqrRadius || e1.sqrMagnitude < sqrRadius)
                {
                    return true;
                }

                var line = Utils.Line.FromPoints(p0, p1);
                var point = line.ClosestPoint(b.Position);
                var vector = point - b.Position;

                if (vector.sqrMagnitude > sqrRadius)
                {
                    continue;
                }

                if (Utils.Utils.InsideSegment(point, p0, p1))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Overlaps(ICollidable a, ICollidable b)
        {
            {
                if (a is ICirceCollidable circleA && b is ICirceCollidable circleB)
                {
                    return Overlaps(circleA, circleB);
                }
            }

            {
                if (a is ILineCollidable lineA && b is ICirceCollidable circleB)
                {
                    return Overlaps(lineA, circleB);
                }

                if (a is ICirceCollidable circleA && b is ILineCollidable lineB)
                {
                    return Overlaps(lineB, circleA);
                }
            }

            return false;
        }

        private static T FindCollision<T>(ICollidable collidable, Cache<T> collidables) where T : class, ICollidable, ICacheItem
        {
            for (int i = 0; i < collidables.Count; ++i)
            {
                var item = collidables.Items[i];

                if (!item.IsAlive)
                {
                    continue;
                }

                var overlaps = Overlaps(collidable, item);

                if (!overlaps)
                {
                    continue;
                }

                return item;
            }

            return null;
        }

        private void CheckBullets()
        {
            for (int i = 0; i < _bullets.Cache.Count; ++i)
            {
                var bullet = _bullets.Cache.Items[i];

                if (!bullet.IsAlive)
                {
                    continue;
                }

                var asteroid = FindCollision(bullet, _asteroids.Cache);

                if (asteroid != null)
                {
                    Destruct(asteroid);
                    bullet.Deactivate();

                    if (bullet.IsPlayer)
                    {
                        _statisticsStorage.ScoreAsteroid();
                    }

                    continue;
                }

                if (_saucer.IsAlive && Overlaps(_saucer, bullet))
                {
                    _saucer.Deactivate();
                    bullet.Deactivate();

                    _statisticsStorage.ScoreSaucer();

                    continue;
                }
            }
        }

        private void CheckLaser()
        {
            if (_ship.Laser.IsActive)
            {
                var asteroid = FindCollision(_ship.Laser, _asteroids.Cache);

                if (asteroid != null)
                {
                    Destruct(asteroid);

                    _statisticsStorage.ScoreAsteroid();
                }

                if (_saucer.IsAlive && Overlaps(_saucer, _ship.Laser))
                {
                    _saucer.Deactivate();

                    _statisticsStorage.ScoreSaucer();
                }
            }
        }

        private void Destruct(Asteroid asteroid)
        {
            asteroid.Deactivate();

            if (asteroid.Size == AsteroidSize.Small)
            {
                return;
            }

            var position = asteroid.Position;
            var velocity = asteroid.Speed.magnitude;
            var size = asteroid.Size == AsteroidSize.Big ? AsteroidSize.Medium : AsteroidSize.Small;

            for (int i = 0; i < 2; ++i)
            {
                var speed = Utils.Utils.RandomDirection();
                speed *= velocity * Random.Range(1.2f, 1.8f);

                _asteroids.Create(position, speed, size);
            }
        }
    }
}