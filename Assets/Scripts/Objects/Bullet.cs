using Game.Cache;
using Game.Collisions;
using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class Bullet : ICirceCollidable, ICacheItem
    {
        private const float _velocity = 20.0f;
        private const float Duration = 1.0f;

        private Rect _cameraRect;
        private Vector2 _position;
        private Vector2 _speed;
        private bool _isAlive;
        private bool _isPlayer;
        private float _bulletTime;

        public Vector2 Position => _position;
        public Vector2 Speed => _speed;
        public bool IsAlive => _isAlive;
        public bool IsPlayer => _isPlayer;
        public float Radius => 0.06f;

        private BulletView _view;

        public Bullet(BulletView prefab, Rect cameraRect)
        {
            _cameraRect = cameraRect;
            _view = Object.Instantiate(prefab);
        }

        public void Activate(Vector2 position, Vector2 speed, Vector2 direction, bool isPlayer)
        {
            _position = position;
            _speed = speed + direction * _velocity;
            _isAlive = true;
            _isPlayer = isPlayer;
            _bulletTime = 0.0f;

            _view.Activate();
            _view.Position = position;
        }

        public void Deactivate()
        {
            _isAlive = false;
            _view.Deactivate();
        }

        public void Update()
        {
            if (_bulletTime < Duration)
            {
                _bulletTime += Time.deltaTime;

                _position += _speed * Time.deltaTime;
                _position = Utils.Utils.Teleport(_cameraRect, _position);

                _view.Position = _position;

                if (_bulletTime >= Duration)
                {
                    Deactivate();
                }
            }
        }
    }
}