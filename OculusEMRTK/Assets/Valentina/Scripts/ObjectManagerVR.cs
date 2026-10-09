using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Examples.Demos;

public class ObjectManagerVR : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    public GameObject ob;
    public GameObject title;
    public GameObject positionTitle;
    public AudioClip _notificationSound;
    private GameObject _soundManager;

    public AudioClip MRTK_Move_Start;
    public AudioClip MRTK_Move_End;

    public GameObject HorizontallScroll_Objects;
    private float planeWidth = 0;
    private float planeHeight = 0;

    private NavMeshSurface navMeshSurface;
    public GameObject empty;
    private GameObject plane;
    public GameObject sceneName;
    private bool pianoTrovato = false;
    public bool editMode;

    [SerializeField] private GameObject secondCamera;

    public GameObject current;
    private TextMeshPro text;
    public InteractableToggleCollection radio;
    
    //public MRTKTMPInputField newText;
    public TMP_InputField _newText;
    public bool changeNameScene;
    public bool changeName;
    public GameObject SaveSceneButton;
    public GridObjectCollection SceneMenuButtonGridObjectCollection;

    //public TMP_InputField _sceneNameInputField;

    public List<GameObject> objectsInScene = new List<GameObject>();
    private AnimaPersonaggio animaPersonaggio;

    private List<GameObject> sceneObjects = new List<GameObject>();

   


    void Start()
    {
        // nome oggetto selezionato
        text = title.GetComponent<TextMeshPro>();
        _soundManager = GameObject.Find("SoundManager");
        changeNameScene = false;
        changeName = false;
        //sceneName.GetComponent<TextMeshPro>().text = "funziona";
        SaveSceneButton.SetActive(false);
        SceneMenuButtonGridObjectCollection.UpdateCollection();

        

    }


    void Update()
    {
        // nome oggetto selezionato
        if (current != null)
        {
            positionTitle.SetActive(true);
            text.text = current.name; // definisce nome oggetto

            positionTitle.transform.position = new Vector3(current.transform.position.x + 0.05f, current.transform.position.y + (current.transform.position.y) / 2.5f, current.transform.position.z); // posiziona nome oggetto in base al padre empty
        }
        else
        {
            positionTitle.SetActive(false);
            text.text = "";
        }

        DeleteObject();
        //ActivateDeactivateEditMode();

        if (!pianoTrovato)
        {
            plane = GameObject.FindGameObjectWithTag("PlaneTable");
            if (plane != null)
            {
                //plane.GetComponent<MeshRenderer>().material = tableMaterial;
                planeWidth = plane.transform.localScale.x;
                planeHeight = plane.transform.localScale.y;
                pianoTrovato = true;
                //empty = new GameObject("emptyPlane");
                empty.transform.position = plane.transform.position;
                empty.transform.position = new Vector3(empty.transform.position.x, empty.transform.position.y, empty.transform.position.z - 0.5f);
                //empty.tag = "emptyPlane";
                empty.transform.localRotation = Quaternion.Euler(empty.transform.localRotation.x, - plane.transform.localRotation.z, empty.transform.localRotation.z);
            }
        }

        
        


    }

    public void CreateObject(string n)
    {

        foreach (GameObject o in objects)
        {
            if (n == o.name)
                ob = o;
        }

        // aggiunta rotazione 180 su Y perche il personaggio era girato
        pendingObject = Instantiate(ob, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y + 180, gameObject.transform.localRotation.z)); 

        objectsInScene.Add(pendingObject);

       

        for (int i = 0; i < pendingObject.transform.childCount; i++)
        {
            pendingObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
        }

        //DeactivateEditMode();
        //radio.SetSelection(0);



        // diverso da AR

        //var actualScale = pendingObject.GetComponent<Collider>().bounds.size.x / planeWidth;
        //var wantedScale = 0.13f;
        //var dimensioniColliderCheVorrei = wantedScale * planeWidth;

        // var nuovaScala = (pendingObject.transform.localScale.x * dimensioniColliderCheVorrei) / pendingObject.GetComponent<Collider>().bounds.size.x;

        var wantedScale = 0;

        if (ob.name == "wall")
        {
            wantedScale = 18;
           
        }
        else
        {
            wantedScale = 8;
           
        }

        var nuovaScala = pendingObject.transform.localScale.x * wantedScale;
        pendingObject.transform.localScale = new Vector3(nuovaScala, nuovaScala, nuovaScala);


        //Setta come figlio di ObjectManagerVR
        pendingObject.transform.parent = gameObject.transform;

        pendingObject.name = ob.name; //Rimuove "(Clone)" dal nome delle nuove istanze

        pendingObject.layer = LayerMask.NameToLayer("Default");

        for (int i = 0; i < pendingObject.transform.childCount; i++)
        {
            pendingObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
        }

        pendingObject.AddComponent<Rigidbody>();
        pendingObject.AddComponent<ConstraintManager>();
        pendingObject.AddComponent<ObjectManipulator>();
        pendingObject.AddComponent<NearInteractionGrabbable>();
        pendingObject.AddComponent<TetheredPlacement>();
        pendingObject.AddComponent<CursorContextObjectManipulator>();
        pendingObject.AddComponent<CharacterManager>();
        pendingObject.AddComponent<State>();

        pendingObject.AddComponent<NearInteractionTouchable>();


        // aggiunta per togliere la gravità agli oggetti
        pendingObject.GetComponent<Rigidbody>().useGravity = false;
        pendingObject.GetComponent<Rigidbody>().isKinematic = true;
        // disattivo NavMeshAgent per poter muovere liberamente il personaggio  --> la aggiungo direttamente in StopManipulation, in fase di storyboard
        /*if (pendingObject.CompareTag("Player"))
        {
            pendingObject.AddComponent<NavMeshAgent>();
            pendingObject.GetComponent<NavMeshAgent>().speed = 0.5f;
            pendingObject.GetComponent<NavMeshAgent>().height = 1.55f;
            pendingObject.GetComponent<NavMeshAgent>().enabled = false;

        }*/




        //suono
        //pendingObject.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(ManStart);

        //pendingObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(ManEnd);



    }

    // suono
    private void ManStart(ManipulationEventData arg)
    {
        if (current != null)
        {
            current.GetComponent<AudioSource>().PlayOneShot(MRTK_Move_Start);
        }
        
    }

    //suono
    private void ManEnd(ManipulationEventData arg)
    {
        if (current != null)
        {
            current.GetComponent<AudioSource>().PlayOneShot(MRTK_Move_End);
        }
            
    }

    public void SelectObject(GameObject g)
    {
       
        current = g;
        
    }

    public void RemoveObject(GameObject g)
    {
        if (current == g)
        {
            current = null;
        }
    }

    public GameObject getCurrent()
    {
        return current;
    }


    // premendo X, dopo aver selezionato un oggetto lo posso eliminare
    public void DeleteObject()
    {

        if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.RawButton.X))
            if (current != null && SimulationManager.status == 0)
            {
                {
                    _soundManager.GetComponent<AudioSource>().PlayOneShot(_notificationSound);
                    objectsInScene.Remove(current);
                    Destroy(current);
                }
            }
        
    }

    // DISATTIVATO
    // premendo Y freezzo o no i rigidbody degli oggetti in scena
    /*public void ActivateDeactivateEditMode()
    {

        foreach (GameObject ob in objectsInScene)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {

                if (ob.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
                {


                    editMode = false;
                    ob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    ob.GetComponent<Rigidbody>().useGravity = true;

                    ob.GetComponent<ObjectManipulator>().enabled = true;



                    HorizontallScroll_Objects.SetActive(true);


                }
                else
                {
                    editMode = true;
                    ob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    ob.GetComponent<ObjectManipulator>().enabled = false;

                    HorizontallScroll_Objects.SetActive(false);

                    
                }
            }

        }

        if (objectsInScene.Count == 0)
        {
            HorizontallScroll_Objects.SetActive(true);
        }
    }*/

    public void StopManipulation()
    {
        animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();

        foreach (GameObject ob in objectsInScene)
        {
            //ob.GetComponent<TapToPlace>().enabled = false;
            ob.GetComponent<ObjectManipulator>().enabled = false;
            ob.GetComponent<NearInteractionGrabbable>().enabled = false;
            //ob.GetComponent<BoundsControl>().enabled = false;
            ob.GetComponent<CharacterManager>().simulation = true;


            if (ob.CompareTag("Player"))
            {
                if (ob.GetComponent<NavMeshAgent>() == null)
                {
                    ob.AddComponent<NavMeshAgent>();
                    ob.GetComponent<NavMeshAgent>().speed = 0.5f;
                    ob.GetComponent<NavMeshAgent>().height = 1.55f;
                    ob.GetComponent<NavMeshAgent>().radius = 0.18f;
                }
                else
                {
                    ob.GetComponent<NavMeshAgent>().enabled = true;
                }

                if (ob.GetComponent<Animator>() != null && ob.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sit"))
                {
                    ob.GetComponent<NavMeshAgent>().enabled = false;
                }

                ob.transform.SetParent(empty.transform, worldPositionStays: true);
            }
            else
            {
                ob.transform.SetParent(plane.transform.parent.transform, worldPositionStays: true);
            }

        }
        plane.AddComponent<PointerHandler>();
        PointerHandler ph = plane.GetComponent<PointerHandler>();

        ph.OnPointerClicked.AddListener((e) => {
            animaPersonaggio.FindDestination(e);
        });


        //navMeshSurface = plane.transform.parent.gameObject.GetComponent<NavMeshSurface>();
       // navMeshSurface = plane.GetComponent<NavMeshSurface>();
        secondCamera.GetComponent<SecondCameraManager>().Activate();

        //navMeshSurface.BuildNavMesh();

    }

    public void RestartManipulation()
    {
        foreach (GameObject ob in objectsInScene)
        {
            //ob.GetComponent<TapToPlace>().enabled = true;
            ob.GetComponent<ObjectManipulator>().enabled = true;
            ob.GetComponent<NearInteractionGrabbable>().enabled = true;
            //ob.GetComponent<BoundsControl>().enabled = true;
            ob.GetComponent<CharacterManager>().simulation = false;

            // DISATTIVANDO NavMeshAgent POSSO MUOVERE IL PERSONAGGIO SENZA CHE SIA ATTACCATO ALLA NAVMESH
            if (ob.CompareTag("Player"))
            {
                ob.GetComponent<NavMeshAgent>().enabled = false;

            }

        }

        //navMeshSurface = plane.transform.parent.gameObject.GetComponent<NavMeshSurface>();
        //navMeshSurface.enabled = false;
        secondCamera.SetActive(false);

    }


    public void SetChangeName(bool l)
    {
        changeName = l;
    }

    
    public void Rename()
    {
        if (current != null && changeName == true)
        {
            current.name = _newText.text;
        }
        //_newText.text = "";
        changeName = false;
    }

    public void SetChangeNameScene(bool l)
    {
        changeNameScene = l;
    }

    public void RenameScene()
    {
        if (sceneName.GetComponent<TextMeshPro>().text != null && changeNameScene == true)
        {
            sceneName.GetComponent<TextMeshPro>().SetText(_newText.text);

            if (SaveSceneButton.activeSelf == false)
            {
                SaveSceneButton.SetActive(true);
                SceneMenuButtonGridObjectCollection.UpdateCollection();
            }
            
        }
        //_newText.text = "";
        changeNameScene = false;


    }


    public void SaveScene()
    {


        foreach (GameObject hologram in objectsInScene)
        {

            //WAM.AttachAnchor(hologram);


            hologram.transform.SetParent(empty.transform, worldPositionStays: true);


            // Assicurati che gli oggetti seguano il movimento e le trasformazioni del punto di riferimento
            foreach (Transform child in empty.transform)
            {
                child.SetParent(null); // Rimuovi temporaneamente il padre per evitare trasformazioni indesiderate
                child.SetParent(empty.transform, worldPositionStays: true);
            }

        }


        // Salva relativePositions in un file o in una variabile per poterlo caricare successivamente
        string SN = sceneName.GetComponent<TextMeshPro>().text;
        GameObject.Find("SaveManager")?.GetComponent<SaveLoadStage>().SaveData(SN);

    }

    public void LoadScene(string scene)
    {
        

        Debug.Log("Building Manager: loading scene " + scene);
        //sceneName.GetComponent<TMP_InputField>().text = scene;
        
        string basePath = Path.Combine(Application.streamingAssetsPath, "saved_stages.csv");
        sceneName.GetComponent<TextMeshPro>().text = scene;

        using (var reader = new StreamReader(basePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                Debug.Log("sta leggendo le linee");
                if (line.Split('|')[0] == scene)
                {
                    //istanzia ogni oggetto della linea

                    var objs = line.Split('|');
                    var i = 0;
                    Debug.Log("ci sono degli oggetti");

                    foreach (string toInstantiate in objs)
                    {
                        //Debug.Log(toInstantiate);
                        if (toInstantiate != "" && i != 0)
                        {
                        
                            var n = toInstantiate.Split(';')[0];
                            var x = float.Parse(toInstantiate.Split(';')[1]);
                            var y = float.Parse(toInstantiate.Split(';')[2]);
                            var z = float.Parse(toInstantiate.Split(';')[3]);
                            var r = float.Parse(toInstantiate.Split(';')[4]);
                            var s = float.Parse(toInstantiate.Split(';')[5]);
                            var width = float.Parse(toInstantiate.Split(';')[6]);
                            var height = float.Parse(toInstantiate.Split(';')[7]);
                            var type = toInstantiate.Split(';')[8];

                            foreach (GameObject o in objects)
                            {
                                if (type == o.name)
                                    ob = o;
                            }

                            var ogg = Instantiate(ob, empty.transform);
                            ogg.name = n;
                            if (ogg.activeSelf == false)
                            {
                                ogg.SetActive(true);
                            }
                            foreach (Transform child in ogg.transform)
                            {
                                child.gameObject.SetActive(true);
                            }


                            // tolto scale factor, diverso da AR
                            //float scaleFactor = 0;


                            objectsInScene.Add(ogg);
                            //scaleFactor = MathF.Min(planeWidth / width, planeHeight / height);

                            ogg.transform.SetParent(empty.transform, worldPositionStays: true);
                            ogg.transform.localScale = new Vector3(s , s , s );
                            ogg.transform.localPosition = (new Vector3(x, y, z));
                            ogg.name = n;
                            ogg.transform.localRotation = Quaternion.Euler(0, r, 0);

                            //Debug.Log("scalefactor is " + scaleFactor);


                            

                            ogg.AddComponent<NearInteractionGrabbable>();
                            ogg.AddComponent<ObjectManipulator>();
                            

                            ogg.AddComponent<CharacterManager>();
                            ogg.GetComponent<CharacterManager>().loadedObject = true;
                            ogg.GetComponent<CharacterManager>().type = type;
                            ogg.AddComponent<State>();

                            ogg.layer = LayerMask.NameToLayer("Default");

                            if (ogg.transform.childCount != 0)
                            {
                                for (int j = 0; j < ogg.transform.childCount; j++)
                                {
                                    ogg.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Default");
                                }

                            }

                            ogg.AddComponent<Rigidbody>();
                            ogg.AddComponent<ConstraintManager>();

                            ogg.AddComponent<NearInteractionTouchable>();
                            

                            ogg.AddComponent<TetheredPlacement>();
                            ogg.AddComponent<CursorContextObjectManipulator>();

                            // aggiunta per togliere la gravità agli oggetti
                            ogg.GetComponent<Rigidbody>().useGravity = false;
                            ogg.GetComponent<Rigidbody>().isKinematic = true;
                        }
                        i++;
                    }
                }

            }
        }

        
    }

    public List<GameObject> getObjectsInScene()
    {
        sceneObjects.AddRange(objectsInScene);
        return sceneObjects;
    }
}

