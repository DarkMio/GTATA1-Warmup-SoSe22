using UnityEngine;
using Scripts;

namespace Scripts
{
    /// <summary>
    /// Very basic rotational ship controller, adding force into forward direction
    /// </summary>
    public class ShipController : MonoBehaviour
    {
        AsteroidGameController Controller;
        public float Health = 100f;
        [SerializeField] [Range(0, 10)] private float speed;
        [SerializeField] [Range(0, 10)] private float rotationSpeed;
        private MovementObject playerShip;
        public GameObject Ship,WinScreen,LoseScreen;
        public bool Hit=false;

        private void Start()
        {   
            WinScreen.SetActive(false);
            LoseScreen.SetActive(false);
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            playerShip = GetComponent<MovementObject>();
            Controller = FindObjectOfType<AsteroidGameController>();
        }

        private void Update()
        {
            // clockwise rotation is negative euler z rotation, anti-clockwise is positive
            var rotation = 0f;
            if (Input.GetKey(KeyCode.D)) rotation += 1f;
            if (Input.GetKey(KeyCode.A)) rotation -= 1f;

            var forward = 0f;
            if (Input.GetKey(KeyCode.W)) forward += 1f;

            if (Input.GetKey(KeyCode.S)) forward -= 1f;

            playerShip.Impulse(transform.up * (Time.deltaTime * speed * forward), Vector3.zero);
            playerShip.Add(Vector3.zero, new Vector3(0, 0, rotation * Time.deltaTime * rotationSpeed * 3.6f));
            // win condition
            if(Controller.Count>20)
            {
                //show win screen
                WinScreen.SetActive(true);
            }
        }

    // detect Collision
    private void OnTriggerEnter2D (Collider2D target)
        {   
            //compare tag
            if(target.gameObject.CompareTag("Respawn"))
            {
            // health status
            Health = Health - 20;
            // if hitting the asteroid
            if(Health <= 0)
            {
            // ship goes invisible
            Ship.SetActive(false);
            // show win screen
            LoseScreen.SetActive(true);
            }
            // detect upgrade
            }else if(target.gameObject.CompareTag("Upgrade"))
            {
            // upgrade gameobject goes invisible
            target.gameObject.SetActive(false);
            Hit=true;
            }

        }
    }
}