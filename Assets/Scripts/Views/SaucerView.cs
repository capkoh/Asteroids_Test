using UnityEngine;

namespace Game.Views
{
    public class SaucerView : MonoBehaviour
    {
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

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
    }
}