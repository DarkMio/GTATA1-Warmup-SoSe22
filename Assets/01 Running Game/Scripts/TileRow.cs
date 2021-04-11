using System;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// A game component that  merely holds references to important components, makes access cleaner and easier
    /// </summary>
    public class TileRow : MonoBehaviour
    {
        [SerializeField] private VisibilityDetector visibilityDetector;
        [SerializeField] private SpriteRenderer topSprite; 
        public bool isInHighPosition;
        public VisibilityDetector VisibilityDetector => visibilityDetector;
        public SpriteRenderer TopSprite => topSprite;
    }
}