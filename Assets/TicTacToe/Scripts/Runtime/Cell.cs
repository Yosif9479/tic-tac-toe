﻿using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        public static event UnityAction Occupied;
        
        [SerializeField] private SpriteRenderer _iconSpriteRenderer;
        
        private AudioSource _audioSource;
        private TurnManager _turnManager;
        private Field _field;

        public bool IsOccupied { get; private set; } = false;
        public Player OccupiedPlayer { get; private set; } = null;
        
        private void Awake()
        {
            _turnManager = TurnManager.Instance;
            _field = GetComponentInParent<Field>();
            _audioSource = GetComponent<AudioSource>();
        }

        private bool TryOccupy()
        {
            if (IsOccupied) return false;

            if (_field.State is not FieldState.WaitingForTurn) return false;
            
            IsOccupied = true;
            OccupiedPlayer = _turnManager.CurrentPlayer;
            _iconSpriteRenderer.color = OccupiedPlayer.Color;
            _iconSpriteRenderer.sprite = OccupiedPlayer.Icon;
            _audioSource.Play();
            Occupied?.Invoke();

            return true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            TryOccupy();
        }
    }
}