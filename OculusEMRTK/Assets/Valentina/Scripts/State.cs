using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class State : MonoBehaviour
{
    public List<string> state; //Lista di stati correnti, per implementare eventualmente il multi-stato
    public List<string> possibleStates;
    public ActionsDataBase DataBase;
    public Animator animator;
    public PhraseGenerator phraseGenerator;
    [SerializeField] private SimulationManager simulationManager;

    private string initialState;
    public string surname;

    public List<string> nearObjs; //Oggetti vicini, validi solo per i Players

    //Questo script è assegnato ad ogni oggetto al momento della sua creazione in creation mode, nel BuildingManager (metodo SelectObject)
    void Start()
    {
        state = new List<string>();
        nearObjs = new List<string>();

        //surname = gameObject.name;
        DataBase = GameObject.Find("SimulationManager").GetComponent<ActionsDataBase>();
        phraseGenerator = GameObject.Find("SimulationManager").GetComponent<PhraseGenerator>();

        possibleStates = DataBase.GetPossibleStates(gameObject.GetComponent<CharacterManager>().type);

        simulationManager = FindObjectOfType<SimulationManager>();

        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();

        //Se l'oggetto può avere stati, il suo stato viene inizializzato al primo presente nella lista degli stati possibili
        if (possibleStates != null)
        {
            state.Add(possibleStates[0]);

            //Multistati binari     DA DEPRECARE
           /* if (possibleStates.Count > 2)
            {
                int i = 2;
                while (possibleStates.Count >= (i + 2))
                {
                    state.Add(possibleStates[i]);
                    i = i + 2;
                }
            }*/
        }
        if (possibleStates != null)
        {
            initialState = state[0];
        }
    }

    public void Update()
    {
        // aggiunto altrimenti nel testo generato non stampava sempre i nomi rinomati 
       // surname = gameObject.name;
    }

    public void ChangeState(string action) {
        string[] transition;
        if (DataBase.GetStateTransition(action, gameObject.GetComponent<CharacterManager>().type) != null)
        {
            transition = DataBase.GetStateTransition(action, gameObject.GetComponent<CharacterManager>().type);
            if (state[0] == transition[0])
            {
                ChangeStateAnimation(state[0], transition[2]);
                state[0] = transition[2];
                //phraseGenerator.GenerateStatusPhrase(gameObject.GetComponent<CharacterManager>().type, state[0]);


                /* //Suona eventuali stati
                if (GetComponent<AudioSource>() != null) 
                { 
                if (GetComponent<AudioSource>()?.clip.name == state[0])
                {
                    GetComponent<AudioSource>().Play();
                    //Debug.Log("playing music!");
                }
                if (state[0] == "muted")
                {
                    GetComponent<AudioSource>()?.Stop();
                }
                }*/
                

                //Controllo particle
                /*foreach (Transform c in transform)
                {
                    if (c.gameObject.name == state[0])
                    {
                        if (c.GetComponent<ParticleSystem>() != null)
                        {
                            c.GetComponent<ParticleSystem>().Play();
                        }
                    }

                }
                if (state[0] == "extinct")
                {
                    foreach (Transform c in transform)
                    {
                        if (c.gameObject.name == "burning")
                        {
                            if (c.GetComponent<ParticleSystem>() != null)
                            {
                                c.GetComponent<ParticleSystem>().Stop();
                            }
                        }

                    }
                }*/


            }

            //Multistati binari


            if (state.Count > 1)
            {
                for (int i = 1; i < state.Count; i++)
                {
                    if (state[i] == transition[0])
                    {
                        
                        // Memorizza il nome dell'elemento da rimuovere
                        string stateToRemove = state[i];

                        // Rimuovi tutti gli stati con lo stesso nome
                        for (int j = i; j < state.Count; j++)
                        {
                            if (state[j] == stateToRemove)
                            {
                                state.RemoveAt(j);
                                j--; // Decrementa j per continuare a controllare lo stesso indice
                            }
                        }

                      
                        //phraseGenerator.GenerateStatusPhrase(gameObject.GetComponent<CharacterManager>().type, stateToRemove);

                        /* //Suona eventuali stati
                        if (GetComponent<AudioSource>() != null)
                        {
                            if (GetComponent<AudioSource>()?.clip.name == state[i])
                            {
                                GetComponent<AudioSource>().Play();
                                //Debug.Log("playing music!");
                            }
                            if (state[i] == "muted")
                            {
                                GetComponent<AudioSource>()?.Stop();
                            }
                        }*/


                   }
               }
           }


           // vecchio codice di quello sopra che però andava a stampare la stessa frase più volte 
           /*if (state.Count > 1)
            {
                int i = 1;
                while (state.Count >= (i+1))
                {
                    if (state[i] == transition[0])
                    {
                        state[i] = transition[2];
                        phraseGenerator.GenerateStatusPhrase(gameObject.name, state[i]);


             /* //Suona eventuali stati
                        if (GetComponent<AudioSource>() != null)
                        {
                            if (GetComponent<AudioSource>()?.clip.name == state[i])
                            {
                                GetComponent<AudioSource>().Play();
                                //Debug.Log("playing music!");
                            }
                            if (state[i] == "muted")
                            {
                                GetComponent<AudioSource>()?.Stop();
                            }
                        }


                    }
                    i++;
                }
            }*/
                    }
                }

    public void SetState(string s, string action)
    {
        if (action == "play" || action == "sit")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        ChangeStateAnimation(state[0], s);

        state[0] = s;
        //phraseGenerator.GenerateStatusPhrase(gameObject.GetComponent<CharacterManager>().type, state[0]);
    }

    //Animazioni passive
    public void PlayAnimation(string action) {
        if (animator != null)
        {
            //Se è un oggetto passivo, effettua l'animazione corrispondente all'azione
            if (CompareTag("Object"))
            {
                foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
                {
                    if (ac.name == action)
                    {
                        animator.Play(ac.name);
                        return;
                    }
                }
            }
            //Se è un personaggio passivo, effettua l'animazione passiva corrispondente all'azione, cercando quelle con prefisso "get"
            if (CompareTag("Player"))
            {
                foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
                {
                    if (ac.name == "get " + action)
                    {
                        animator.Play(ac.name);
                        return;
                    }
                }
            }
        }

        /*//Souno eventuale
        if (GetComponent<AudioSource>() != null)
        {
            if (GetComponent<AudioSource>().clip.name == action)
                GetComponent<AudioSource>().Play();
        }*/

    }

    //Metodo invocato dal ThirdPersonMovement
    /*public void NewNearObjects(List<string> newNearObjects)
    {
        var d = false;
        if (nearObjs.Count == newNearObjects.Count)
        {
            for (int i = 0; i < nearObjs.Count; i++)
                {
                if (!(nearObjs[i] == newNearObjects[i]))
                {
                    d = true;
                }
                }
        }
        else d = true;

        if (d)
        {
            nearObjs.Clear();
            foreach (string s in newNearObjects)
                nearObjs.Add(s);
            phraseGenerator.GenerateNearObjectsPhrase(simulationManager.activeCharacter.GetComponent<CharacterManager>().type, name, nearObjs);
        }
    }*/

    public void ChangeStateAnimation(string Oldstate, string newState)
    {
        if (animator != null)
        {
            if (CompareTag("Player"))
            {
                if (Oldstate != initialState)
                {
                    animator.SetBool(Oldstate, false);
                }
                animator.SetBool(newState, true);
                simulationManager.activeCharacter.GetComponent<Animator>().speed = 0.5f;

            }
        }
    }

    public string GetCurrentState()
    {
        return state[0];
    }

}