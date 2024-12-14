using Runtime.Basics;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class TurnManager : SingletonBehaviour<TurnManager>
    {
        [SerializeField] private Player[] _players;

        private int _currentIndex;

        public Player CurrentPlayer
        {
            get
            {
                if (_currentIndex >= _players.Length) _currentIndex = 0;
                return _players[_currentIndex]; 
            }
        }

        private void OnEnable()
        {
            Cell.Occupied += OnCellOccupied;
        }

        private void OnDisable()
        {
            Cell.Occupied -= OnCellOccupied;
        }

        private void NextTurn()
        {
            _currentIndex++;
        }

        private void OnCellOccupied() => NextTurn();
    }
}