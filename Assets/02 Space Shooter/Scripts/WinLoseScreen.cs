using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseScreen : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject background;

    public void Start(){
        background.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void WinSetup(bool winLose){
        
        Debug.Log("CALLED");
        background.SetActive(true);
        if(winLose){
        loseScreen.SetActive(false);
        winScreen.SetActive(true);
        }else{            
        winScreen.SetActive(false);
        loseScreen.SetActive(true);
        }
    }
}
