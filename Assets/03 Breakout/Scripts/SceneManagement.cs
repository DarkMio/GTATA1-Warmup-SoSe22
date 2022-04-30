using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private int SwitchScene;
    private int Length;
    private WinLose winLose;
    [SerializeField] private GameObject background;

    private void Start()
    {
        winLose=background.GetComponent<WinLose>();
    }
    // Update is called once per frame
    private void Update()
    {
    }


    public void ChangeScene()
    {
        winLose.ClearWindow();
        SwitchScene = SceneManager.GetActiveScene().buildIndex+1;
        SceneManager.LoadScene(SwitchScene);
        Time.timeScale=1;
    }
}