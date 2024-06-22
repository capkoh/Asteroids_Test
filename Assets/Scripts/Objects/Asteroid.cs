using Game.Cache;
using Game.Collisions;
using Game.Data;
using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class Asteroid : ICirceCollidable, ICacheItem
    {
        public Vector2 Position => _position;
        public Vector2 Speed => _speed;
        public AsteroidSize Size => _size;
        public float Radius => _radius;
        public bool IsAlive => _isAlive;

        private Rect _cameraRect;
        private Vector2 _position;
        private Vector2 _speed;
        private AsteroidSize _size;
        private float _radius;
        private bool _isAlive;

        private AsteroidView _view;

        public Asteroid(AsteroidView prefab, Rect cameraRect)
        {
            _cameraRect = cameraRect;
            _view = Object.Instantiate(prefab);
        }

        public void Activate(Vector2 position, Vector2 speed, AsteroidSize size)
        {
            _position = position;
            _speed = speed;
            _size = size;
            _radius = SizeToRadius(size);
            _isAlive = true;

            _view.Activate(size);
            _view.Position = position;
        }

        public void Deactivate()
        {
            _isAlive = false;

            _view.Deactivate();
        }

        public void Update()
        {
            _position += _speed * Time.deltaTime;
            _position = Utils.Utils.Teleport(_cameraRect, _position);

            _view.Position = _position;
        }

        private static float SizeToRadius(AsteroidSize size)
        {
            switch (size)
            {
                case AsteroidSize.Small: return 0.6f;
                case AsteroidSize.Medium: return 1.2f;
                default: return 1.8f;
            }
        }
    }
}