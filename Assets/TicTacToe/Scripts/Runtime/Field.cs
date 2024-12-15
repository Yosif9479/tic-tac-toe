using System;
using System.Collections;
using System.Linq;
using Runtime.Enums;
using Runtime.Models;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class Field : MonoBehaviour
    {
        public event UnityAction<Player> PlayerWon;
        public event UnityAction Draw;
        
        [SerializeField] private LineRenderer _winLine;
        [SerializeField] private float _restartDelay = 2f;
        
        private static readonly WinCombination[] WinCombinations =
        {
            //Diagonals
            new(new Vector2Int[] { new(0, 0), new(1, 1), new(2, 2) }),
            new(new Vector2Int[] { new(0, 2), new(1, 1), new(2, 0) }),
            
            //Rows
            new(new Vector2Int[] { new(0, 0), new(1, 0), new(2, 0) }),
            new(new Vector2Int[] { new(0, 1), new(1, 1), new(2, 1) }),
            new(new Vector2Int[] { new(0, 2), new(1, 2), new(2, 2) }),
            
            //Columns
            new(new Vector2Int[] { new(0, 0), new(0, 1), new(0, 2) }),
            new(new Vector2Int[] { new(1, 0), new(1, 1), new(1, 2) }),
            new(new Vector2Int[] { new(2, 0), new(2, 1), new(2, 2) }),
        };
        
        private readonly Cell[,] _cells = new Cell[3, 3];
        private AudioSource _audioSource;

        public FieldState State { get; private set; } = FieldState.WaitingForTurn;

        private void Awake()
        {
            InitCells();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            Cell.Occupied += OnCellOccupied;
        }

        private void OnDisable()
        {
            Cell.Occupied -= OnCellOccupied;
        }

        private void OnCellOccupied()
        {
            Player winner = CheckForWinner(out WinCombination combination);

            if (winner is not null)
            {
                OnWin(winner, combination);
                return;
            }

            bool isDraw = CheckForDraw();
            
            if (isDraw) OnDraw();
        }
        
        /// <summary>
        /// <returns>
        /// <c>Player</c> that won.
        /// <c>null</c> if drawn.
        /// </returns>
        /// </summary>
        private Player CheckForWinner(out WinCombination winCombination)
        {
            foreach (WinCombination combination in WinCombinations)
            {
                Vector2Int[] coords = combination.Coordinates;
                
                Cell first = _cells[coords[0].x, coords[0].y];
                Cell second = _cells[coords[1].x, coords[1].y];
                Cell third = _cells[coords[2].x, coords[2].y];
                
                Cell[] cells = { first, second, third };

                Player player = first.OccupiedPlayer;

                if (player is null) continue;
                
                if (cells.Any(cell => !cell.IsOccupied)) continue;

                if (cells.Any(cell => cell.OccupiedPlayer != player)) continue;
                
                winCombination = combination;
                return player;
            }
            
            winCombination = null;
            
            return null;
        }

        private bool CheckForDraw()
        {
            bool result = true;
            
            foreach(Cell cell in _cells) if (!cell.IsOccupied) result = false;
            
            return result;
        }

        private void OnWin(Player player, WinCombination combination)
        {
            State = FieldState.Finished;
            DrawLine(combination.Coordinates);
            StartCoroutine(RestartDelayedCoroutine(_restartDelay));
            _audioSource.Play();
            PlayerWon?.Invoke(player);
        }

        private void OnDraw()
        {
            State = FieldState.Finished;
            StartCoroutine(RestartDelayedCoroutine(_restartDelay));
            Draw?.Invoke();
        }

        private void DrawLine(Vector2Int[] coords)
        {
            Vector3 first = _cells[coords[0].x, coords[0].y].transform.position;
            Vector3 second = _cells[coords[1].x, coords[1].y].transform.position;
            Vector3 third = _cells[coords[2].x, coords[2].y].transform.position;
            
            Vector3[] points = { first, second, third };
            
            _winLine.SetPositions(points);
        }

        private void InitCells()
        {
            Cell[] cellChildren = GetComponentsInChildren<Cell>();

            if (cellChildren.Length != 9) throw new ArgumentOutOfRangeException($"Field must have 9 cells. Found {cellChildren.Length} cells.");
            
            cellChildren = cellChildren
                .OrderBy(cell => cell.transform.position.x)
                .ThenBy(cell => cell.transform.position.y)
                .ToArray();
            
            int currentChildren = 0;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _cells[i, j] = cellChildren[currentChildren];
                    currentChildren++;
                }
            }
        }

        private static IEnumerator RestartDelayedCoroutine(float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}