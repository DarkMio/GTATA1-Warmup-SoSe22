using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Very basic rotational ship controller, adding force into forward direction
    /// </summary>
    public class ShipController : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float speed;
        [SerializeField] [Range(0, 10)] private float rotationSpeed;
        private MovementObject playerShip;
        private AsteroidGameController healthPoint;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            playerShip = GetComponent<MovementObject>();
            healthPoint=FindObjectOfType<AsteroidGameController>();
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
        }

        private void OnTriggerEnter2D(Collider2D target){
            target.gameObject.SetActive(false);
            healthPoint.HealthIncrease();
            Debug.Log("HIT");
        }
    }
}