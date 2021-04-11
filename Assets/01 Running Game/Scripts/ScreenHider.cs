using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Helper component to show/hide a UI element
    /// </summary>
    public class ScreenHider : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        private float targetVisibility;
        private float lastHide;

        /// <summary>
        /// Awake is a Unity runtime function called first when a component is constructed and deserialized
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Awake()
        {
            targetVisibility = 1f;
        }

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Update()
        {
            // When nobody requested to hide the screen for 30s, it is being shown
            if (Time.time - lastHide > 30f)
            {
                Show();
            }

            // MoveTowards is a helper interpolation function that "moves a number towards another one"
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetVisibility, 1f * Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Show the target screen
        /// </summary>
        public void Show()
        {
            targetVisibility = 1f;
        }
        
        /// <summary>
        /// Hide the target screen
        /// </summary>
        public void Hide()
        {
            targetVisibility = 0f;
            // this implicitly resets the timer to show a screen again
            lastHide = Time.time;
        }
    }
}