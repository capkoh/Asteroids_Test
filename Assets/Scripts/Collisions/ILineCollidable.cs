using System.Collections.Generic;
using UnityEngine;

namespace Game.Collisions
{
    public interface ILineCollidable : ICollidable
    {
        IReadOnlyList<Vector2> Points { get; }
    }
}
