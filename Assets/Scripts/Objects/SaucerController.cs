using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class SaucerController
    {
        public Saucer Saucer => _saucer;

        private Rect _cameraRect;
        private float _saucerDelay;
        private float _saucerTime;
        private Saucer _saucer;
        private Ship _ship;

        public SaucerController(Rect cameraRect, Saucer saucer, Ship ship)
        {
            _cameraRect = cameraRect;
            _saucer = saucer;
            _ship = ship;

            _saucer.Deactivate();
        }

        private Vector2 GetTarget()
        {
            if (_ship.IsAlive)
            {
                return _ship.Position;
            }

            return new Vector2(
                Random.Range(_cameraRect.xMin, _cameraRect.xMax),
                Random.Range(_cameraRect.yMin, _cameraRect.yMax));
        }

        public void Update()
        {
            if (_saucer.IsAlive)
            {
                _saucer.Target = GetTarget();
                _saucer.Update();
            }
            else
            {
                _saucerTime += Time.deltaTime;

                if (_saucerTime >= _saucerDelay)
                {
                    _saucerTime = 0.0f;
                    _saucerDelay = Random.Range(5.0f, 10.0f);

                    _saucer.Target = GetTarget();
                    _saucer.Activate(Utils.Utils.RandomLocationOnEdge(_cameraRect));
                }
            }
        }
    }
}