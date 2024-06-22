using UnityEngine;

namespace Game.Objects
{
    public class LaserCharges
    {
        private const float CooldownDuration = 5.0f;
        private const int MaximumCount = 3;

        private float _cooldownTime;
        private int _count;

        public float Cooldown => CooldownDuration - _cooldownTime;
        public int Count => _count;

        public LaserCharges()
        {
            Reset();
        }

        public void Reset()
        {
            _count = 1;
            _cooldownTime = 0.0f;
        }

        public void Use()
        {
            if (_count > 0)
            {
                _count--;
            }
        }

        public void Update()
        {
            if (_count < MaximumCount)
            {
                _cooldownTime += Time.deltaTime;

                if (_cooldownTime > CooldownDuration)
                {
                    _cooldownTime -= CooldownDuration;
                    _count++;
                }
            }
            else
            {
                _cooldownTime = 0.0f;
            }
        }
    }
}