using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Simple component moving a transform forward (local X axis of the object)
    /// </summary>
    public class ForwardMover : MonoBehaviour
    {
        public float speed;

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Update()
        {
            // calculating the advancement with deltaTime gets the advancement per unit of time
            // (which is in seconds)
            transform.position += transform.right * (speed * Time.deltaTime);
            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }
}