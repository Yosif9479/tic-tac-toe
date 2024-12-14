using Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class WinnerDisplay : MonoBehaviour
    {
        [SerializeField] private Image _iconSpriteRenderer;
        [SerializeField] private GameObject _panel;
        
        [SerializeField] private Field _field;

        private void OnEnable()
        {
            _field.PlayerWon += OnPlayerWon;
        }

        private void OnDisable()
        {
            _field.PlayerWon -= OnPlayerWon;
        }

        private void OnPlayerWon(Player player)
        {
            _panel.SetActive(true);
            _iconSpriteRenderer.sprite = player.Icon;
            _iconSpriteRenderer.color = player.Color;
        }
    }
}