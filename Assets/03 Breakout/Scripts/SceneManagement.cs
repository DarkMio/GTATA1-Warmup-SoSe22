using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private int SceneToLoad;
    public GameObject LoseScreen,WinScreen;
    private int Length;
    
    private void Start()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
    // count how many bricks to hit to win the game
    if(GameObject.FindGameObjectsWithTag("Bricks").Length <= 65) {
    // show winning screen
    WinScreen.SetActive(true);
    //load new scene
    SceneToLoad = SceneManager.GetActiveScene().buildIndex+1;
    SceneManager.LoadScene(SceneToLoad);
    // unpause the game
    Time.timeScale=1;
    }
    }
    
    // change scene after click button
    void ChangeScene()
    {
        SceneToLoad = SceneManager.GetActiveScene().buildIndex+1;
        SceneManager.LoadScene(SceneToLoad);
        Time.timeScale=1;
    }
}
