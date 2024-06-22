using Game.Cache;
using Game.Data;
using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class Asteroids
    {
        private Rect _cameraRect;
        private float _time;
        private Cache<Asteroid> _cache;

        public Cache<Asteroid> Cache => _cache;

        public Asteroids(AsteroidView prefab, Rect cameraRect)
        {
            _cameraRect = cameraRect;
            _cache = new Cache<Asteroid>(() => new Asteroid(prefab, cameraRect));
        }

        public void Create(Vector2 position, Vector2 speed, AsteroidSize size)
        {
            var asteroid = _cache.Create();

            asteroid.Activate(position, speed, size);
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;

            _cache.Update();

            _time -= deltaTime;

            if (_cache.Count < 5 || _time < 0.0f)
            {
                _time = UnityEngine.Random.Range(5.0f, 15.0f);

                var position = Utils.Utils.RandomLocationOnEdge(_cameraRect);
                var speed = Utils.Utils.RandomDirection() * UnityEngine.Random.Range(0.5f, 4.0f);

                Create(position, speed, AsteroidSize.Big);
            }
        }
    }
}