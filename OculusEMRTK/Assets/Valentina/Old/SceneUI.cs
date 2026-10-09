using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUI : MonoBehaviour
{

    private GameObject[] scrollCreateScene;
    private GameObject[] secondCamera;
    // Start is called before the first frame update
    void Start()
    {
        // prende tutti i menu a scroll con il rispettivo tag
        scrollCreateScene = GameObject.FindGameObjectsWithTag("Scroll");
        secondCamera = GameObject.FindGameObjectsWithTag("SecondCamera");

        foreach (GameObject go in scrollCreateScene)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in secondCamera)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // attiva e disattiva, premendo su Create Scene le 3 scroll con gli elementi che si possono aggiungere alla scena
    public void OpenScrollbarObject ()
    {
        foreach (GameObject go in scrollCreateScene)
        {
            if (go.activeSelf)
            {
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
            }
        }
            
    }

    public void OpenCamera ()
    {
        foreach (GameObject go in secondCamera)
        {
            if (go.activeSelf)
            {
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
            }
        }
    }

}
