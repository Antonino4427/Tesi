using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public GameObject objectManagerVR;

    public GameObject emptyPlane;

    public GameObject tabletop;

    public GameObject sceneMenu;


    public GameObject MenuAnimazioni;

    public GameObject MenuControllers;

    public GameObject panel1;
    public GameObject panel1_1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;
    public GameObject panel7;
    public GameObject panel8;
    public GameObject panel9;

    public GameObject panel10;
    public GameObject panel11;

    public GameObject panel7_1;
    public GameObject panel4_1;
    public GameObject panel11_1;


    public StartingScene StartingScene;

    // Start is called before the first frame update
    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        panel5.SetActive(false);
        panel6.SetActive(false);
        panel7.SetActive(false);
        panel8.SetActive(false);
        panel9.SetActive(false);
        panel10.SetActive(false);
        panel11.SetActive(false);
        panel5.SetActive(false);
        panel4_1.SetActive(false);
        panel11_1.SetActive(false);

        panel1_1.SetActive(false);

        StartingScene = FindObjectOfType<StartingScene>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeactivateObjects()
    {
        foreach (Transform child in objectManagerVR.transform)
        {

            child.gameObject.SetActive(false);
            

        }
    }

    public void ActivateWoman()
    {
        foreach (Transform child in emptyPlane.transform)
        {
            if (child.name == "woman")
            {
                child.gameObject.SetActive(true);

                child.transform.position = new Vector3(-0.40799999237060549f, 0.7712223529815674f, 1.8f);
            }

        }
    }

    

    public void ActivateMan()
    {
        foreach (Transform child in emptyPlane.transform)
        {
            if (child.name == "man")
            {
                child.gameObject.SetActive(true);
                child.transform.position = new Vector3(0.8730000257492065f, 0.7712223529815674f, 2.1319999575614929f);
            }
        }

    }


    public void ActivatePiano()
    {
        foreach (Transform child in tabletop.transform)
        {
            if (child.name == "piano")
            {
                child.gameObject.SetActive(true);
                child.transform.position = new Vector3(0.054099999368190768f, 0.8321999907493591f, 1.8f);
            }

        }

    }

    public void SpostaMenu()
    {
        sceneMenu.transform.SetPositionAndRotation(new Vector3(-1.9250001907348633f, 2.488999843597412f, 2.8980000019073488f), Quaternion.Euler(0, -33, 0));
    }


    public void PosizionaMenuAzioni()
    {
        MenuAnimazioni.transform.SetPositionAndRotation(new Vector3(-0.041999999433755878f, 2.7039999961853029f, 4.079999923706055f), Quaternion.Euler(0, 0, 0));
    }


    public void SpostaMenuAzioni()
    {
        //MenuAnimazioni.transform.SetPositionAndRotation(new Vector3(-0.8100000023841858f, 3.308000087738037f, 4.079999923706055f), Quaternion.Euler(0, 0, 0));
        //MenuAnimazioni.transform.localScale = new Vector3(7.22f, 7.22f, 7.22f);
        //MenuAnimazioni.GetComponent<BoundsControl>().enabled = true;
    }


    public void SpostaMenuControllers()
    {
        MenuControllers.transform.SetPositionAndRotation(new Vector3(-2.2920000553131105f, 2.572000026702881f, 1.69700003f), Quaternion.Euler(0, -90, 0));
    }
   



    public void Unload()
    {
        StartingScene.UnloadTutorial();
    }



}