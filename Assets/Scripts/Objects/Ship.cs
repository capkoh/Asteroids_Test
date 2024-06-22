using Game.Collisions;
using Game.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Objects
{
    public class Ship : ICirceCollidable
    {
        private const float _bulletDelay = 0.15f;
        private const float _impulse = 10.0f;
        private const float _rotation = 180.0f;
        private const float _friction = 0.2f;

        public Vector2 Position => _position;
        public Vector2 Speed => _speed;
        public float Angle => _angle;
        public float Radius => 0.5f;
        public bool IsAlive => _isAlive;

        public Laser Laser => _laser;
        public LaserCharges LaserAmmo => _laserAmmo;

        private float _bulletTime;
        private bool _isAlive;

        private ShipView _view;
        private Bullets _bullets;
        private Laser _laser;
        private LaserCharges _laserAmmo;

        private Rect _cameraRect;
        private Vector2 _position;
        private Vector2 _direction;
        private Vector2 _speed;
        private float _angle;

        public Ship(ShipView prefab, Rect cameraRect, Bullets bullets, Laser laser)
        {
            _cameraRect = cameraRect;
            _laserAmmo = new LaserCharges();
            _view = Object.Instantiate(prefab);
            _bullets = bullets;
            _laser = laser;
        }

        public void Activate()
        {
            _direction = Vector2.up;
            _position = Vector2.zero;
            _speed = Vector2.zero;
            _angle = 0.0f;

            _isAlive = true;
            _laserAmmo.Reset();
            _view.SetActive(true);
        }

        public void Deactivate()
        {
            _isAlive = false;
            _laser.Deactivate();
            _view.SetActive(false);
        }

        public void Update()
        {
            if (_isAlive)
            {
                UpdateInput();

                Move();

                UpdateLaser();
                UpdateBullets();

                UpdateView();
            }
        }

        private void UpdateView()
        {
            _view.Position = _position;
            _view.Angle = _angle;
            _view.HasEngine = Keyboard.current.upArrowKey.isPressed;
        }

        private void UpdateInput()
        {
            var keyboard = Keyboard.current;

            if (keyboard.leftArrowKey.isPressed)
            {
                Rotate(Time.deltaTime);
            }
            else if (keyboard.rightArrowKey.isPressed)
            {
                Rotate(-Time.deltaTime);
            }

            if (keyboard.upArrowKey.isPressed)
            {
                Engine();
            }
        }

        private void UpdateBullets()
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                _bulletTime -= Time.deltaTime;

                if (_bulletTime <= 0.0f)
                {
                    _bullets.Create(
                        _view.BarrelPosition,
                        _speed,
                        _direction,
                        true);

                    _bulletTime += _bulletDelay;
                }
            }
            else
            {
                _bulletTime = 0.0f;
            }
        }

        private void UpdateLaser()
        {
            if (Keyboard.current.eKey.isPressed)
            {
                if (_laserAmmo.Count > 0 && !_laser.IsActive)
                {
                    _laserAmmo.Use();
                    _laser.Activate();
                }
            }

            if (_laser.IsActive)
            {
                var p = _position + _direction * Radius * 2.0f;
                _laser.Rebuild(p, _direction);
            }

            _laserAmmo.Update();
            _laser.Update();
        }

        private void Rotate(float deltaTime)
        {
            _angle += _rotation * deltaTime;

            if (_angle > 360.0f)
            {
                _angle -= 360.0f;
            }
            else if (_angle < 0.0f)
            {
                _angle += 360.0f;
            }

            var a = _angle * Mathf.Deg2Rad;

            _direction = new Vector2(
                -Mathf.Sin(a),
                 Mathf.Cos(a));
        }

        private void Engine()
        {
            _speed += _direction * _impulse * Time.deltaTime;
        }

        private void Move()
        {
            _position += _speed * Time.deltaTime;
            _position = Utils.Utils.Teleport(_cameraRect, _position);

            _speed -= _speed * _friction * Time.deltaTime;
        }
    }
}