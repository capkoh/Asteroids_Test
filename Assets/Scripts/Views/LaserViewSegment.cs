using UnityEngine;

namespace Game.Views
{
    public class LaserViewSegment : MonoBehaviour
    {
        public Transform Transform;
        public SpriteRenderer SpriteRenderer;

        public Vector2 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        public void Activate(Vector2 start, Vector2 end)
        {
            var vector = end - start;

            Transform.position = start;
            Transform.rotation = Quaternion.LookRotation(Vector3.forward, vector);
            SpriteRenderer.size = new Vector2(0.16f + vector.magnitude, 0.16f);

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}