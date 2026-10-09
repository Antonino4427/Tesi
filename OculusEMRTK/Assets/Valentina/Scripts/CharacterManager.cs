using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    public ObjectManagerVR objectManagerVR;
    public SimulationManager simulationManager;
    public AnimaPersonaggio animaPersonaggio;

    public SaveLoadStage saveLoadManager;
    //private TapToPlace ttp;
    //private BoundsControl bc;
    public string type="";
    public bool simulation = false;
    public bool isWalking;
    public List<string> nearObjects;
    public PhraseGenerator phraseGenerator;
    public bool loadedObject = false;

    private bool primaryHandTriggerPress = false;

    public string lastAction = "";
    public int lastTimeAction = 0;

    public Transform groundCheck;
    public float groundDistance = 0.4f;

    private bool checkDestination = false;

    public string place;
    private bool newPlace;
    public Collider[] hitColliders;

    private GameObject CursorVisual;

    // Start is called before the first frame update
    void Start()
    {
        //ttp = gameObject.GetComponent<TapToPlace>();
        //bc = gameObject.GetComponent<BoundsControl>();
        objectManagerVR = FindObjectOfType<ObjectManagerVR>();
        gameObject.AddComponent<Interactable>();
        phraseGenerator = FindObjectOfType<PhraseGenerator>();
        saveLoadManager = FindObjectOfType<SaveLoadStage>();
        animaPersonaggio = FindObjectOfType<AnimaPersonaggio>();




        if (!loadedObject)
        {
            type = gameObject.name;
        }

        if (CompareTag("Player"))
        {
            //interazione toccando l'oggetto o il personaggio, uguale all'OnClick da lontano. Per funzionare si aggiunge il componente NearInteractionTouchable all'oggetto in ObjectManagerVR
            var onpress = GetComponent<Interactable>().AddReceiver<InteractableOnPressReceiver>();

            
                GetComponent<Interactable>().OnClick.AddListener(() => simulationManager.SetActiveCharacter(gameObject));

                
                onpress.OnPress.AddListener(() => simulationManager.SetActiveCharacter(gameObject));
                
            
            
            onpress.OnPress.AddListener(() => objectManagerVR.SelectObject(gameObject));

        }
        else
        {
            GetComponent<Interactable>().OnClick.AddListener(() => simulationManager.setActiveObject(gameObject));

            //interazione toccando l'oggetto o il personaggio, uguale all'OnClick da lontano. Per funzionare si aggiunge il componente NearInteractionTouchable all'oggetto in ObjectManagerVR
            var onpress = GetComponent<Interactable>().AddReceiver<InteractableOnPressReceiver>();

                onpress.OnPress.AddListener(() => simulationManager.setActiveObject(gameObject));
                onpress.OnPress.AddListener(() => objectManagerVR.SelectObject(gameObject));

        }

        nearObjects = new List<string>();
        simulationManager = FindObjectOfType<SimulationManager>();
        place = "ground";

        isWalking = false;
        newPlace = false;

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
        if (!simulation)
        {
            /*if (ttp.enabled)
            {
                if (ttp.IsBeingPlaced)
                {
                    objectManagerVR.SelectObject(gameObject);
                }
                else
                {
                    objectManagerVR.RemoveObject(gameObject);
                }
            }

            if (objectManagerVR.getCurrent() == null)
            {
                bc.BoundsControlActivation = BoundsControlActivationType.ActivateByPointer;
                if (objectManagerVR.editMode == false)
                {
                    ttp.enabled = true;
                }
            }
            else if (objectManagerVR.getCurrent().Equals(gameObject))
            {
                bc.BoundsControlActivation = BoundsControlActivationType.ActivateByPointer;
                if (objectManagerVR.editMode == false)
                {
                    ttp.enabled = true;
                }

            }
            else
            {
                bc.BoundsControlActivation = BoundsControlActivationType.ActivateManually;
                ttp.enabled = false;

            }*/

            Interactable i = gameObject.GetComponent<Interactable>();
            i.OnClick.AddListener(() => objectManagerVR.SelectObject(gameObject));
           
           

            // per vedere apparire il nome dell'oggetto quando viene manipolato da vicino con Object Manipulator 
            if (gameObject.GetComponent<ObjectManipulator>() != null)
            {
                ObjectManipulator o = gameObject.GetComponent<ObjectManipulator>();

                o.OnManipulationStarted.AddListener(TouchStarted);
            }

            

        }
        else 
        {
            if (isWalking && gameObject.CompareTag("Player"))
            {
                Walk(true);
                CheckNearObjects();
                //CheckPlace();
                CheckForward();
            }

            if (checkDestination)
            {
                if (isWalking)
                {
                    FindObjectOfType<AnimaPersonaggio>().WalkMode(true);
                    var navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
                    if (!navMeshAgent.pathPending)
                    {
                        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                        {
                            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                            {
                                gameObject.GetComponent<Animator>().SetBool("walking", false);
                                isWalking = false;
                                checkDestination = false;
                            }
                        }
                    }
                }
                else
                {
                    gameObject.GetComponent<CharacterManager>().isWalking = false;
                    FindObjectOfType<AnimaPersonaggio>().WalkMode(false);
                }
            }

        }



    }

    private void TouchStarted(ManipulationEventData arg)
    {
        objectManagerVR.SelectObject(gameObject);
    }


    public void CheckNearObjects()
    {
        
        if (nearObjects != null)
        {
            nearObjects.Clear();
            //rimuovi outline
           //simulationManager.DestroyParticles();

        }
        float objCollider = GetComponent<Collider>().bounds.size.x;
        hitColliders = Physics.OverlapSphere(transform.position,  objCollider*1.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform != transform && hitCollider.gameObject.layer != LayerMask.NameToLayer("Ground") && hitCollider.gameObject.name != "light" && objectManagerVR.objectsInScene.Contains(hitCollider.gameObject))
            {
                nearObjects.Add(hitCollider.gameObject.name);
                //Aggiungi outline se l'uomo punta 
                if (Vector3.Angle(transform.forward, hitCollider.transform.position - transform.position) < 40f)
                {
                    if (hitCollider.gameObject.name == "door")
                    {
                        var pd = new Vector3(hitCollider.transform.Find("Door External").transform.position.x, hitCollider.transform.position.y, hitCollider.transform.Find("Door External").transform.position.z);
                        //simulationManager.CreateParticle(hitCollider.gameObject);

                    }
                    //else
                        //simulationManager.CreateParticle(hitCollider.gameObject);
                }
            }
        }
       // GetComponent<State>().NewNearObjects(nearObjects);
    }

    public void Walk(bool walking)
    {
        var navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 0.15f;

        phraseGenerator.AggiornaTesto();

        if (!isWalking && walking)
        {
            isWalking = true;
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                // fermo
            }
            else
            {
                //phraseGenerator.GenerateMovementPhrase(gameObject.name, place);
            }
        }
        else if (isWalking && !walking)
            isWalking = false;
    }

    /*public void CheckPlace()
    {
        foreach (Transform child in buildingManager.transform)
        {
            if (child.CompareTag("tile"))
            {
                if (transform.position.x - 1 < child.transform.position.x && transform.position.x + 1 > child.transform.position.x)
                {
                    if (transform.position.z - 1 < child.transform.position.z && transform.position.z + 1 > child.transform.position.z)
                    {
                        if (transform.position.y - 1 < child.transform.position.y && transform.position.y + 1 > child.transform.position.y)
                        {
                            if (place != child.name)
                            {
                                newPlace = true;
                            }
                            place = child.name;

                        }
                    }
                }

            }
            if (child.name == "stair")
            {
                if (transform.position.x - 1 < child.transform.position.x && transform.position.x + 1 > child.transform.position.x)
                {
                    if (transform.position.z - 1 < child.transform.position.z && transform.position.z + 1 > child.transform.position.z)
                    {
                        if (transform.position.y - 1 < child.transform.position.y && transform.position.y + 1 > child.transform.position.y)
                        {
                            if (place != "stairs")
                            {
                                newPlace = true;



                            }
                            if (transform.position.y - oldY < 0f) //sta scendendo
                                place = "stair";
                            else place = "stair";
                        }
                    }
                }
            }
        }
        if (newPlace)
        {
            gameManager.GetComponent<PhraseGenerator>().GenerateMovementPhrase(gameObject.name, place);
            Debug.Log(place + "luogo");
            newPlace = false;
        }

    }*/

    public void CheckForward()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Vector3 startPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);

        //Debug.DrawRay(raycastObject.transform.position, fwd * 50, Color.green);
        if (Physics.Raycast(startPosition + transform.forward * 0.5f, fwd, out hit, 50))
        {
            bool p = false;
            if (hit.transform.gameObject.GetComponent<CharacterManager>() != null)
            { p = hit.transform.gameObject.name == hit.transform.gameObject.GetComponent<CharacterManager>().type; }


            Debug.Log(hit.transform.name);
           
            phraseGenerator.UpdateForward(hit.transform.gameObject.name,p);
        }
        //else gameManager.GetComponent<PhraseGenerator>().UpdateForward("");
    }

    public void CheckDestination()
    {

        checkDestination = true;
    }

    public void StopWalking()
    {
        isWalking = false;
        gameObject.GetComponent<Animator>().SetBool("walking", false);
        checkDestination = false;
        gameObject.GetComponent<CharacterManager>().isWalking = false;
        FindObjectOfType<AnimaPersonaggio>().WalkMode(false);
        FindObjectOfType<AnimaPersonaggio>().ResetDestination();
    }
}
