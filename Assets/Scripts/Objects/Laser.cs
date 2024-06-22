using Game.Collisions;
using Game.Views;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects
{
    public class Laser : ILineCollidable
    {
        private const float LaserDuration = 1.0f;
        private const float LaserLength = 20.0f;

        public IReadOnlyList<Vector2> Points => _points;
        public bool IsActive => _isActive;

        private Rect _cameraRect;
        private bool _isActive;
        private List<Vector2> _points = new List<Vector2>();
        private float _laserTime;

        private LaserView _view;

        public Laser(LaserView prefab, Rect cameraRect)
        {
            _cameraRect = cameraRect;
            _view = Object.Instantiate(prefab);
        }

        public void Activate()
        {
            _isActive = true;
            _laserTime = 0.0f;

            _view.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;

            _view.SetActive(false);
        }

        public void Rebuild(Vector2 position, Vector2 direction)
        {
            position = Utils.Utils.Teleport(_cameraRect, position);
            BuildPoints(position, direction);

            _view.Build(_points);
        }

        public void Update()
        {
            if (_laserTime < LaserDuration)
            {
                _laserTime += Time.deltaTime;

                if (_laserTime >= LaserDuration)
                {
                    Deactivate();
                }
            }
        }

        private void BuildPoints(Vector2 start, Vector2 direction)
        {
            _points.Clear();

            var length = 0.0f;

            while (length < LaserLength)
            {
                var side = Utils.Utils.IntesectInsideRect(_cameraRect, start, direction, out var intersection);

                var distance = (intersection - start).magnitude;
                var segment = Mathf.Min(LaserLength - length, distance);
                var segmentPoint = start + direction * segment;

                _points.Add(start);
                _points.Add(segmentPoint);

                start = intersection;
                length += distance;

                start = Utils.Utils.Teleport(_cameraRect, side, start);
            }
        }
    }
}