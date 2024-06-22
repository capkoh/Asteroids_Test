using UnityEngine;

namespace Game.Views
{
    public class ShipView : MonoBehaviour
    {
        public GameObject Engine;
        public GameObject Barrel;

        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public float Angle
        {
            get => _transform.eulerAngles.z;
            set => _transform.eulerAngles = new Vector3(0.0f, 0.0f, value);
        }

        public bool HasEngine
        {
            get => Engine.activeSelf;
            set => Engine.SetActive(value);
        }

        public Vector2 BarrelPosition => _barrelTransform.position;

        private Transform _transform;
        private Transform _barrelTransform;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _barrelTransform = Barrel.GetComponent<Transform>();
        }
    }
}