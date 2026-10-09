using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SecondCameraManager : MonoBehaviour
{


    public Camera cam;
    private float t;

   

    public ShowFocalLengthValue focalLenghtValue;
    private CameraManager cameraManager;
    public SimulationManager simulationManager;

    private GameObject MixedRealityPlayspace;

    private GameObject CursorVisual;

    public GameObject display;
    public GameObject padreDisplay;

    public GameObject posizioneDisplay;

    // Start is called before the first frame update
    void Start()
    {
        simulationManager = FindObjectOfType<SimulationManager>();
        cam = gameObject.GetComponent<Camera>();
        cam.usePhysicalProperties = true;
        focalLenghtValue = gameObject.GetComponent<ShowFocalLengthValue>();
        cameraManager = FindObjectOfType<CameraManager>();

        

        var tex = new RenderTexture(1920, 1080, 16);
        cam.targetTexture = tex;

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.CompareTag("imagePlane"))
            {
                child.gameObject.GetComponent<MeshRenderer>().material.mainTexture = cam.targetTexture;
            }
        }

        // la camera si disattiva dopo aver preso i valori sopra -- diverso dal codice di Sara
        if (simulationManager.startSimulationPremuto == false)
        {
            gameObject.SetActive(false);
        }


        MixedRealityPlayspace = GameObject.Find("MixedRealityPlayspace");
        
        Transform[] children = MixedRealityPlayspace.GetComponentsInChildren<Transform>();

        
        foreach (Transform child in children)
        {
            // Verifica se il layer del figlio è il layer predefinito (Default)
            if (child.gameObject.layer == 0) // Layer 0 è il layer predefinito
            {
               
                child.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }

        CursorVisual = GameObject.Find("CursorVisual");

        Transform[] children2 = CursorVisual.GetComponentsInChildren<Transform>();


        foreach (Transform child in children2)
        {
            // Verifica se il layer del figlio è il layer predefinito (Default)
            if (child.gameObject.layer == 0) // Layer 0 è il layer predefinito
            {

                child.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
        


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == display)
        {

           
            display.transform.parent = gameObject.transform;

            /*display.transform.position = posizioneDisplay.transform.position;
            display.transform.rotation = posizioneDisplay.transform.rotation;
            display.transform.localScale = posizioneDisplay.transform.localScale;*/
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == display)
        {
            

            padreDisplay.transform.position = gameObject.transform.position;
            padreDisplay.transform.rotation = gameObject.transform.rotation;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == display)
        {
            display.transform.parent = padreDisplay.transform;


        }
    }

    


    public void Activate()
    {

        var tex = new RenderTexture(1920, 1080, 16);
        cam.targetTexture = tex;

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.CompareTag("imagePlane"))
            {
                child.gameObject.GetComponent<MeshRenderer>().material.mainTexture = cam.targetTexture;
            }
        }
    }

    public void ChangeFocalLength(SliderEventData eventData)
    {

        if (gameObject.activeSelf)
        {
            // trasformare i valori tra 0 e 1 in valori delle focali
            if (eventData.NewValue == 0)
            {
                t = 14;
            }

            else if (eventData.NewValue == 0.125)
            {
                t = 24;
            }

            else if (eventData.NewValue == 0.25)
            {
                t = 35;
            }
            else if (eventData.NewValue == 0.375)
            {
                t = 50;
            }
            else if (eventData.NewValue == 0.5)
            {
                t = 85;
            }
            else if (eventData.NewValue == 0.625)
            {
                t = 105;
            }
            else if (eventData.NewValue == 0.75)
            {
                t = 135;
            }
            else if (eventData.NewValue == 0.875)
            {
                t = 200;
            }
            else if (eventData.NewValue == 1)
            {
                t = 400;
            }

            // assegna valore al campo Focal Length
            cam.focalLength = t;

        }
    }

    public void CamCapture()
    {
        cameraManager.CamCapture(cam, focalLenghtValue.getValue().ToString());
    }
}
