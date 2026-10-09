using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadCreateStoryboard()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
