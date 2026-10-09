using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Rendering;

public class CameraManager : MonoBehaviour
{
    //[SerializeField] private Camera mainCamera;
    private Camera mainCamera;
    [SerializeField] private Camera secondCamera;
    public Hashtable focalTable = new Hashtable();
    public Hashtable actionTimes = new Hashtable();
    public List<byte[]> images = new List<byte[]>();
    private SimulationManager simulationManager;
    private OutputGenerator outputGenerator;
    private AnimaPersonaggio animaPersonaggio;
    private Camera newCamera;
    private int code;
    private int actionTime;

    private GameObject NewPadreDisplay;


    private GameObject canvasNewCamera;
   // public Image screenshotImage; 

    public GameObject plane;
    public GameObject pendingObject;

    public TMPro.TextMeshPro TestoNumScreen ;
    public GameObject photoCaptureManager;

    private  GameObject TestoNumScreenOBJ;
    public List<Camera> cameraList = new List<Camera>();
    // Start is called before the first frame update
    void Start()
    {
        simulationManager = FindObjectOfType<SimulationManager>();
        outputGenerator = FindObjectOfType<OutputGenerator>();
        animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();
        
      mainCamera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
      //mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();


        canvasNewCamera = GameObject.Find("ParentCanvas").transform.Find("Canvas_NewCameraAdded").gameObject;

        canvasNewCamera.SetActive(false);

        


        cameraList.Add(secondCamera);

        for (int i = 0; i < secondCamera.transform.childCount; i++)
        {
            if (secondCamera.transform.GetChild(i).CompareTag("cameraButton"))
            {
                var interactable = secondCamera.transform.GetChild(i).GetComponent<Interactable>();

                var onFocusReceiver = interactable.AddReceiver<InteractableOnFocusReceiver>();

                onFocusReceiver.OnFocusOn.AddListener(() => secondCamera.GetComponent<ObjectManipulator>().enabled = false);
                onFocusReceiver.OnFocusOff.AddListener(() => secondCamera.GetComponent<ObjectManipulator>().enabled = true);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NewCamera() {
        newCamera = Instantiate(secondCamera);

        foreach (Transform child in newCamera.transform)
        {
            if (child.name == "Display")
            {
                child.gameObject.SetActive(false);
            }

            if (child.name == "DisplayNewCamera")
            {
                child.gameObject.SetActive(true);
            }
        }


       
        NewPadreDisplay = Instantiate(secondCamera.GetComponent<SecondCameraManager>().padreDisplay);


        newCamera.GetComponent<SecondCameraManager>().padreDisplay = NewPadreDisplay;
        newCamera.GetComponent<SecondCameraManager>().display = newCamera.transform.Find("DisplayNewCamera").gameObject;

        foreach (Transform child in NewPadreDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        
        newCamera.transform.position = mainCamera.transform.position;
        newCamera.transform.rotation = Quaternion.Euler( mainCamera.transform.rotation.eulerAngles);
        newCamera.GetComponent<ShowFocalLengthValue>().setValue(0.1f);

        

        cameraList.Add(newCamera);

        for (int i = 0; i < newCamera.transform.childCount; i++)
        {
            if (newCamera.transform.GetChild(i).CompareTag("cameraButton"))
            {
                var interactable = newCamera.transform.GetChild(i).GetComponent<Interactable>();

                var onFocusReceiver = interactable.AddReceiver<InteractableOnFocusReceiver>();

                onFocusReceiver.OnFocusOn.AddListener(() => newCamera.GetComponent<ObjectManipulator>().enabled = false);
                onFocusReceiver.OnFocusOff.AddListener(() => newCamera.GetComponent<ObjectManipulator>().enabled = true);

            }
        }


        //canvas new camera
        canvasNewCamera.SetActive(true);
        StartCoroutine(DeactivatecanvasNewCamera());
    }

    private IEnumerator DeactivatecanvasNewCamera()
    {
        yield return new WaitForSeconds(3);

        canvasNewCamera.SetActive(false);
    }


    /*public void AddNewValue(int time, string focal)
    {
        focalTable.Add(time.ToString(), focal);
    }*/

    public void AddNewValue(string index, string focal)
    {
        if (focalTable.ContainsKey(index))
        {
            focalTable[index] = focal;
        }
        else
        {
            focalTable.Add(index, focal);
        }
            
    }

    public void AddImages(byte[] image)
    {
        images.Add(image);
    }

    /*public void AddActionTime(string index, string time)
    {
        actionTimes.Add(index, time);
    }*/

    public void AddActionTime(string index, int time)
    {

        if (actionTimes.ContainsKey(index))
        {
            actionTimes[index] = time;
        }
        else
        {
            actionTimes.Add(index, time);
        }
        
    }


    public void CamCapture(Camera cam, string focalLenght)
    {
        if (SimulationManager.status == 1)
        {
            photoCaptureManager = GameObject.Find("PhotoCaptureManager");

            //simulationManager.IncrementScreenshotCount(); // chiamato da Unity quando si preme il tasto per fare lo screen
            animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();
            animaPersonaggio.StopAll();
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            cam.Render();

            Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = currentRT;

            var Bytes = image.EncodeToPNG();
            AddImages(Bytes);

            code = outputGenerator.sceneCode;

            //screenshotImage.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

            pendingObject = Instantiate(plane); // creao un nuovo piano

            

            pendingObject.transform.localScale = plane.transform.lossyScale;

            pendingObject.name = code + "_img" + simulationManager.GetScreenshotCount().ToString();

            foreach (Transform figlio in photoCaptureManager.transform)
            {
                if (figlio.name == pendingObject.name)
                {
                    Destroy(figlio.gameObject);
                }
            }

            pendingObject.transform.parent = photoCaptureManager.transform; // definisco padre

            pendingObject.transform.rotation = plane.transform.rotation;


            pendingObject.GetComponent<Renderer>().material.SetTexture("_MainTex", image);

            TestoNumScreen = pendingObject.transform.Find("TitleScreen").GetComponent<TMPro.TextMeshPro>();

            //TestoNumScreenOBJ = pendingObject.transform.Find("TitleScreen").gameObject;

            int NScreen = simulationManager.GetScreenshotCount() + 1;

            TestoNumScreen.text = "# " + NScreen.ToString();

            




            StartCoroutine(ExecuteAfterTimeUpdate());




            /*foreach (Transform c in GameObject.Find("PhotoCaptureManager").transform.GetComponentsInChildren<Transform>())
            {

                c.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            GameObject.Find("PhotoCaptureManager").transform.rotation = Quaternion.Euler(0, 0, 0);*/


            // Destroy(image);










            /*string basePath = Path.Combine(Application.streamingAssetsPath, "storyboards/screenshots/" + code + "_img" + simulationManager.GetScreenshotCount().ToString() + ".png");
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, code + "_img" + simulationManager.GetScreenshotCount().ToString() + ".png");
            File.WriteAllBytes(filePath, Bytes);
            File.WriteAllBytes(basePath, Bytes);*/

            File.WriteAllBytes(Application.dataPath + "/Valentina/Scripts/storyboards/screenshots/" + code + "_img" + simulationManager.GetScreenshotCount().ToString() + ".png", Bytes);

            actionTime = simulationManager.GetTime();

            AddActionTime(simulationManager.GetScreenshotCount().ToString(), actionTime);
            //actionTimes.Add(simulationManager.GetScreenshotCount().ToString(), actionTime);

            //chiama il metodo per generare una frase di camminata e una di vicinanza
            simulationManager.GenerateCondition();

            AddNewValue(simulationManager.GetScreenshotCount().ToString(), focalLenght); //memorizza la focale al tempo time 


            StartCoroutine(ExecuteAfterTime(1));

            //TestoNumScreenOBJ.GetComponent<MaterialInstance>().CacheSharedMaterialsFromRenderer = true;

        }
    }

    

    IEnumerator ExecuteAfterTime(float time)
{
    yield return new WaitForSeconds(time);

        //animaPersonaggio.PlayAll();
        simulationManager.CompletedAction();
    }

    IEnumerator ExecuteAfterTimeUpdate()
    {
        yield return new WaitForSeconds(0.2f);

        photoCaptureManager.GetComponent<GridObjectCollection>().UpdateCollection();

        GameObject.Find("ScrollingObjectCollection_Screen").GetComponent<ScrollingObjectCollection>().UpdateContent();
    }



}
