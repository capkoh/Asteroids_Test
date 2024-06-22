using Game.UI;
using Game.Views;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Main : MonoBehaviour
    {
        public Camera Camera;
        public MainUI UI;
        public Prefabs Prefabs;

        private Logic _logic;
        private float _startCooldown;

        private void Start()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            _logic = new Logic(Camera, Prefabs);
            _logic.Ship.Deactivate();
        }

        private void Update()
        {
            var s = string.Empty;

            var ship = _logic.Ship;

            if (ship.IsAlive)
            {
                // Not optimized. Made this way to read and edit easily.
                s += $"Score: {_logic.StatisticsStorage.Score}\n";
                s += $"Location: {ship.Position.x:0.0}, {ship.Position.y:0.0}\n";
                s += $"Angle: {ship.Angle:0.0}\n";
                s += $"Speed: {ship.Speed.magnitude:0.0} u/s\n";
                s += $"Laser Charges: {ship.LaserAmmo.Count}\n";
                s += $"Laser Charge Cooldown: {ship.LaserAmmo.Cooldown:0.0} s\n";

                _startCooldown = 1.5f;
            }
            else
            {
                UI.ShowAsteroidsLabel(_logic.StatisticsStorage.EverStarted, _logic.StatisticsStorage.Score);

                _startCooldown -= Time.deltaTime;

                if (_startCooldown < 0.0f)
                {
                    UI.StartLabel.SetActive(true);

                    if (Keyboard.current.anyKey.isPressed)
                    {
                        _logic.StatisticsStorage.SetEverStarted();

                        UI.StartLabel.SetActive(false);
                        UI.HideAsteroidsLabel();

                        _logic.Restart();
                    }
                }
            }

            UI.Label.text = s;

            _logic.Update();
        }
    }
}