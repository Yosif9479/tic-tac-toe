using UnityEngine;

namespace Runtime.Models
{
    public class WinCombination
    {
        public Vector2Int[] Coordinates { get; private set; }

        public WinCombination(Vector2Int[] coordinates)
        {
            Coordinates = coordinates;
        }
    }
}