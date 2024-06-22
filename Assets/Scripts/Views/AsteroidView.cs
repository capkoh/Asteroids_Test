using Game.Data;
using UnityEngine;

namespace Game.Views
{
    public class AsteroidView : MonoBehaviour
    {
        private Transform _transform;

        public SpriteRenderer Renderer;
        public Sprite[] Sprites;

        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public void Activate(AsteroidSize size)
        {
            Renderer.sprite = Sprites[(int)size];

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }
}