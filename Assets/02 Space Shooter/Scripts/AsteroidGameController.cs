using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Scripts
{
    /// <summary>
    /// Game controller handling asteroids and intersection of components.
    /// </summary>
    public class AsteroidGameController : MonoBehaviour
    {
        public Asteroid[] bigAsteroids;
        public Asteroid[] mediumAsteroids;
        public Asteroid[] smallAsteroids;
       // public 

        [SerializeField] private Vector3 maximumSpeed, maximumSpin;
        [SerializeField] private PlayerShip playerShip;
        [SerializeField] private Transform spawnAnchor;
        [SerializeField] private GameObject background; //Background for winlose screen

        private List<Asteroid> activeAsteroids;
        private Random random;
        private int shipLifes; //ship lifes
        private WinLoseScreen winLose; // winlose
        private HealthPoint healthPoint;
        
        bool shipDestroyed=false; //check if destroyed


        private void Start()
        {
            winLose=background.GetComponent<WinLoseScreen>();//added
            healthPoint=FindObjectOfType<HealthPoint>();//added
            shipLifes=5;
            activeAsteroids = new List<Asteroid>();
            random = new Random();
            // spawn some initial asteroids
            for (var i = 0; i < 5; i++)
            {
                SpawnAsteroid(bigAsteroids, Camera.main.OrthographicBounds());
            }
        }
        //Update to check activeAsteroids, if 0 then game won
        private void Update(){
            healthPoint.onHit(shipLifes);

            if(activeAsteroids.Count==0){
                winLose.WinSetup(true);
            }
            if(shipDestroyed){
                winLose.WinSetup(false);
            }
        }

        //Health points increase if receive upgrade
        public void HealthIncrease(){
            shipLifes +=1;
        }


        /// <summary>
        /// Behaviour to spawn an asteroid within the screen
        /// If there is a parent given, the velocity of that parent is put into consideration
        /// </summary>
        private void SpawnAsteroid(Asteroid[] prefabs, Bounds inLocation, Asteroid parent = null)
        {
            // get a random prefab from the list
            var prefab = prefabs[random.Next(prefabs.Length)];
            // create an instance of the prefab
            var newObject = Instantiate(prefab, spawnAnchor);
            // position it randomly within the box given (either the parent asteroid or the camera)
            newObject.transform.position = RandomPointInBounds(inLocation);
            // we can randomly invert the x/y scale to mirror the sprite. This creates overall more variety
            newObject.transform.localScale = new Vector3(UnityEngine.Random.value > 0.5f ? -1 : 1,
                UnityEngine.Random.value > 0.5f ? -1 : 1, 1);
            // renaming, I'm also sometimes lazy typing
            var asteroidSprite = newObject.spriteRenderer;

            // try to position the asteroid somewhere where it doesn't hit the player or another active asteroid
            for (var i = 0;
                playerShip.shipSprite.bounds.Intersects(asteroidSprite.bounds) ||
                activeAsteroids.Any(x => x.GetComponent<SpriteRenderer>().bounds.Intersects(asteroidSprite.bounds));
                i++)
            {
                // give up after 15 tries.
                if (i > 15)
                {
                    DestroyImmediate(newObject.gameObject);
                    return;
                }

                newObject.transform.position = RandomPointInBounds(inLocation);
            }
            
            // take parent velocity into consideration
            if (parent != null)
            {
                var offset = parent.transform.position - newObject.transform.position;
                var parentVelocity = parent.movementObject.CurrentVelocity.magnitude *
                                     (UnityEngine.Random.value * 0.4f + 0.8f);
                newObject.movementObject.Impulse(offset.normalized * parentVelocity, RandomizeVector(maximumSpeed));
            }
            // otherwise randomize just some velocity
            else
            {
                newObject.movementObject.Impulse(RandomizeVector(maximumSpeed), RandomizeVector(maximumSpin));
            }

            activeAsteroids.Add(newObject);
        }


        /// <summary>
        /// Checks if a laser is intersecting with an asteroid and executes gameplay behaviour on that
        /// </summary>
        public void LaserIntersection(SpriteRenderer laser)
        {
            // go through all asteroids, check if they intersect with a laser and stop after the first
            var asteroid = activeAsteroids
                .FirstOrDefault(x => x.GetComponent<SpriteRenderer>().bounds.Intersects(laser.bounds));

            // premature exit: this laser hasn't hit anything
            if (asteroid == null)
            {
                return;
            }
            
            // otherwise remove the asteroid from the tracked asteroid
            activeAsteroids.Remove(asteroid);
            var bounds = asteroid.spriteRenderer.bounds;
            // get the correct set of prefabs to spawn asteroids in place of the asteroid that now explodes
            var prefabs = asteroid.asteroidSize switch
            {
                AsteroidSize.Large => mediumAsteroids,
                AsteroidSize.Medium => smallAsteroids,
                _ => null
            };
            // remote the asteroid gameobject with all its components
            Destroy(asteroid.gameObject);
            
            // premature exit: we have no prefabs (ie: small asteroids exploding)
            if (prefabs == null)
            {
                return; 
            }

            // randomize two to six random asteroids
            var objectCountToSpawn = (int) (UnityEngine.Random.value * 4 + 2);
            for (var i = 0; i < objectCountToSpawn; i++) // put i<0 to stop new smaller asteroid from spawning, jst for test
            {
                SpawnAsteroid(prefabs, bounds);
            }
            
            // oh, also get rid of the laser now
            Destroy(laser.gameObject);
        }

        public void ShipIntersection(SpriteRenderer ship)
        {
            // :thinking: this could be solved very similarly to a laser intersection Hmmmmmmmm
            var asteroid = activeAsteroids
                .FirstOrDefault(x => x.GetComponent<SpriteRenderer>().bounds.Intersects(ship.bounds));

            // premature exit: this ship hasn't hit anything
            if (asteroid == null)
            {
                return;
            }
            shipLifes -=1;// reduce health everytime ship got hit by the asteroid
            if(shipLifes>0)
            {
                    // otherwise remove the asteroid from the tracked asteroid
                activeAsteroids.Remove(asteroid);
                var bounds = asteroid.spriteRenderer.bounds;
                // get the correct set of prefabs to spawn asteroids in place of the asteroid that now explodes
                var prefabs = asteroid.asteroidSize switch
                {
                    AsteroidSize.Large => mediumAsteroids,
                    AsteroidSize.Medium => smallAsteroids,
                    _ => null
                };
                // remote the asteroid gameobject with all its components
                Destroy(asteroid.gameObject);
                // premature exit: we have no prefabs (ie: small asteroids exploding)
                if (prefabs == null)
                {
                    return;
                }
                // randomize two to six random asteroids
                var objectCountToSpawn = (int) (UnityEngine.Random.value * 4 + 2);
                for (var i = 0; i < 0; i++) // put i<0 to stop new smaller asteroid from spawning, jst for test
                {
                    SpawnAsteroid(prefabs, bounds);
                }
            }else{
                    // oh, also get rid of the ship now
                Destroy(ship.gameObject); 
                shipDestroyed=true;
            }
                       
        }

        private static float RandomPointOnLine(float min, float max)
        {
            return UnityEngine.Random.value * (max - min) + min;
        }

        private static Vector2 RandomPointInBox(Vector2 min, Vector2 max)
        {
            return new Vector2(RandomPointOnLine(min.x, max.x), RandomPointOnLine(min.y, max.y));
        }

        private static Vector2 RandomPointInBounds(Bounds bounds)
        {
            return RandomPointInBox(bounds.min, bounds.max);
        }

        private static Vector3 RandomizeVector(Vector3 maximum)
        {
            // that is an inline method - it's good enough to just get a float [-1...+1]
            float RandomValue()
            {
                return UnityEngine.Random.value - 0.5f * 2;
            }

            maximum.Scale(new Vector3(RandomValue(), RandomValue(), RandomValue()));
            return maximum;
        }
    }
}