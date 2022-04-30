using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{   
    [SerializeField] public GameObject[] UpgradePrefab;
    [SerializeField] float SpawnInterval= 0.5f;
    [SerializeField] float min;
    [SerializeField] float max;

    // Use this for initialization
    void Start(){
            StartCoroutine(UpgradesSpawn());
    }

    IEnumerator UpgradesSpawn()
    {
        while(true)
        {
            // create random number
            var rand= Random.Range(min,max);
            var position= new Vector3(rand, transform.position.y);
            // spawn obejct
            GameObject upgrade = Instantiate(UpgradePrefab[Random.Range(0, UpgradePrefab.Length)],
            position, Quaternion.identity);
            yield return new WaitForSeconds(SpawnInterval);
            // destory object
            Destroy(gameObject,5f);
        }
    }
    }


