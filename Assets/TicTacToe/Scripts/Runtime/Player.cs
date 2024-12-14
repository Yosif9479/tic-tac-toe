using System;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class Player
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color;
        
        public Sprite Icon => _icon;
        public Color Color => _color;
    }
}