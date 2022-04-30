using UnityEngine;
using Scripts;

namespace Scripts
{
    /// <summary>
    /// Simple component to create a laser and shoot it forward 
    /// </summary>
    public class Gun : MonoBehaviour
    {
        [SerializeField] public Laser laserPrefab;
        [SerializeField] public Laser laserPrefab2;
        private PlayerShip ship;
        ShipController State;
        private void Start()
        {
            ship = GetComponent<PlayerShip>();
            State = FindObjectOfType<ShipController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
        }

        private void Fire()
        {
            Debug.Log(State.Hit);
            if(State.Hit == false){
            laserPrefab.initialVelocity = ship.movementObject.CurrentVelocity;
            Instantiate(laserPrefab, transform.position, transform.rotation);
        }
            else if(State.Hit == true)
        {
            laserPrefab2.initialVelocity = ship.movementObject.CurrentVelocity;
            Instantiate(laserPrefab2, transform.position, transform.rotation);
        }
        
        }
    }
}