using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AnimaPersonaggio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject character;
    [SerializeField] private TMPro.TextMeshPro characterName;
    [SerializeField] private ActionsDataBase actionsDB;
    [SerializeField] private GameObject canvasParent;
    [SerializeField] private SimulationManager simulationManager;
    [SerializeField] private GridObjectCollection buttonCollection;
    [SerializeField] private ObjectManagerVR objectManager;

    [SerializeField] GameObject GetControlButton;

    [SerializeField] private GameObject moveButton;
    private string[] selectedObjectActions;
    public GameObject actionButton;
    public GameObject otherButton;
    public GameObject tastiera;
    public TMP_InputField otherInput;
    public TMP_Text _transcribedText;

    private bool self;
    public GameObject actionsPanel;

    public GameObject StopAndPlayButton;

    public GameObject StopAndPlayButtonPadre;

    public GameObject MenuAzioniTasti;


    // public GameObject controlCharacter;

    public float selectionDistance = 1;
    public float selectionAngle = 1;
    public bool isWalking;
    private GameObject plane;
    private NavMeshAgent navMeshAgent;
    private PhraseGenerator phraseGenerator;

    private GameObject interactionObject;

    public ButtonConfigHelper buttonConfigHelperStartStop;


    public bool OtherPremuto;
    public bool keyboardForDictaction;

    public float time = 0.15f;

    public TextMeshPro t;

     void Start()
    {
        simulationManager = FindObjectOfType<SimulationManager>();
        phraseGenerator = FindObjectOfType<PhraseGenerator>();

        objectManager = FindObjectOfType<ObjectManagerVR>();

        OtherPremuto = false;

        actionsPanel.SetActive(false);

        buttonConfigHelperStartStop = StopAndPlayButton.GetComponent<ButtonConfigHelper>();

        buttonConfigHelperStartStop.SetQuadIconByName("IconHandMesh");
       
        buttonConfigHelperStartStop.MainLabelText = "STOP";


    }

    // Update is called once per frame
    void Update()
    {
        


        if (character != null && character.CompareTag("Player"))
        {
            if (character != null)
            {
                navMeshAgent = character.GetComponent<NavMeshAgent>();
            }


            if (GetControlButton != null)
            {
                GetControlButton.transform.position = new Vector3(character.transform.position.x + 0.01f, 1.57f * character.transform.position.y, character.transform.position.z);

            }



            /*if (isWalking)
            {
                plane = GameObject.FindGameObjectWithTag("PlaneTable");
                character.GetComponent<CharacterManager>().isWalking = true;
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            character.GetComponent<Animator>().SetBool("walking", false);
                            character.GetComponent<CharacterManager>().isWalking = false;
                        }
                    }
                }
            }
            else
            {
                if (character != null)
                    character.GetComponent<CharacterManager>().isWalking = false;
            }*/
        }

        /*else
        {
            moveButton.SetActive(false);
            StopAndPlayButton.SetActive(false);
        }*/

      

        if (simulationManager.activeCharacter != null)
        {
            if ((simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "sitting" || simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "playing") && SimulationManager.status == 1)
            {
                moveButton.GetComponent<Interactable>().IsEnabled = false;
                simulationManager.activeCharacter.GetComponent<CharacterManager>().StopWalking();
                ChangeWalker();
               
            }
            else
            {
                moveButton.GetComponent<Interactable>().IsEnabled = true;
            }

          

            if ((simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "dancing" || simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "working out" || 
                simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "sitting" || simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "playing") && SimulationManager.status == 1)
            {
                foreach (Transform child in canvasParent.transform)
                { 
                    if (child.name == "PLAY" || child.name == "SIT")
                    {
                        child.GetComponent<Interactable>().IsEnabled = false;
                        t = child.Find("IconAndText").Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
                        t.color = Color.grey;
                    }
                }

            } else
            {
                foreach (Transform child in canvasParent.transform)
                {
                    if (child.name == "PLAY" || child.name == "SIT")
                    {
                        child.GetComponent<Interactable>().IsEnabled = true;
                        t = child.Find("IconAndText").Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
                        t.color = Color.white;
                    }
                }
            }
        }

        



    }

    public void PosizioneTasti()
    {
        StartCoroutine(Aspetta());
    }

    public void PosizioneTastiSopraPersonaggio()
    {
        if (actionsPanel.activeSelf == true && simulationManager.activeCharacter != null && isWalking == false)
        {
            //actionsPanel.transform.position = new Vector3(simulationManager.activeCharacter.transform.position.x, 1.56f * simulationManager.activeCharacter.transform.position.y, simulationManager.activeCharacter.transform.position.z);
            //StopAndPlayButtonPadre.transform.position = new Vector3(simulationManager.activeCharacter.transform.position.x + 0.04f, simulationManager.activeCharacter.transform.position.y + (simulationManager.activeCharacter.transform.position.y) / 2, simulationManager.activeCharacter.transform.position.z);
            //moveButton.transform.position = new Vector3(simulationManager.activeCharacter.transform.position.x + 0.13f, simulationManager.activeCharacter.transform.position.y + (simulationManager.activeCharacter.transform.position.y) / 2, simulationManager.activeCharacter.transform.position.z);
            MenuAzioniTasti.transform.position = new Vector3(simulationManager.activeCharacter.transform.position.x + 0.04f, simulationManager.activeCharacter.transform.position.y + (simulationManager.activeCharacter.transform.position.y) / 2, simulationManager.activeCharacter.transform.position.z);

        }
    }

    public void FindDestination(MixedRealityPointerEventData eventData)
    {
        if (isWalking && simulationManager.activeCharacter.CompareTag("Player"))
        {
            simulationManager.activeCharacter.GetComponent<Animator>().speed = 0.5f;
            simulationManager.activeCharacter.GetComponent<NavMeshAgent>().Resume();

            // cambia testo e icona tasto Play e Stop
            buttonConfigHelperStartStop.SetQuadIconByName("IconHandMesh");
            buttonConfigHelperStartStop.MainLabelText = "STOP";

            var result = eventData.Pointer.Result;
            var q = Quaternion.LookRotation(result.Details.Point - simulationManager.activeCharacter.transform.position);
            simulationManager.activeCharacter.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 3 * Time.deltaTime);

            var navMeshAgent = simulationManager.activeCharacter.GetComponent<NavMeshAgent>();
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(result.Details.Point, out hit, 1.0f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(result.Details.Point);
                simulationManager.activeCharacter.GetComponent<Animator>().SetBool("walking", true);
                simulationManager.activeCharacter.GetComponent<CharacterManager>().CheckDestination();
            }




            RaycastHit point;
            Vector3 r = new Vector3(result.Details.Point.x, simulationManager.activeCharacter.transform.position.y, result.Details.Point.z);
            Vector3 direction = (r - simulationManager.activeCharacter.transform.position);

            if (Physics.Raycast(simulationManager.activeCharacter.transform.position, direction, out point, 1f))
            {
                bool p = false;
                if (point.transform.gameObject.GetComponent<CharacterManager>() != null)
                {
                    p = point.transform.gameObject.name == point.transform.gameObject.GetComponent<CharacterManager>().type;
                }

                //Debug.Log(point.transform.name);
                phraseGenerator.UpdateForward(point.transform.gameObject.name, p);
                phraseGenerator.GenerateMovementPhrase(simulationManager.activeCharacter.name, simulationManager.activeCharacter.GetComponent<CharacterManager>().type, point.transform.gameObject.name, point.transform.gameObject.GetComponent<CharacterManager>().type);
            }
            else
            {
                phraseGenerator.GenerateMovementPhrase(simulationManager.activeCharacter.name, simulationManager.activeCharacter.GetComponent<CharacterManager>().type, "room", "room");
            }

        }
        

    }

    public void setCharacter(GameObject o) {

        actionsPanel.SetActive(false);

        WalkMode(false);
        
        if (o != null )
        {
            
                character = o;
                characterName.text = o.name;

                simulationManager.DestroyParticlesComplement();
                simulationManager.CreateParticleComplement(character);

            if (GameObject.Find("ParticleActive") == false)
            {
                
                simulationManager.CreateParticleActive(simulationManager.activeCharacter);
            }
               



            if (o.CompareTag("Player") && simulationManager.activeCharacter == o)
            {
                if (simulationManager.activeCharacter.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                {
                    moveButton.SetActive(true);
                }
                StopAndPlayButton.SetActive(true);
                
                self = true;

                
                ShowAloneActions();


                


                return;
            }
            else
            {
                interactionObject = o;
            }
        }
            /*if (o.CompareTag("Player"))
            {
                controlCharacter.SetActive(true); //bottone per prendere controllo del personaggio

                if (simulationManager.activeCharacter == null) //se è la prima volta che un personaggio viene selezionato
                {
                    simulationManager.SetActiveCharacter(o);
                    controlCharacter.SetActive(false);
                    return;
                }
            }
            else
            {
                controlCharacter.SetActive(false);
            }*/

            //si pone il criterio di vicinanza e puntamento dell'oggetto <- personaggio attivo
        if (simulationManager.activeCharacter != null && simulationManager.activeCharacter != character)
        {
            
           // if (Vector3.Distance(simulationManager.activeCharacter.transform.position, character.transform.position) < selectionDistance && Vector3.Angle(simulationManager.activeCharacter.transform.forward, character.transform.position - simulationManager.activeCharacter.transform.position) < selectionAngle)

                self = false;
            {
                characterName.text = o.name;


                
                ShowActions();

                


            }
        }
    }



    public void ResetDestination()
    {
        foreach (GameObject obj in objectManager.objectsInScene)
        {
            if (obj.CompareTag("Player"))
            {
                var navMeshAgent = obj.GetComponent<NavMeshAgent>();
                if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                    navMeshAgent.ResetPath();
            }
        }
    }


    public void startStop()
    {
        var speed = simulationManager.activeCharacter.GetComponent<Animator>().speed;
        
        if (speed == 0)
        {           
            simulationManager.activeCharacter.GetComponent<Animator>().speed = 0.5f;

            // cambia testo e icona tasto Play e Stop
            buttonConfigHelperStartStop.SetQuadIconByName("IconHandMesh");
            buttonConfigHelperStartStop.MainLabelText = "STOP";

            if (isWalking)
            {
                
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().Resume();
            }
        }
        else
        {            
            simulationManager.activeCharacter.GetComponent<Animator>().speed = 0;

            // cambia testo e icona tasto Play e Stop
            buttonConfigHelperStartStop.SetQuadIconByName("IconPlay");
            buttonConfigHelperStartStop.MainLabelText = "PLAY";

            if (isWalking)
            {
                
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().Stop();
            }
        }
    }

    public void PlayAll()
    {
        objectManager = FindObjectOfType<ObjectManagerVR>();
        foreach (GameObject o in objectManager.objectsInScene)
        {
            if (o.GetComponent<Animator>() != null )
            {
                o.GetComponent<Animator>().speed = 0.5f;

                // cambia testo e icona tasto Play e Stop
                buttonConfigHelperStartStop.SetQuadIconByName("IconHandMesh");
                buttonConfigHelperStartStop.MainLabelText = "STOP";

                if (o.GetComponent<NavMeshAgent>() != null && o.GetComponent<NavMeshAgent>().enabled)
                {
                    o.GetComponent<NavMeshAgent>().Resume();
                }
            }
        }
    }

    public void StopAll()
    {
        objectManager = FindObjectOfType<ObjectManagerVR>();
        foreach (GameObject o in objectManager.objectsInScene)
        {
            if (o.GetComponent<Animator>() != null)
            {
                o.GetComponent<Animator>().speed = 0;

                // cambia testo e icona tasto Play e Stop
                buttonConfigHelperStartStop.SetQuadIconByName("IconPlay");
                buttonConfigHelperStartStop.MainLabelText = "PLAY";

                if (o.GetComponent<NavMeshAgent>() != null && o.GetComponent<NavMeshAgent>().enabled)
                {
                    o.GetComponent<NavMeshAgent>().Stop();
                }
            }
        }
    }



    public void ShowAloneActions()
    {
        Debug.Log("ShowAloneActions");
        //actionsPanel.SetActive(true);
        foreach (Transform child in canvasParent.transform)
        {
            GameObject.Destroy(child.gameObject);
            
        }
        if (actionsDB.ReturnActions(simulationManager.activeCharacter.GetComponent<CharacterManager>().type, character.GetComponent<CharacterManager>().type, character.GetComponent<State>().state, self) != null)
        {
            selectedObjectActions = actionsDB.ReturnActions(simulationManager.activeCharacter.GetComponent<CharacterManager>().type, character.GetComponent<CharacterManager>().type, character.GetComponent<State>().state, self);

            //Mantenimento delle sole azioni riflessive
            //selectedObjectActions = selectedObjectActions.Intersect(reflexList).ToArray();

            foreach (string s in selectedObjectActions)
            {
                if (s != null)
                {
                    GameObject b = Instantiate(actionButton);
                    b.GetComponent<ButtonConfigHelper>().MainLabelText = s.ToUpper();
                    b.name = s.ToUpper();

                    Color DefaltCustomColor;

                    if (b.name == "STOP DANCE" || b.name == "STOP PLAY" || b.name == "STAND UP" || b.name == "STOP WORK OUT")
                    {
                         DefaltCustomColor = new Color(0.4f, 0.145098f, 0.2862745f, 1); // granata 662549
                    }
                    else
                    {
                         DefaltCustomColor = new Color(0.682353f, 0.2666667f, 0.3529412f, 1); // granata leggero AE445A
                    }

                                       
                    Color FocusCustomColor = new Color(0.09803919f, 0.1490196f, 0.3333333f, 1);
                    Color PressedCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
                    Color DisabledCustomColor = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1);
                    var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableColorTheme>().Value;
                    var interactableObject = b.transform.Find("Backplate").Find("Quad").gameObject;

                    // Define a color for every state in our Default Interactable States
                    newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
                            {
                                new ThemePropertyValue() { Color = DefaltCustomColor},  // Default
                                new ThemePropertyValue() { Color = FocusCustomColor}, // Focus
                                new ThemePropertyValue() { Color = PressedCustomColor},   // Pressed
                                new ThemePropertyValue() { Color = DisabledCustomColor},   // Disabled
                            };

                    
                    
                   

                    b.GetComponent<Interactable>().Profiles = new List<InteractableProfileItem>()
                            {
                                new InteractableProfileItem()
                                {
                                    Themes = new List<Theme>()
                                    {
                                        Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType })
                                    },
                                    Target = interactableObject,
                                },
                            };


                    b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ActionClick(s));
                    b.transform.SetParent(canvasParent.transform, false);
                }
                
            }
        }

        
        OtherActionAlone();
       
    }

    public void ShowActions()
    {

        Debug.Log("ShowActions");
        //actionsPanel.SetActive(true);
        foreach (Transform child in canvasParent.transform)
        {
            GameObject.Destroy(child.gameObject);
            
        }

        if (actionsDB.ReturnActions(simulationManager.activeCharacter.GetComponent<CharacterManager>().type, character.GetComponent<CharacterManager>().type, character.GetComponent<State>().state, self) != null)
        {
            selectedObjectActions = actionsDB.ReturnActions(simulationManager.activeCharacter.GetComponent<CharacterManager>().type, character.GetComponent<CharacterManager>().type, character.GetComponent<State>().state, self);
        }
       
        if (selectedObjectActions != null)
        {
            foreach (string s in selectedObjectActions)
            {
                if (s != null)
                {
                    
                    GameObject b = Instantiate(actionButton);
                    b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ActionClick(s));
                    b.GetComponent<ButtonConfigHelper>().MainLabelText = s.ToUpper();
                    b.name = s.ToUpper();


                    // colori tasto
                    //Color DefaltCustomColor = new Color(0.9529411f, 0.6235294f, 0.3529411f, 1); // giallo F39F5A

                    Color DefaltCustomColor = new Color(0.3333333f, 0.4784314f, 0.2745098f, 1); // verde 557A46

                    Color FocusCustomColor = new Color(0.09803919f, 0.1490196f, 0.3333333f, 1);
                    Color PressedCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
                    Color DisabledCustomColor = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1);

                   

                    var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableColorTheme>().Value;
                    var interactableObject = b.transform.Find("Backplate").Find("Quad").gameObject;

                    

                    // Define a color for every state in our Default Interactable States
                    newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
                            {
                                new ThemePropertyValue() { Color = DefaltCustomColor},  // Default
                                new ThemePropertyValue() { Color = FocusCustomColor}, // Focus
                                new ThemePropertyValue() { Color = PressedCustomColor},   // Pressed
                                new ThemePropertyValue() { Color = DisabledCustomColor},   // Disabled
                            };

                    
                    
                   
                    

                            b.GetComponent<Interactable>().Profiles = new List<InteractableProfileItem>()
                            {
                                new InteractableProfileItem()
                                {
                                    Themes = new List<Theme>()
                                    {
                                        Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType,  })
                                    },

                                    Target = interactableObject, 

                                },

                            };

                  
                    //b.transform.Find("Backplate").Find("Quad").GetComponent<Renderer>().material.SetColor("_Color", customColor);
                       
                    b.transform.SetParent(canvasParent.transform, false);
                }
                
            }

        }
       //buttonCollection.UpdateCollection();
       OtherAction();
        
    }

    public void OtherActionAlone()
    {
        
        GameObject b = Instantiate(otherButton);
        b.name = "ZZOTHER";
        //b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ActionClick("Other"));
        b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ApriTastiera());
        b.GetComponent<ButtonConfigHelper>().MainLabelText = "OTHER";





        // colori tasto


        Color DefaltCustomColor = new Color(0.682353f, 0.2666667f, 0.3529412f, 1); // granata leggero AE445A
        Color FocusCustomColor = new Color(0.09803919f, 0.1490196f, 0.3333333f, 1);
        Color PressedCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
        Color DisabledCustomColor = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1);
        var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableColorTheme>().Value;
        var interactableObject = b.transform.Find("Backplate").Find("Quad").gameObject;

        // Define a color for every state in our Default Interactable States
        newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
                            {
                                new ThemePropertyValue() { Color = DefaltCustomColor},  // Default
                                new ThemePropertyValue() { Color = FocusCustomColor}, // Focus
                                new ThemePropertyValue() { Color = PressedCustomColor},   // Pressed
                                new ThemePropertyValue() { Color = DisabledCustomColor},   // Disabled
                            };

       
        
        

        b.GetComponent<Interactable>().Profiles = new List<InteractableProfileItem>()
                            {
                                new InteractableProfileItem()
                                {
                                    Themes = new List<Theme>()
                                    {
                                        Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType })
                                    },
                                    Target = interactableObject,
                                },
                            };

        //b.transform.Find("Backplate").Find("Quad").GetComponent<Renderer>().material.SetColor("_Color", customColor);
        b.transform.SetParent(canvasParent.transform, false);


        //GameObject i = Instantiate(otherInput);

        //_newText.onValueChanged.AddListener(ChangeOtherAction);

        StartCoroutine(InvokeUpdateCollection());
        //buttonCollection.UpdateCollection();
    }


    public void OtherAction()
    {

        GameObject b = Instantiate(otherButton);
        b.name = "ZZOTHER";
        //b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ActionClick("Other"));
        b.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => ApriTastiera());
        b.GetComponent<ButtonConfigHelper>().MainLabelText = "OTHER";





        // colori tasto
        Color DefaltCustomColor = new Color(0.3333333f, 0.4784314f, 0.2745098f, 1); // verde 557A46

        Color FocusCustomColor = new Color(0.09803919f, 0.1490196f, 0.3333333f, 1);
        Color PressedCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
        Color DisabledCustomColor = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1);
        var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableColorTheme>().Value;
        var interactableObject = b.transform.Find("Backplate").Find("Quad").gameObject;

        // Define a color for every state in our Default Interactable States
        newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
                            {
                                new ThemePropertyValue() { Color = DefaltCustomColor},  // Default
                                new ThemePropertyValue() { Color = FocusCustomColor}, // Focus
                                new ThemePropertyValue() { Color = PressedCustomColor},   // Pressed
                                new ThemePropertyValue() { Color = DisabledCustomColor},   // Disabled
                            };





        b.GetComponent<Interactable>().Profiles = new List<InteractableProfileItem>()
                            {
                                new InteractableProfileItem()
                                {
                                    Themes = new List<Theme>()
                                    {
                                        Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType })
                                    },
                                    Target = interactableObject,
                                },
                            };

        //b.transform.Find("Backplate").Find("Quad").GetComponent<Renderer>().material.SetColor("_Color", customColor);
        b.transform.SetParent(canvasParent.transform, false);


        //GameObject i = Instantiate(otherInput);

        //_newText.onValueChanged.AddListener(ChangeOtherAction);

        StartCoroutine(InvokeUpdateCollection());
        //buttonCollection.UpdateCollection();
    }


    public void HideActions()
    {
        actionsPanel.SetActive(false);
        //controlCharacter.SetActive(false);
     }

    public void ApriTastiera()
    {
        tastiera.SetActive(true);
        OtherPremuto = true;
    }

    public void SetOther(bool l)
    {
        OtherPremuto = l;
    }

    public void StopOldLongAnimation()
    {
        if (!simulationManager.contemporaryAction)
        {
            //string ultimaAzione = simulationManager.activeCharacter.GetComponent<CharacterManager>().lastAction;

            if (simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "working out")
            {
                string azione = "stop work out";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "dancing")
            {
                string azione = "stop dance";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "playing")
            {
                string azione = "stop play";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (simulationManager.activeCharacter.GetComponent<State>().GetCurrentState() == "sitting")
            {
                string azione = "stand up";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                simulationManager.activeCharacter.transform.Translate(0, 0, +0.05f);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }

        }
    }

    /*public void StopOldLongAnimation()
    {
        if (!simulationManager.contemporaryAction)
        {
            string ultimaAzione = simulationManager.activeCharacter.GetComponent<CharacterManager>().lastAction;

            if (ultimaAzione == "work out")
            {
                string azione = "stop work out";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (ultimaAzione == "dance")
            {
                string azione = "stop dance";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (ultimaAzione == "play")
            {
                string azione = "stop play";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }
            else if (ultimaAzione == "sit")
            {
                string azione = "stand up";
                simulationManager.activeCharacter.GetComponent<State>().ChangeState(azione);
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                simulationManager.activeCharacter.transform.Translate(0, 0, +0.05f);
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);

            }

        }
    }*/

    public void ActionClick(string action)
    {
        WalkMode(false);

        StopOldLongAnimation();

        simulationManager.activeCharacter.GetComponent<CharacterManager>().lastAction = action;
        simulationManager.activeCharacter.GetComponent<CharacterManager>().lastTimeAction = simulationManager.GetScreenshotCount();

       

        // cambia testo e icona tasto Play e Stop
            buttonConfigHelperStartStop.SetQuadIconByName("IconHandMesh");
        buttonConfigHelperStartStop.MainLabelText = "STOP";

        if (action == "smile")
        {
            simulationManager.activeCharacter.GetComponent<Animator>().speed = 0.5f;
        }


        if (character.CompareTag("Player") && simulationManager.activeCharacter.CompareTag("Player") && character != simulationManager.activeCharacter)
        {
            var q = Quaternion.LookRotation(character.transform.position - simulationManager.activeCharacter.transform.position);
            simulationManager.activeCharacter.transform.rotation = q;

            var r = Quaternion.LookRotation(simulationManager.activeCharacter.transform.position - character.transform.position);
            character.transform.rotation = r;
        }
       

        if (action == "talk" || action == "talk to")
        {
            //simulationManager.dialogue = true;
            phraseGenerator.StartSpeech();
            SetKeyboardForDictaction(true);
            //HideActions();
            //simulationManager.DestroyParticles();

            //distruggi particelle
            simulationManager.DestroyParticlesActive();
            //crea particlle
            simulationManager.CreateParticleActive(simulationManager.activeCharacter);
        }

        else
        {

            //notifica lo state di avviare l'eventuale animazione dell'oggetto che subisce l'azione
            character.GetComponent<State>().PlayAnimation(action);

            //notifica il simulation manager di avviare animazione del personaggio attivo
            simulationManager.PlayActiveCharacterAnimation(action);

            //Genera la frase
            phraseGenerator.GenerateSimplePhrase(simulationManager.activeCharacter.name, simulationManager.activeCharacter.GetComponent<CharacterManager>().type, action, character.name, character.GetComponent<CharacterManager>().type, self);

            

            if (action != "sit" && action != "stand up" && action != "play" && action != "stop play" && action != "dance" && action != "stop dance" && action != "work out" && action != "stop work out")
            {
                //notifica lo State del gameobject la cui azione è stata cliccata per effettuare un controllo di cambio di stato
                character.GetComponent<State>().ChangeState(action);

            }
            else
            {

                simulationManager.activeCharacter.GetComponent<State>().ChangeState(action);

            }

            //pick & place
            if (action == "pick")
            {
                character.transform.parent = simulationManager.activeCharacter.transform;
                //selectedObject.transform.position = new Vector3(simulationManager.activeCharacter.transform.position.x + 1f, selectedObject.transform.position.y + 0.5f, simulationManager.activeCharacter.transform.localPosition.z);
                character.transform.position = simulationManager.activeCharacter.transform.position + simulationManager.activeCharacter.transform.forward * 1f; ;
            }
            else if (action == "place")
            {
                character.transform.parent = GameObject.Find("BuildingManager").transform;
                character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z);
            }
            else if (action == "sit")
            {
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = false;

                if (character.GetComponent<CharacterManager>().type == "bench")
                {
                    simulationManager.activeCharacter.transform.position = new Vector3(character.transform.position.x, character.transform.position.y - 0.005f, character.transform.position.z - 0.035f);
                }

                if (character.GetComponent<CharacterManager>().type == "chair")
                {
                    simulationManager.activeCharacter.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z + 0.015f);
                }
                simulationManager.activeCharacter.transform.localRotation = Quaternion.Euler(character.transform.localRotation.eulerAngles);
            }
            else if (action == "stand up" )
            {
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                simulationManager.activeCharacter.transform.Translate(0,0, +0.05f);

            }

            else if (action == "stop play")
            {
                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = true;
                
            }

            else if (action == "play")
            {

                simulationManager.activeCharacter.GetComponent<NavMeshAgent>().enabled = false;
                simulationManager.activeCharacter.transform.localRotation = Quaternion.Euler(character.transform.localRotation.eulerAngles.x,  character.transform.localRotation.eulerAngles.y + 180f , character.transform.localRotation.eulerAngles.z);

                simulationManager.activeCharacter.transform.position = new Vector3(character.transform.position.x , character.transform.position.y - 0.07f, character.transform.position.z - 0.0155f);

            }

            //

            //HideActions();

            if (action == "work out" || action == "dance" || action == "play" || action == "sit" || action == "stop dance" || action == "stop work out" || action == "stop play" || action == "stand up")
            {
                simulationManager.SetActiveCharacterActionClick(simulationManager.activeCharacter);
            }
            else
            {
                //distruggi particelle
                simulationManager.DestroyParticlesActive();
                //crea particlle
                simulationManager.CreateParticleActive(simulationManager.activeCharacter);
            }



            Debug.Log("Action: " + action);
            

        }

        ChangeWalker();

        if (!simulationManager.contemporaryAction)
        {
            simulationManager.activeCharacter.GetComponent<CharacterManager>().StopWalking();
        }

        phraseGenerator.AggiornaTesto();
    }

   

    // chiamato da DONE in dictation (deprecato) // chiamato da KeyboardForDictaction 
    public void Talk()
    {
        simulationManager.PlayActiveCharacterAnimation("talk to");
        phraseGenerator.GenerateSimplePhrase(simulationManager.activeCharacter.name, simulationManager.activeCharacter.GetComponent<CharacterManager>().type, "talk to", character.name, character.GetComponent<CharacterManager>().type, self);
        //HideActions();
        //simulationManager.DestroyParticles();

        phraseGenerator.AggiornaTesto();
    }

    // messo a false da Close della tastiera nel caso in cui non si preme Enter, messo a true quando si preme l'azione Talk to
    public void SetKeyboardForDictaction(bool l)
    {
        keyboardForDictaction = l;
    }


    // chiamato da Enter della tastiera, serve per scrivere il dialogo o dettarlo tramite la tastiera
    public void KeyboardForDictaction()
    {
        if (keyboardForDictaction)
        {
            //_transcribedText.text = otherInput.text; //dictation
            Talk();
        }
        
        keyboardForDictaction = false;
    }


    //chiamato dall'Inspector quando premo Enter sulla tastiera virtuale
    public void ChangeOther()
    {
        if (OtherPremuto == true)
        {
            string customAction = otherInput.text;

            if (string.IsNullOrEmpty(customAction))
            {
                customAction = "other";
            }

            //Debug.Log( "testoInput: " + otherInput.text);
            //Debug.Log("testo: " + customAction);

            ActionClick(customAction);

            OtherPremuto = false;

            simulationManager.activeCharacter.GetComponent<Animator>().speed = 0.5f; // visto che Other non ha una animazione che fa ripartire lo stato di Stop
        }
        else return;
    }


    public bool ActiveWalk = false;

    public void ChangeWalker()
    {
        if (ActiveWalk)
            ActivateWalkMode(true);
        
    }

    public void ActivateWalkMode(bool walk)
    {
        Color DefaltCustomColor;

        if (ActiveWalk)
        {
            ActiveWalk = false;
            DefaltCustomColor = new Color(0.682353f, 0.2666667f, 0.3529412f, 1); //granata leggero AE445A
           
            isWalking = false;
           
        }
        else
        {
            DefaltCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
            ActiveWalk = true;
            StopOldLongAnimation();


        }


        Color FocusCustomColor = new Color(0.09803919f, 0.1490196f, 0.3333333f, 1);
        Color PressedCustomColor = new Color(0.5526878f, 0.582599f, 0.8679245f, 1);
        Color DisabledCustomColor = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1);
        var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableColorTheme>().Value;
        var interactableObject = moveButton.transform.Find("BackPlate").Find("Quad").gameObject;
        newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
        {
            new ThemePropertyValue() { Color = DefaltCustomColor},  // Default
            new ThemePropertyValue() { Color = FocusCustomColor}, // Focus
            new ThemePropertyValue() { Color = PressedCustomColor},   // Pressed
            new ThemePropertyValue() { Color = DisabledCustomColor},   // Disabled
        };

        moveButton.GetComponent<Interactable>().Profiles = new List<InteractableProfileItem>()
        {
            new InteractableProfileItem()
            {
                Themes = new List<Theme>()
                {
                    Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType })
                },
                Target = interactableObject,
            },
        };

        if (simulationManager.activeCharacter != null)
        {
            simulationManager.activeCharacter.GetComponent<CharacterManager>().isWalking = true;
        }
        WalkMode(walk);
    }

    public void WalkMode(bool walk)
    {
        if (ActiveWalk)
        {
            isWalking = walk;
            if (simulationManager.activeCharacter != null)
            {
                simulationManager.activeCharacter.GetComponent<CharacterManager>().isWalking = true;
               
            }
        }
        

    }

    private IEnumerator InvokeUpdateCollection()
    {
        yield return null;
        
        buttonCollection.UpdateCollection();
        StartCoroutine(ShowPannel());
    }

    private IEnumerator ShowPannel()
    {
        yield return new WaitForSeconds(time);

        actionsPanel.SetActive(true);
    }

    private IEnumerator Aspetta()
    {
        yield return new WaitForSeconds(0.15f);

        PosizioneTastiSopraPersonaggio();
    }

    

}
