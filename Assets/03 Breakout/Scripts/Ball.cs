using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Component to keep a Ball in control and speed
    /// </summary>
    public class Ball : MonoBehaviour
    {
        private static int brickLayerId;
        private static int deathLayerId;
        [SerializeField] [Range(0, 100)] private float initialSpeed;
        private bool hasStarted;
        public Rigidbody2D RigidBody { get; private set; }

        private void FixedUpdate()
        {
            // sometimes odd angles cause the ball to decelerate, which is fixed by setting it to a fixed magnitude
            RigidBody.velocity = RigidBody.velocity.normalized * (initialSpeed * 10);

            // preventing to accelerate the ball downwards after the game has started
            if (hasStarted)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                RigidBody.velocity = Vector2.down * (initialSpeed * 10);
                hasStarted = true;
            }
        }

        private void OnEnable()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            brickLayerId = LayerMask.NameToLayer("Brick");
            deathLayerId = LayerMask.NameToLayer("Death");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == brickLayerId)
            {
                Destroy(other.gameObject);
            }

            if (other.gameObject.layer == deathLayerId)
            {
                Destroy(gameObject);
            }
        }
    }
}