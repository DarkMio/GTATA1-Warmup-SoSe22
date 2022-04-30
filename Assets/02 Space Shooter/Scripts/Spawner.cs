using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject upgradePrefab;
    public float respawnTime= 1.0f;
    private Vector2 screenBounds;

    // Use this for initialization
    void Start(){
            screenBounds=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            StartCoroutine(upgradeWave());
    }

    private void spawnUpgrade(){
        GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
        upgrade.transform.position=new Vector2(screenBounds.x*2, Random.Range(-screenBounds.y, screenBounds.y));
            
    }
    IEnumerator upgradeWave(){
        while(true){
            yield return new WaitForSeconds(respawnTime);
            spawnUpgrade();
        }
    }
    }