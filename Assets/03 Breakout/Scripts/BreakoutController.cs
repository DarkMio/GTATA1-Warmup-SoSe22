using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class BreakoutController : MonoBehaviour
    {
        [SerializeField] [Range(0, 1000)] private float speed;
        [SerializeField] [Range(0, 384)] private float pedalSize;
        [SerializeField] private List<Ball> balls;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject button;
        private CapsuleCollider2D collider;
        private RectTransform rectTransform;
        private Rigidbody2D rigidBody;
        private int upgradeCollisionId;
        private WinLose winLose;


        /// <summary>
        /// FixedUpdate is a Unity runtime function called *every physics* frame
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void FixedUpdate()
        {
            // set the pedal graphic and collider size based on the pedal size property
            rectTransform.sizeDelta = new Vector2(pedalSize, rectTransform.sizeDelta.y);
            collider.size = new Vector2(pedalSize, collider.size.y);
            
            var transformPosition = transform.position;
            var pedalPosition = transformPosition;
            var mousePosition = Input.mousePosition;
            // calculate the distance on the horizontal axis between mouse and center of the pedal
            var mouseDistance = pedalPosition.x - mousePosition.x;
            var direction = Mathf.Sign(mouseDistance);
            
            var newPositionX = direction > 0
                ? Mathf.Max(mousePosition.x, pedalPosition.x - speed * Time.deltaTime)
                : Mathf.Min(mousePosition.x, pedalPosition.x + speed * Time.deltaTime);
            // updating the rigid body so the physics engine works correctly
            rigidBody.MovePosition(new Vector2(newPositionX, transformPosition.y));

            // remove all destroyed balls
            balls = balls.Where(x => x != null).ToList();
            // if there are no balls left, the game is lost
            if (balls.Count == 0)
            {
                // maybe this is a good entry point for a loss system, similarly no bricks -> win
                winLose.WinSetup(false);
                button.SetActive(true);
                Debug.Log("Game Over!");
                Time.timeScale = 0f;
            }
            Debug.Log(GameObject.FindGameObjectsWithTag("Bricks").Length);
            if(GameObject.FindGameObjectsWithTag("Bricks").Length <= 0){ //Change to 73 or 75 to test win condition
                winLose.WinSetup(true);
                button.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        private void OnEnable()
        {   // set some references once
            rigidBody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CapsuleCollider2D>();
            winLose = background.GetComponent<WinLose>();
            rectTransform = GetComponent<RectTransform>();
            button.SetActive(false);
            upgradeCollisionId = LayerMask.NameToLayer("Upgrade");
        }

        /// <summary>
        /// Collision event triggered from Unity when two colliders (in the physics collision layer matrix) collide with
        /// each other 
        /// </summary>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == upgradeCollisionId)
            {
                var upgradeType = other.gameObject.GetComponent<Upgrade>().Type;
                // control code for upgrades
                switch (upgradeType)
                {
                    case UpgradeType.BiggerPedal:
                        pedalSize += 16;
                        break;
                    case UpgradeType.SmallerPedal:
                        pedalSize -= 16;
                        break;
                    case UpgradeType.ExtraBall:
                        var ball = Instantiate(balls[0], transform.parent);
                        ball.transform.position = balls[0].transform.position;
                        ball.RigidBody.velocity = Vector2.Reflect(balls[0].RigidBody.velocity, Vector2.up);
                        balls.Add(ball);
                        break;
                }

                pedalSize = Mathf.Clamp(pedalSize, 48, 384);
                Destroy(other.gameObject);
            }
        }
    }
}