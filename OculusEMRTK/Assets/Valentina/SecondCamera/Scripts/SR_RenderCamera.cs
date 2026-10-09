using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;

public class SR_RenderCamera : MonoBehaviour
{
    /*public int FileCounter = 0; // oppure 1???
    [SerializeField] Camera Cam;
    //public TextMeshPro focalLenght;
    private ShowFocalLengthValue focalLenghtValue;
    public GameObject plane;
    public GameObject pendingObject;
    public Texture2D image;
    public RenderTexture currentRT;
    public int code;
    //public int time;
    //public GameObject SimulationManager;
    
    public Hashtable focalTable = new Hashtable();
    public Hashtable actionTimes = new Hashtable();
    public List<byte[]> images = new List<byte[]>();


    private SimulationManager simulationManager;
    private OutputGenerator outputGenerator;
    private AnimaPersonaggio animaPersonaggio;
    private string sceneName;
    private int actionTime;

    private void Start()
    {
        focalLenghtValue = FindObjectOfType<ShowFocalLengthValue>();
        simulationManager = FindObjectOfType<SimulationManager>();
        outputGenerator = FindObjectOfType<OutputGenerator>();
        animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CamCapture();
        }
    }
    public void CamCapture()
    {

        //currentRT = RenderTexture.active;
        //RenderTexture.active = Cam.targetTexture;

        // Cam.Render();
        //simulationManager.IncrementScreenshotCount(); // LO CHIAMO DIRETTAMENTE NEL ONCLICK DEL BOTTONE PER GLI SCREEN ALTRIMENTI LA PRIMA IMMAGINE è VUOTA

        if (SimulationManager.status == 1 && simulationManager.dialogue == false)
        {
            animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();
            animaPersonaggio.StopAll();
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = Cam.targetTexture;

            Cam.Render();

            Texture2D image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = currentRT;


            var Bytes = image.EncodeToPNG();

            images.Add(Bytes);

            Destroy(image);





            //StartCoroutine(ExecuteAfterTime(5));

            pendingObject = Instantiate(plane); // creao un nuovo piano

            pendingObject.transform.parent = gameObject.transform; // definisco padre

            //Renderer renderer = pendingObject.GetComponent<Renderer>();
            //renderer.material.mainTexture = image;
            pendingObject.GetComponent<Renderer>().material.SetTexture("_MainTex", image);
            gameObject.GetComponent<GridObjectCollection>().UpdateCollection(); // è come premere tasto Update Collection




            code = simulationManager.GetComponent<OutputGenerator>().sceneCode;
            //time = SimulationManager.GetComponent<SimulationManager>().GetTime();
            File.WriteAllBytes(Application.dataPath + "/Valentina/Scripts/storyboards/screenshots/" + code + "_img" + simulationManager.GetScreenshotCount().ToString() + ".png", Bytes);

            

    FileCounter++;

            
            actionTime = simulationManager.GetTime();
            actionTimes.Add(simulationManager.GetScreenshotCount().ToString(), actionTime);

            //chiama il metodo per generare una frase di camminata e una di vicinanza
            simulationManager.GenerateCondition();

            focalTable.Add(simulationManager.GetScreenshotCount().ToString(), focalLenghtValue.getValue().ToString()); //memorizza la focale al tempo time 

            //simulationManager.IncrementScreenshotCount();
            StartCoroutine(ExecuteAfterTime(1));


        }
        
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        animaPersonaggio.PlayAll();
        simulationManager.CompletedAction();

    }

        public void AddNewValue(int time, string focal)
        {


            focalTable.Add(time.ToString(), focal);
        }

    public void AddImages(byte[] image)
    {
        images.Add(image);
    }

    public void AddActionTime(string index, string time)
    {
        actionTimes.Add(index, time);
    } */

}
