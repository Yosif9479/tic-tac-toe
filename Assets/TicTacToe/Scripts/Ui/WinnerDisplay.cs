using Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class WinnerDisplay : MonoBehaviour
    {
        [SerializeField] private Image _iconSpriteRenderer;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text _displayText;
        
        [SerializeField] private Field _field;

        private const string WonText = "Won!";
        private const string DrawnText = "Draw!";

        private void OnEnable()
        {
            _field.PlayerWon += OnPlayerWon;
            _field.Draw += OnDraw;
        }

        private void OnDisable()
        {
            _field.PlayerWon -= OnPlayerWon;
            _field.Draw += OnDraw;
        }

        private void OnPlayerWon(Player player)
        {
            _panel.SetActive(true);
            _iconSpriteRenderer.sprite = player.Icon;
            _iconSpriteRenderer.color = player.Color;
            _displayText.text = WonText;
        }

        private void OnDraw()
        {
            _panel.SetActive(true);
            _iconSpriteRenderer.enabled = false;
            _displayText.text = DrawnText;
        }
    }
}