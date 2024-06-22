using UnityEngine;

namespace Game.Utils
{
    public struct Line
    {
        public float A;
        public float B;
        public float C;

        public static Line FromPoints(Vector2 _1, Vector2 _2)
        {
            var a = _1.y - _2.y;
            var b = _2.x - _1.x;
            var c = _1.x * _2.y - _2.x * _1.y;

            return new Line { A = a, B = b, C = c };
        }

        public static Line FromRay(Vector2 p, Vector2 direction)
        {
            return FromPoints(p, p + direction);
        }

        public float Distance(Vector2 point)
        {
            var b = Mathf.Sqrt(A * A + B * B);

            if (b < float.Epsilon)
            {
                return 0.0f;
            }

            var a = A * point.x + B * point.y + C;

            return Mathf.Abs(a) / b;
        }

        public Vector2 ClosestPoint(Vector2 point)
        {
            var b = A * A + B * B;

            if (b < float.Epsilon)
            {
                return point;
            }

            return new Vector2(
                (B * (B * point.x - A * point.y) - A * C) / b,
                (A * (A * point.y - B * point.x) - B * C) / b);
        }
    }
}