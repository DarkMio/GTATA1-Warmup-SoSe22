using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpgrade : MonoBehaviour
{
    //Upgrade to increase LIFEPOINTS
    public float speed = 1.0f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("created");
        rb = this.GetComponent<Rigidbody2D>(); 
        rb.velocity=new Vector2(-speed,0);
        screenBounds=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -screenBounds.x){
            Destroy(this.gameObject);
        }
    }
}
