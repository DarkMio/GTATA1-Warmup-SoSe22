using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Container component to hold important references
    /// </summary>
    public class Asteroid : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public MovementObject movementObject;
        public AsteroidSize asteroidSize;
    }
    public enum AsteroidSize
    {
        Large,
        Medium,
        Small
    }
}