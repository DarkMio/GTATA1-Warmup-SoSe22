using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Game Manager does the runtime management of the game, like start, stop, lose, ...
    /// In the best case scenario components can rely on the behaviour of other components and keep their own
    /// implementation small and concise.
    ///
    /// An anti example: It's not impossible to manage the spawning of blocks, the character movement, intersections,
    /// UI states in a single "god" class component, debugging it would be a nightmare however
    /// </summary>
    public class RunGameManager : MonoBehaviour
    {
        [SerializeField] private RunCharacterController runCharacterController;
        [SerializeField] private RunGameController gameController;
        [SerializeField] private ScreenHider screenHider;
        [SerializeField] private float hitThreshold = 0.1f;
        private bool hasStarted;

        /// <summary>
        /// OnEnable is a Unity runtime function called from Unity after "Awake" and _every_ time the component gets
        /// enabled/disabled by either component.enabled = true or the GameObject becoming active
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnEnable()
        {
            // we can try to unsubscribe first, avoiding double subscription by accident
            // unsubscription when not subscribed ends in doing nothing and doesn't break
            runCharacterController.onJump -= OnJump;
            runCharacterController.onJump += OnJump;
        }

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        public void Update()
        {
            // when the game has started, we're seeking if the character bounding box hit a ground tile
            if (hasStarted && gameController.TileRows.Any(IntersectionHit))
            {
                // A coroutine will be discussed a bit later in the lecture, but it's exactly what the name says:
                // 
                // A coroutine runs next to the game loop and is useful for all operations that have a well known time box.
                // If "while(true)" is analogue to this "Update" function
                // then "for(var i = 0; i < count; i++)" is like a Coroutine
                StartCoroutine(ResetGame());
            }
        }

        /// <summary>
        /// Resets the game, displaying screens and slowing down time for a brief moment 
        /// 
        /// To be used in an Coroutine, this function is a generator (return IEnumerator) and has special syntactic
        /// sugar with "yield return"
        /// </summary>
        private IEnumerator ResetGame()
        {
            hasStarted = false;
            Time.timeScale = 0.5f;
            gameController.enabled = false;
            screenHider.Show();
            // WaitForSecondsRealtime ignores Time.timeScale and waits a real second, not game time second
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
            gameController.enabled = true;
        }

        /// <summary>
        /// Composited function to check if a TileRow intersects with a character
        /// </summary>
        private bool IntersectionHit(TileRow row)
        {
            // sometimes the selected row already has been "deleted" in the sense of removal from the scene.
            // In Unity the MonoBehaviour Null Equality operator is overwritten to make this check like this:
            if (row == null)
            {
                return false;
            }
            // does a characters bounding box intersects enough of a tiles bounding box? 
            return Intersection.NormalizedIntersectionAmount(runCharacterController, row) > hitThreshold;
        }

        /// <summary>
        /// Referencable action callback whenever a jump happens
        /// </summary>
        private void OnJump()
        {
            // we just overwrite the state - whatever it was.
            // these interacting components are programmed to handle that kind of overwriting, making them less
            // error prone by being seemingly "stateless"
            hasStarted = true;
            gameController.StartGame();
            screenHider.Hide();
        }
    }
}