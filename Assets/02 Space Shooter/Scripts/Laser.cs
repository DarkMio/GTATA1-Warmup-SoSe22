using UnityEngine;
using Scripts;

namespace Scripts
{
    /// <summary>
    /// Simple component that regularly checks if it intersects with an asteroid and moves forward, speeding up
    /// </summary>
    public class Laser : MonoBehaviour
    {
        private static AsteroidGameController _runGameController;
        public Vector3 initialVelocity;
        [SerializeField] private MovementObject movement;
        [SerializeField] private float bulletSpeed;
        [SerializeField] [Range(0, 5)] private float lifetime;
        private SpriteRenderer sprite;
        // declaring variable of Shipcontroller
        ShipController Upgrade;

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            Destroy(gameObject, lifetime);
            movement.Impulse(initialVelocity, Vector3.zero);
            if (_runGameController == null) _runGameController = FindObjectOfType<AsteroidGameController>();
            //access value
            Upgrade = FindObjectOfType<ShipController>();
        }

        // make the laser goes faster after we took the 
        private void Update()
        {
            if(Upgrade.Hit == false)
            {
            movement.Impulse(transform.up * bulletSpeed * Time.deltaTime, Vector3.zero);
            }
            else if(Upgrade.Hit == true)
            {
            movement.Impulse(transform.up * bulletSpeed * Time.deltaTime*20, Vector3.zero);
            }
        }

        /// <summary>
        /// Late Update is a Unity Runtime function that gets executed after Update (and Coroutines)
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void LateUpdate()
        {
            _runGameController.LaserIntersection(sprite);
        }
    }
}