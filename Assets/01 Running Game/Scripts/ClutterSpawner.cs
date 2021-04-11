using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Spawns random clutter on the screen as backdrop
    /// </summary>
    public class ClutterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject clutterPrefab;
        [SerializeField] private Sprite[] clutterSprites;
        [SerializeField] private Camera mainCamera;
        [SerializeField] [Range(0, 10)] private float spawnDuration;
        [SerializeField] private Vector2 minMaxSpeed;
        [SerializeField] private Vector2 minMaxHeight;

        private float maxSpriteHeight;

        /// <summary>
        /// Awake is a Unity runtime function called first when a component is constructed and deserialized
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Awake()
        {
            // retrieve the maximum height of a sprite
            maxSpriteHeight = clutterSprites.Max(x => x.bounds.size.y);
            // Coroutines will be discussed later in the lectures
            StartCoroutine(SpawnClutterRoutine());
            // spawn some initial clutter
            for (var i = 0; i < 10; i++)
            {
                SpawnClutter(false);
            }
        }

        /// <summary>
        /// OnDrawGizmosSelected is a Unity editor function called when the attached GameObject is selected and used to
        /// display debugging information in the Scene view
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (mainCamera == null)
            {
                return;
            }

            var groundPosition = mainCamera.transform.position + new Vector3(0, minMaxHeight.x, 0);
            var boxSize = new Vector3(mainCamera.orthographicSize * 2 * mainCamera.aspect, minMaxHeight.y - minMaxHeight.x);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundPosition, boxSize);
        }
        
        /// <summary>
        /// Spawns repeatedly clutter outside of the camera rectangle
        /// 
        /// To be used in an Coroutine, this function is a generator (return IEnumerator) and has special syntactic
        /// sugar with "yield return"
        /// </summary>
        private IEnumerator SpawnClutterRoutine()
        {
            while (true)
            {
                SpawnClutter(true);
                yield return new WaitForSeconds(spawnDuration);
            }
        }

        private void SpawnClutter(bool outsideCamera)
        {
            // create a new GameObject based on the blueprint of another - as children of this GameObject
            var go = Instantiate(clutterPrefab, transform);
            // select a random sprite
            var clutterSprite = clutterSprites[(int) (Random.value * clutterSprites.Length)];
            
            var xPosition = 0f;
            
            var horizontalCameraSize = mainCamera.orthographicSize * 2 * mainCamera.aspect;
            // either randomly within the camera width or just outside of it
            xPosition += outsideCamera
                ? horizontalCameraSize
                : horizontalCameraSize * Random.value - horizontalCameraSize / 2;
            var yPosition = minMaxHeight.x + (minMaxHeight.y - minMaxHeight.x) * Random.value;
            // depth is based on the height - so smaller sprites get pushed into the background
            var localPosition = new Vector3(xPosition, yPosition, maxSpriteHeight / clutterSprite.bounds.size.y);
            // casting twice create a Vector with the z component removed
            go.transform.position = localPosition + (Vector3) (Vector2) mainCamera.transform.position;

            var speedDelta = minMaxSpeed.y - minMaxSpeed.x;
            // the sprite size speeds up sprites by about 10%
            var sizeRandomness = (clutterSprite.bounds.size.y / maxSpriteHeight) * (Random.value * 0.1f + 0.9f);
            go.GetComponent<ForwardMover>().speed = minMaxSpeed.x + speedDelta * sizeRandomness;
            // assign the randomized sprite
            go.GetComponent<SpriteRenderer>().sprite = clutterSprite;
        }
    }
}