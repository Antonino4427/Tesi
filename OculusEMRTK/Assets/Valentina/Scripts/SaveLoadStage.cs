using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;

public class SaveLoadStage : MonoBehaviour
{
    //public GameObject objectManager;
    private TMPro.TMP_Dropdown dropdown;
    public List<string> scenes;
    public string sceneToLoad;
    public List<string> backup;
    public bool isLoading;

    public GameObject loading_canvas;


    //public GameObject ToolkitMRTK;

    void Start()
    {
        //DontDestroyOnLoad(gameObject); // tolto perchè carico le scene in modo additivo per risolvere problema dictaction che non funziona sulla seconda scena caricata
       
        //DontDestroyOnLoad(ToolkitMRTK);

        //inizializza le scene 
        sceneToLoad = "none";
        isLoading = false;
        scenes = new List<string>();
        scenes.Clear();
        string basePath = Path.Combine(Application.streamingAssetsPath, "saved_stages.csv");
        using (var reader = new StreamReader(basePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                scenes.Add(line.Split('|')[0]);

            }
        }


        // crea il dropdown
        dropdown = GameObject.Find("ScenesDropdown").GetComponent<TMP_Dropdown>();
        dropdown.options.Clear();
        foreach (string s in scenes)
        {
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = s });
        }
        sceneToLoad = dropdown.options[dropdown.value].text;
        dropdown.onValueChanged.AddListener(delegate { DropDownObjectSelected(dropdown); });
        dropdown.RefreshShownValue();

        

    }
    void DropDownObjectSelected(TMP_Dropdown dropdown)
    {
        sceneToLoad = dropdown.options[dropdown.value].text;
    }

    void Update()
    {
        
    }

    public void setLoading(bool l)
    {
        isLoading = l;
    }

    public void SaveData(string scene)
    {
        string basePath = Path.Combine(Application.streamingAssetsPath, "saved_stages.csv");
        backup = new List<string>();
        backup.Clear();
        using (var reader = new StreamReader(basePath))
        {
            while (!reader.EndOfStream)
            {
                backup.Add(reader.ReadLine());
            }
        }

        Debug.Log("Scene saved!");
        GameObject plane = GameObject.FindGameObjectWithTag("PlaneTable");
        GameObject empty = GameObject.FindGameObjectWithTag("emptyPlane"); 




        using (var writer = new StreamWriter(basePath))
        {
            string line = scene + "|";
            foreach (Transform child in empty.transform)
            {
                line += child.name + ";" + child.transform.localPosition.x + ";" + child.transform.localPosition.y + ";" + child.transform.localPosition.z + ";" + child.transform.localRotation.eulerAngles.y + ";"
                    + child.transform.localScale.x + ";" + plane.transform.localScale.x + ";" + plane.transform.localScale.y + ";" + child.GetComponent<CharacterManager>().type + "|";

                Debug.Log("ROTAZIONE " + child.transform.localRotation.eulerAngles.y + "POSIZIONE " + child.transform.localPosition.x + child.transform.localPosition.y + child.transform.localPosition.z);
            }
            writer.WriteLine(line);
            foreach (string s in backup)
            {
                writer.WriteLine(s);
            }
        }
    }



    public void LoadData()
    {
        Debug.Log("Load from menu " + sceneToLoad);
        string basePath = Path.Combine(Application.streamingAssetsPath, "saved_stages.csv");
        using (var reader = new StreamReader(basePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Split('|')[0] == sceneToLoad)
                {
                    Debug.Log("Asking objectManager...");

                    StartCoroutine(LoadingCanvas(0.8f));
                    StartCoroutine(ExecuteAfterTime(3));
                    
                }

            }
        }

        
    }

    IEnumerator LoadingCanvas(float time)
    {
        yield return new WaitForSeconds(time);

        loading_canvas.SetActive(true);
       
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        loading_canvas.SetActive(false);
        GameObject.Find("ObjectsManager VR")?.GetComponent<ObjectManagerVR>().LoadScene(sceneToLoad);
       
    }
}
