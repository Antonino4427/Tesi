using UnityEngine;
using UnityEngine.UI;
using extOSC;


public class WekinatorExpressionSelection : MonoBehaviour
{
    public string address_12 = "/wek/outputs/mapping_12";
    public string address_4 = "/wek/outputs/mapping_4";

    public int port = 12000;

    private OSCReceiver receiver;
    public Animator animator;
    public Image fillImage; // assegnare l'immagine dalla UI


    private string[] animationStates_12 = {"root|smile", "root|Excited", "root|Delighted", "root|Content", "root|Relaxed", "root|Calm",
                                        "root|Angry", "root|Frustrated", "root|Tense", "root|Tired", "root|Bored", "root|Depressed"};
    private string[] animationStates_4 = { "root|Excited", "root|Relaxed", "root|Angry", "root|Depressed"};
    // esempio
    private string chosenAnimation;
    private int lastIndex = -1;
    private float timer = 0f; 
    private bool animationPlayed=false; // flag to check if animation has been played
    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = port;
        receiver.Bind(address_12, OnReceiveValues_12);
        receiver.Bind(address_4, OnReceiveValues_4);
        fillImage.fillAmount = 0f;
    }

    void OnReceiveValues_12(OSCMessage message)
    {
        if (message.Values.Count == 0) return;

        float maxVal = float.MinValue;
        int maxIndex = -1;

        for (int i = 0; i < message.Values.Count; i++)
        {
            float val = message.Values[i].FloatValue;
            if (val > maxVal)
            {
                maxVal = val;
                maxIndex = i;
            }
        }
        if (maxIndex < 0 || maxIndex > animationStates_12.Length-1)
        {
            Debug.LogWarning("Received index out of bounds: " + maxIndex);
            return;
        }
        if (maxIndex != lastIndex)
        {
            timer=0f; // reset timer if a new animation is selected
            fillImage.fillAmount = 0f; // reset the fill amount of the image
            Debug.Log("in posa: " + animationStates_12[maxIndex]);
            lastIndex = maxIndex;
            animationPlayed = false; // reset the flag when a new animation is selected

        }
        else
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / 2f); // update the fill amount based on the timer
            if (timer > 2f && animationPlayed==false ) { // if the same animation is selected for more than 2 seconds, play it
                Debug.Log("riproduco animazione: " + animationStates_12[maxIndex]);
                string chosenAnimation = animationStates_12[maxIndex];
                PlayAnimation(chosenAnimation);
                animationPlayed = true; // set the flag to true to indicate that the animation has been played
            }
        }
 

    }

    void OnReceiveValues_4(OSCMessage message)
    {
        if (message.Values.Count == 0) return;

        float maxVal = float.MinValue;
        int maxIndex = -1;

        for (int i = 0; i < message.Values.Count; i++)
        {
            float val = message.Values[i].FloatValue;
            if (val > maxVal)
            {
                maxVal = val;
                maxIndex = i;
            }
        }
        if (maxIndex < 0 || maxIndex > animationStates_4.Length - 1)
        {
            Debug.LogWarning("Received index out of bounds: " + maxIndex);
            return;
        }
        if (maxIndex != lastIndex)
        {
            timer = 0f; // reset timer if a new animation is selected
            fillImage.fillAmount = 0f; // reset the fill amount of the image
            Debug.Log("in posa: " + animationStates_4[maxIndex]);
            lastIndex = maxIndex;
            animationPlayed = false; // reset the flag when a new animation is selected

        }
        else
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / 2f); // update the fill amount based on the timer
            if (timer > 2f && animationPlayed == false)
            { // if the same animation is selected for more than 2 seconds, play it
                Debug.Log("riproduco animazione: " + animationStates_4[maxIndex]);
                string chosenAnimation = animationStates_4[maxIndex];
                PlayAnimation(chosenAnimation);
                animationPlayed = true; // set the flag to true to indicate that the animation has been played
            }
        }


    }

    void PlayAnimation(string animName)
    {
        animator.Play(animName, 0, 0); // layer 0, from beginning
    }
}
