using UnityEngine;

namespace Game.Collisions
{
    public interface ICirceCollidable : ICollidable
    {
        Vector2 Position { get; }
        float Radius { get; }
    }
}