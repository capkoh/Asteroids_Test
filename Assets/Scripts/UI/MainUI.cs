using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class MainUI : MonoBehaviour
    {
        public TextMeshProUGUI Label;
        public GameObject StartLabel;
        public TextMeshProUGUI AsteroidsLabel;

        public void ShowAsteroidsLabel(bool everStarted, int score)
        {
            if (AsteroidsLabel.gameObject.activeSelf)
            {
                return;
            }

            AsteroidsLabel.gameObject.SetActive(true);

            if (everStarted)
            {
                AsteroidsLabel.text = $"GAME OVER\n\nSCORE: {score}";
            }
        }

        public void HideAsteroidsLabel()
        {
            AsteroidsLabel.gameObject.SetActive(false);
        }
    }
}