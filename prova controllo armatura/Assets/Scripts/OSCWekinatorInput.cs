using extOSC;
using UnityEngine;

public class OSCWekinatorInput : MonoBehaviour
{
    public OSCTransmitter transmitter;
    public Transform cube;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        float yvalue =cube.position.y;
        var message = new OSCMessage("/wek/inputs");
        message.AddValue(OSCValue.Float(yvalue));
        transmitter.Send(message);
    }
}
