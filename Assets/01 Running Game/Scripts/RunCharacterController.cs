using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Controls the movement of the Character
    /// </summary>
    public class RunCharacterController : MonoBehaviour
    {
        public Animator animator,animator2;
        public Transform Transform => character;
        public Transform Transform2 => character2;
        public SpriteRenderer CharacterSprite => characterSprite;
        public SpriteRenderer CharacterSprite2 => characterSprite2;
        /// <summary>
        /// Since the Character controller takes responsibility for triggering Input events, it also emits an
        /// event when it does so
        /// </summary>
        public Action onJump;
        
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDuration;
        /// <summary>
        /// Unity handles Arrays and Lists in the inspector correctly (but not Maps, Dictionaries or other Collections)
        /// </summary>
        [SerializeField] private KeyCode[] jumpKeys;
        /// <summary>
        /// We don't require anything else from the Character than its transform
        /// </summary>
        [SerializeField] private Transform character, character2;
        [SerializeField] private SpriteRenderer characterSprite, characterSprite2;
        [SerializeField] private AnimationCurve jumpPosition;
        
        private bool canJump = true;

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Update()
        {
            if (!canJump)
            {
                return;
            }
            // here the input event counts - if there is any button pressed that were defined as jump keys, trigger a jump
            if (jumpKeys.Any(x => Input.GetKeyDown(x)))
            {   // first we disable the jump, then start the Coroutine that handles the jump and invoke the event
                canJump = false;
                FindObjectOfType<AudioManager>().Play("Jump");
                StartCoroutine(JumpRoutine());
                onJump?.Invoke();
                animator.SetBool("IsJumping", true);
                animator2.SetBool("IsJumping2", true);
            }
        }

        /// <summary>
        /// OnDrawGizmosSelected is a Unity editor function called when the attached GameObject is selected and used to
        /// display debugging information in the Scene view
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            var upScale = transform.lossyScale;
            upScale.Scale(transform.up);
            Gizmos.DrawLine(transform.position, Vector3.up * jumpHeight * upScale.magnitude);
        }

        /// <summary>
        /// Handles the jump of a character
        /// 
        /// To be used in an Coroutine, this function is a generator (return IEnumerator) and has special syntactic
        /// sugar with "yield return"
        /// </summary>
        private IEnumerator JumpRoutine()
        {
            // the time this coroutine runs
            var totalTime = 0f;
            // low position is assumed to be a (0, 0, 0)
            var highPosition = character.up * jumpHeight;
            var highPosition2 = character2.up * jumpHeight;
            while (totalTime < jumpDuration)
            {
                totalTime += Time.deltaTime;
                // what's the normalized time [0...1] this coroutine runs at
                var sampleTime = totalTime / jumpDuration;
                // Lerp is a Linear Interpolation between a...b based on a value between 0...1
                character.localPosition = Vector3.Lerp(Vector3.zero, highPosition, jumpPosition.Evaluate(sampleTime));
                character2.localPosition = Vector3.Lerp(Vector3.zero, highPosition, jumpPosition.Evaluate(sampleTime));
                // we enable jumping again after we're almost done to remove some "stuck" behaviour when landing down
                if (sampleTime > 0.95f)
                {
                    canJump = true;
                    animator.SetBool("IsJumping", false);
                    animator2.SetBool("IsJumping2",false);
                }
                // yield return null waits a single frame
                yield return null;
            }
        }
    }
}