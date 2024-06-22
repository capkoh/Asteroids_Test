using Game.Cache;
using Game.Views;
using UnityEngine;

namespace Game.Objects
{
    public class Bullets
    {
        private Cache<Bullet> _cache;

        public Cache<Bullet> Cache => _cache;

        public Bullets(BulletView prefab, Rect cameraRect)
        {
            _cache = new Cache<Bullet>(() => new Bullet(prefab, cameraRect));
        }

        public void Create(Vector2 position, Vector2 speed, Vector2 direction, bool isPlayer)
        {
            var bullet = _cache.Create();

            bullet.Activate(position, speed, direction, isPlayer);
        }

        public void Update()
        {
            _cache.Update();
        }

        public void Reset()
        {
            for (int i = 0; i < _cache.Count; ++i)
            {
                var bullet = _cache.Items[i];
                bullet.Deactivate();
            }
        }
    }
}