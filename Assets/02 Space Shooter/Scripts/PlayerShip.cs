using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Container component to keep references to common components on a ship
    /// </summary>
    internal class PlayerShip : MonoBehaviour
    {
        private static AsteroidGameController _runGameController;
        public MovementObject movementObject;
        public SpriteRenderer shipSprite;

        private void Start(){
        shipSprite=GetComponent<SpriteRenderer>();
        if (_runGameController == null) _runGameController = FindObjectOfType<AsteroidGameController>();
    }
    private void Update(){
        _runGameController.ShipIntersection(shipSprite);
    }
    
    }
    
    
}