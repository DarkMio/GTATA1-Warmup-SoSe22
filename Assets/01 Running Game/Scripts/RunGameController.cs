using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    /// <summary>
    /// Responsible for spawning world components - in this case setting up TileRows
    /// </summary>
    public class RunGameController : MonoBehaviour
    {
        public IEnumerable<TileRow> TileRows => rows; 
        public bool hasGameStarted;

        [SerializeField] private Vector2 minMaxGameSpeed;
        [SerializeField] private Vector2Int minMaxGridHeight;
        [SerializeField] private float spawnFirstRowLocation;
        [SerializeField] private TileRow rowPrefab;
        /// <summary>
        /// The grid size is the world size of a Tile, which can be used to find the center for the next Tile
        /// </summary>
        private Vector2 gridSize;
        private float currentGameSpeed;
        private Queue<TileRow> rows;
        private float startTime;

        /// <summary>
        /// Public interface to indicate the start of a game
        /// </summary>
        public void StartGame()
        {
            hasGameStarted = true;
            startTime = Time.time;
        }

        /// <summary>
        /// Awake is a Unity runtime function called first when a component is constructed and deserialized
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Awake()
        {
            gridSize = rowPrefab.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size;
            rows = new Queue<TileRow>();

            for (var i = 0; i < 30; i++)
            {
                AddRow();
            }
        }

        /// <summary>
        /// OnDisable is a Unity runtime function called called when the GameObject or the component is disabled
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnDisable()
        {
            // implicitly driving this boolean decouples components but in more complex behaviours could yield to
            // unexpected side effects.
            hasGameStarted = false;
        }

        /// <summary>
        /// OnDrawGizmosSelected is a Unity editor function called when the attached GameObject is selected and used to
        /// display debugging information in the Scene view
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            var center = transform.position;
            center.x += spawnFirstRowLocation * transform.lossyScale.x;
            Gizmos.DrawWireCube(center,
                new Vector2(gridSize.x * transform.lossyScale.x, gridSize.y * 10 * transform.lossyScale.y));
        }

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void Update()
        {
            // no speedup when the game hasn't started yet.
            if (!hasGameStarted)
            {
                return;
            }
            // the time a new game has begun
            var gameStartTime = (Time.time - startTime);
            var delta = minMaxGameSpeed.y - minMaxGameSpeed.x;
            // within 1000 seconds we ramp up the difficulty
            // this works by using the minimum + delta * [0...1]
            currentGameSpeed = minMaxGameSpeed.x + delta * Mathf.Clamp01(gameStartTime / 1000);
            Time.timeScale = currentGameSpeed;
        }

        /// <summary>
        /// Method to create a new TileRow after all other tile rows
        /// </summary>
        private void AddRow()
        {
            // create a new instance of a blueprint of a GameObject
            var newRow = Instantiate(rowPrefab, transform);
            var rowPosition = newRow.transform.localPosition;
            // either set the position one slot after the last TileRow or if we don't have any rows, at the first position
            rowPosition.x = rows.Count > 0 ? rows.Last().transform.localPosition.x + gridSize.x : spawnFirstRowLocation;

            // randomize if this is a high or low tile
            newRow.isInHighPosition = IsHighTile();
            // depending if it's in high position, set the offset of the tile vertically correct
            rowPosition.y += newRow.isInHighPosition
                ? minMaxGridHeight.y * gridSize.y
                : minMaxGridHeight.x * gridSize.y;

            // set the new position
            newRow.transform.localPosition = rowPosition;
            // add the callback listener when this tile gets out of view
            newRow.VisibilityDetector.onBecameInvisible += () => RemoveRow(newRow);
            // enqueue it in the tiles we handle
            rows.Enqueue(newRow);
        }

        /// <summary>
        /// Randomize out if the tile is high or low
        /// </summary>
        private bool IsHighTile()
        {
            // when the game hasn't started, we only spawn low tiles
            if (!hasGameStarted)
            {
                return false;
            }
            // take the last 3 entries, assign them 1 or 0 if they're high or low, take the Min of two adjacent tiles
            var results = rows
                .Skip(rows.Count - 4)
                .Select(x => x.isInHighPosition ? 1 : 0)
                .Select((x, y) => Mathf.Min(x, y));
            // if there are no two low tiles adjacent to each other, spawn a low tile
            if (results.Any(x => x != 0))
            {
                return false;
            }

            var delta = minMaxGameSpeed.y - minMaxGameSpeed.x;
            // value that goes from 0.25...1 the closer we get to max game speed
            var difficulty = 0.25f + (currentGameSpeed - minMaxGameSpeed.x) / (delta) * 0.75f;
            // the higher the difficulty number is, the more high tiles it will spawn
            return Random.value < difficulty;
        }

        /// <summary>
        /// When a row is marked for deletion
        /// </summary>
        private void RemoveRow(TileRow row)
        {
            // UnityEngine.Object.Destroy removes whatever is given out of the runtime context, effectively deleting it
            Destroy(row.gameObject);
            // we implicitly assume that the row to be deleted is in front the queue
            // this could very well cause wrong behaviour and the interface is ambiguous
            rows.Dequeue();
            // and then we add for each tile a new one, so we don't run out of tiles
            AddRow();
        }
    }
}