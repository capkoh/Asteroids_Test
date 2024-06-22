using Game.Cache;
using Game.Collisions;
using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class Saucer : ICacheItem, ICirceCollidable
    {
        public Vector2 Position => _position;
        public float Radius => 0.75f;
        public bool IsAlive => _isAlive;
        public Vector2 Target { get; set; }

        private const float _bulletDelay = 0.75f;
        private float _bulletTime;

        private Rect _cameraRect;
        private Bullets _bullets;
        private bool _isAlive;
        private Vector2 _position;
        private Vector2 _direction;
        private float _velocity;

        private SaucerView _view;

        public Saucer(SaucerView prefab, Rect cameraRect, Bullets bullets)
        {
            _cameraRect = cameraRect;
            _view = Object.Instantiate(prefab);

            _bullets = bullets;
        }

        public void Activate(Vector2 position)
        {
            _position = position;
            _direction = Target - position;
            _direction.Normalize();

            _view.Position = position;
            _view.SetActive(true);
            _isAlive = true;
        }

        public void Deactivate()
        {
            _view.SetActive(false);
            _isAlive = false;
        }

        public void Update()
        {
            Move();
            Shot();

            _view.Position = _position;
        }

        private void Shot()
        {
            _bulletTime -= Time.deltaTime;

            if (_bulletTime > 0.0f)
            {
                return;
            }

            var target = Utils.Utils.GetClosestLocation(_position, Target, _cameraRect);
            var vector = target - _position;

            if (vector.sqrMagnitude < 100.0f)
            {
                var bulletDirection = vector.normalized;
                var bulletOffset = bulletDirection * Radius * 1.5f;

                _bullets.Create(
                    _position + bulletOffset,
                    _direction * _velocity,
                    bulletDirection,
                    false);

                _bulletTime += _bulletDelay;
            }
            else
            {
                _bulletTime = 0.0f;
            }
        }

        private void Move()
        {
            var vector = Target - _position;
            var distance = vector.magnitude;
            var factor = distance < 5.0f ? 5.0f : 1.0f;

            if (distance < 10.0f)
            {
                var v = Vector2.Perpendicular(vector);
                var f = Mathf.Clamp01((distance - 5.0f) / 5.0f);
                vector = Vector3.Slerp(v, vector, f);
            }

            _velocity += _velocity * factor * Time.deltaTime;
            _velocity = Mathf.Clamp(_velocity, 0.5f, 2.0f);

            _direction = Vector3.Slerp(_direction, vector.normalized, Time.deltaTime);
            _direction.Normalize();

            _position += _direction * _velocity * Time.deltaTime;
            _position = Utils.Utils.Teleport(_cameraRect, _position);
        }
    }
}