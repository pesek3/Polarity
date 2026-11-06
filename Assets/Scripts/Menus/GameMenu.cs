using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;

    public void play()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void quit()
    {
        Application.Quit();
    }  

}
