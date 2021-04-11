using System;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// A component that emits events if a Renderer became visible or invisible
    /// </summary>
    public class VisibilityDetector : MonoBehaviour
    {
        public Action onBecameInvisible;
        public Action onBecameVisible;

        /// <summary>
        /// OnBecameInvisible is a Unity runtime function called when a GameObject with a Renderer has been culled,
        /// it is called once
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        public void OnBecameInvisible()
        {
            onBecameInvisible?.Invoke();
        }

        
        /// <summary>
        /// OnBecameVisible is a Unity runtime function called when a GameObject with a Renderer is rendered again
        /// it is called once
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnBecameVisible()
        {
            onBecameVisible?.Invoke();
        }
    }
}