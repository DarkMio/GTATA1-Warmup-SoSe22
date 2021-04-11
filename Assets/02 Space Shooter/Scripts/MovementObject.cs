using Unity.Mathematics;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// A physics like movement object that can be pushed around, has angular and velocity drag
    /// </summary>
    public class MovementObject : MonoBehaviour
    {
        [SerializeField] private Vector3 angularVelocity;
        [SerializeField] private Vector3 movementVelocity;

        [SerializeField] [Range(0, 1)] private float angularDrag;
        [SerializeField] [Range(0, 1)] private float movementDrag;

        [SerializeField] [Range(0, 50)] private float maximumSpeed;
        [SerializeField] [Range(0, 50)] private float maximumRotation;

        private SpriteRenderer spriteRenderer;

        public Vector3 CurrentVelocity => movementVelocity;
        public Vector3 CurrentAngularSpin => angularVelocity;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            angularVelocity *= 1 - angularDrag * (1 - Time.deltaTime);
            movementVelocity *= 1 - movementDrag * (1 - Time.deltaTime);

            transform.position += movementVelocity * Time.deltaTime;
            transform.rotation *= quaternion.Euler(angularVelocity * (Time.deltaTime * Mathf.PI));

            spriteRenderer.RepositionSpriteRendererInFrustum(Camera.current);
        }

        /// <summary>
        /// Impulse force vectors onto an object, permanently changing its velocity and spin
        /// </summary>
        public void Impulse(Vector3 movement, Vector3 rotation)
        {
            movementVelocity += movement;
            angularVelocity += rotation;

            movementVelocity = movementVelocity.normalized * Mathf.Min(maximumSpeed, movementVelocity.magnitude);
            angularVelocity = angularVelocity.normalized * Mathf.Min(maximumRotation, angularVelocity.magnitude);
        }

        /// <summary>
        /// Immediately displace an object by two force vectors  
        /// </summary>
        public void Add(Vector3 movement, Vector3 rotation)
        {
            transform.position += movement;
            transform.rotation *= quaternion.Euler(rotation);
        }
    }
}