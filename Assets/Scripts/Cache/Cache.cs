using System;
using System.Collections.Generic;

namespace Game.Cache
{
    public class Cache<T> where T : class, ICacheItem
    {
        private List<T> _items = new List<T>();
        private int _count;

        private Func<T> _factory;

        public IReadOnlyList<T> Items => _items;
        public int Count => _count;

        public Cache(Func<T> factory)
        {
            _factory = factory;
        }

        public T Create()
        {
            T t;

            if (_count < _items.Count)
            {
                t = _items[_count];
            }
            else
            {
                t = _factory();

                _items.Add(t);
            }

            ++_count;

            return t;
        }

        public void Update()
        {
            int itemsCount = 0;

            for (int i = 0; i < _count;)
            {
                var t = _items[i];

                if (t.IsAlive)
                {
                    t.Update();
                }

                if (t.IsAlive)
                {
                    ++itemsCount;

                    ++i;

                    continue;
                }

                _items[i] = _items[_count - 1];
                _items[_count - 1] = t;

                --_count;
            }

            _count = itemsCount;
        }
    }
}