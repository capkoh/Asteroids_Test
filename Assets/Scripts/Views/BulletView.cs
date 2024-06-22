using UnityEngine;

namespace Game.Views
{
    public class BulletView : MonoBehaviour
    {
        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        private Transform _transform;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
    }
}