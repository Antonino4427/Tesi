using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;


public class FingerCoordinatesValues : MonoBehaviour
{
    public OSCTransmitter transmitter;
    public string address = "/wek/inputs"; // indirizzo OSC atteso da Wekinator
    public string startAddress = "/wekinator/control/startDtwRecording"; //inidirizz per iniziare e terminare la registrazione del dynaminc time warping
    public string stopAddress = "/wekinator/control/stopDtwRecording";

    private float angle1;
    private bool isRecording = false;

    public Transform R_thumb1;
    public Transform R_thumb2;
    public Transform R_thumb3;
    public Transform R_index1;
    public Transform R_index2;
    public Transform R_index3;
    public Transform R_middle1;
    public Transform R_middle2;
    public Transform R_middle3;
    public Transform R_ring1;
    public Transform R_ring2;
    public Transform R_ring3;
    public Transform R_pinky1;
    public Transform R_pinky2;
    public Transform R_pinky3;
    public Transform R_wrist;


    void Update()
    {


        #region Messagge construction
        // Crea il messaggio OSC
        var message = new OSCMessage(address);
        // aggiungo valori di rotazione su z solo della base delle dita, falangine e falangette non hanno questa rotazione anatomica
        message.AddValue(OSCValue.Float(R_thumb1.localRotation.z));
        message.AddValue(OSCValue.Float(R_index1.localRotation.z));
        message.AddValue(OSCValue.Float(R_middle1.localRotation.z));
        message.AddValue(OSCValue.Float(R_ring1.localRotation.z));
        message.AddValue(OSCValue.Float(R_pinky1.localRotation.z));
        //poi aggiungo le rotazioni su x di tutto il resto
        message.AddValue(OSCValue.Float(R_thumb1.localRotation.x));
        message.AddValue(OSCValue.Float(R_thumb2.localRotation.x));
        message.AddValue(OSCValue.Float(R_thumb3.localRotation.x));

        message.AddValue(OSCValue.Float(R_index1.localRotation.x));
        message.AddValue(OSCValue.Float(R_index2.localRotation.x));
        message.AddValue(OSCValue.Float(R_index3.localRotation.x));

        message.AddValue(OSCValue.Float(R_middle1.localRotation.x));
        message.AddValue(OSCValue.Float(R_middle2.localRotation.x));
        message.AddValue(OSCValue.Float(R_middle3.localRotation.x));

        message.AddValue(OSCValue.Float(R_ring1.localRotation.x));
        message.AddValue(OSCValue.Float(R_ring2.localRotation.x));
        message.AddValue(OSCValue.Float(R_ring3.localRotation.x));

        message.AddValue(OSCValue.Float(R_pinky1.localRotation.x));
        message.AddValue(OSCValue.Float(R_pinky2.localRotation.x));
        message.AddValue(OSCValue.Float(R_pinky3.localRotation.x));
        // aggiungo la rotazione del polso su tutti e tre gli assi
        message.AddValue(OSCValue.Float(R_wrist.localRotation.x));
        message.AddValue(OSCValue.Float(R_wrist.localRotation.y));
        message.AddValue(OSCValue.Float(R_wrist.localRotation.z));
        //IN TOTALE SONO 23 VALORI
        #endregion
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //angle1 = bone1.localEulerAngles.x > 180f ?   bone1.localEulerAngles.x - 360f: bone1.localEulerAngles.x; ;
        //    Debug.Log("bone 1 " + angle1 );
        //angle1 = bone1.localRotation.x > 180f ? bone1.localRotation.x - 360f : bone1.localRotation.x;
        //Debug.Log("index: " + R_index1.localRotation.z);
        //Debug.Log("bone 2 " + bone2.localEulerAngles.x);
        //Debug.Log("bone 3 " + bone3.localEulerAngles.x);
        //}


        // Invia il messaggio
        transmitter.Send(message);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }
    }

    void StartRecording()
    {
        var message = new OSCMessage(startAddress);
        message.AddValue(OSCValue.Int(1));
        transmitter.Send(message);
        Debug.Log($"Iniziata registrazione DTW (classe {1})");
        isRecording = true;
    }

    void StopRecording()
    {
        var message = new OSCMessage(stopAddress);
        transmitter.Send(message);
        Debug.Log("Terminata registrazione DTW");
        isRecording = false;
    }
}
