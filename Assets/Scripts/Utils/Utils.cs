using System;
using UnityEngine;

namespace Game.Utils
{
    public static class Utils
    {
        public static Vector2 Teleport(Rect cameraRect, Vector2 position)
        {
            if (position.x < cameraRect.xMin)
            {
                position.x += cameraRect.width;
            }
            else if (position.x > cameraRect.xMax)
            {
                position.x -= cameraRect.width;
            }

            if (position.y < cameraRect.yMin)
            {
                position.y += cameraRect.height;
            }
            else if (position.y > cameraRect.yMax)
            {
                position.y -= cameraRect.height;
            }

            return position;
        }

        public static Vector2 RandomDirection()
        {
            var angle = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);
            return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        private static readonly Vector2[] _locations = new Vector2[5];
        private static readonly float[] _distances = new float[5];

        public static Vector2 GetClosestLocation(Vector2 from, Vector2 to, Rect cameraRect)
        {
            _locations[0] = to;
            _locations[1] = new Vector2(to.x - cameraRect.width, to.y);
            _locations[2] = new Vector2(to.x + cameraRect.width, to.y);
            _locations[3] = new Vector2(to.x, to.y - cameraRect.height);
            _locations[4] = new Vector2(to.x, to.y + cameraRect.height);

            for (int i = 0; i < _locations.Length; ++i)
            {
                _distances[i] = (_locations[i] - from).sqrMagnitude;
            }

            Array.Sort(_distances, _locations);

            return _locations[0];
        }

        public static Vector2 RandomLocationOnEdge(Rect rect)
        {
            var x = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var y = UnityEngine.Random.Range(rect.yMin, rect.yMax);

            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0: return new Vector2(rect.xMin, y);
                case 1: return new Vector2(rect.xMax, y);
                case 2: return new Vector2(x, rect.yMin);
                case 3: return new Vector2(x, rect.yMax);
            }

            return Vector2.zero;
        }

        // p - should be on the line
        public static bool InsideSegment(Vector2 p, Vector2 p1, Vector2 p2)
        {
            var a = (p1 - p).normalized;
            var b = (p2 - p).normalized;

            return Vector2.Dot(a, b) < 0.0f;
        }

        // o1 - ray origin
        // o2 - ray direction
        // p1 - 1st point on line
        // p2 - 2nd point on line
        public static bool Intersection(Vector2 o1, Vector2 o2, Vector2 p1, Vector2 p2, out Vector2 p)
        {
            if (Vector2.SignedAngle(o2, p2 - p1) < 0.0f)
            {
                var _1 = Line.FromRay(o1, o2);
                var _2 = Line.FromPoints(p1, p2);

                var d = _1.A * _2.B - _2.A * _1.B;

                if (Mathf.Abs(d) > float.Epsilon)
                {
                    p.x = (_1.B * _2.C - _2.B * _1.C) / d;
                    p.y = (_1.C * _2.A - _2.C * _1.A) / d;

                    if (InsideSegment(p, p1, p2))
                    {
                        return true;
                    }
                }
            }

            p = o1;

            return false;
        }

        public enum RectSide
        {
            None,
            Left,
            Right,
            Bottom,
            Top
        }

        public static RectSide IntesectInsideRect(Rect rect, Vector2 start, Vector2 direction, out Vector2 result)
        {
            var c0 = new Vector2(rect.xMin, rect.yMin);
            var c1 = new Vector2(rect.xMin, rect.yMax);
            var c2 = new Vector2(rect.xMax, rect.yMax);
            var c3 = new Vector2(rect.xMax, rect.yMin);

            if (Intersection(start, direction, c0, c1, out result))
            {
                return RectSide.Left;
            }

            if (Intersection(start, direction, c1, c2, out result))
            {
                return RectSide.Top;
            }

            if (Intersection(start, direction, c2, c3, out result))
            {
                return RectSide.Right;
            }

            if (Intersection(start, direction, c3, c0, out result))
            {
                return RectSide.Bottom;
            }

            return RectSide.None;
        }

        public static Vector2 Teleport(Rect cameraRect, RectSide currentSide, Vector2 position)
        {
            if (currentSide == RectSide.Left)
            {
                position.x += cameraRect.width;
            }
            else if (currentSide == RectSide.Right)
            {
                position.x -= cameraRect.width;
            }

            if (currentSide == RectSide.Bottom)
            {
                position.y += cameraRect.height;
            }
            else if (currentSide == RectSide.Top)
            {
                position.y -= cameraRect.height;
            }

            return position;
        }
    }
}