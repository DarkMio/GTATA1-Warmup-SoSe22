using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Container component to keep references to common components on a ship
    /// </summary>
    internal class PlayerShip : MonoBehaviour
    {
        public MovementObject movementObject;
        public SpriteRenderer shipSprite;
    }
}