using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    public class LaserView : MonoBehaviour
    {
        public LaserViewSegment LaserViewSegmentPrefab;

        private List<LaserViewSegment> _segments = new List<LaserViewSegment>();
        private int _segmentsCount;

        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        private Transform _transform;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private LaserViewSegment GetSegment()
        {
            LaserViewSegment segment;

            if (_segmentsCount < _segments.Count)
            {
                segment = _segments[_segmentsCount];
            }
            else
            {
                segment = Instantiate(LaserViewSegmentPrefab, _transform);

                _segments.Add(segment);
            }

            ++_segmentsCount;

            return segment;
        }

        public void Build(IReadOnlyList<Vector2> points)
        {
            _segmentsCount = 0;

            for (int i = 0; i < points.Count;)
            {
                var p0 = points[i++];
                var p1 = points[i++];

                var segment = GetSegment();
                segment.Activate(p0, p1);
            }

            for (int i = _segmentsCount; i < _segments.Count; ++i)
            {
                _segments[i].Deactivate();
            }
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
    }
}