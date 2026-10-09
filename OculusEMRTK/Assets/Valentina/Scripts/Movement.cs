using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

/// <summary>
/// Moves the player around the world using the gamepad, or any other input action that supports 2D axis.
/// 
/// We extend InputSystemGlobalHandlerListener because we always want to listen for the gamepad joystick position
/// We implement InputHandler<Vector2> interface in order to receive the 2D navigation action events.
/// </summary>
public class Movement : InputSystemGlobalHandlerListener, IMixedRealityInputHandler<Vector2>
{
    public MixedRealityInputAction navigationAction;
    public MixedRealityInputAction rotationAndHeightAction;
   // public float multiplier = 2f; //modificata da Inspector

    private Vector3 deltaMovement = Vector3.zero;
    private float deltaRotation = 0f;
    private float deltaTranslation = 0f;

    

    public float rotationSpeed = 30f; // Velocità di rotazione
    public float translationSpeed = 0.3f; // Velocità di traslazione lungo l'asse Y, modificata da Inspector

    public float altezza;

    void Start()
    {
        altezza = CameraCache.Main.transform.position.y;
    }
    

    public void OnInputChanged(InputEventData<Vector2> eventData)
    {
        float horiz = eventData.InputData.x;
        float vert = eventData.InputData.y;

        float rotationInput = eventData.InputData.x;
        float height = eventData.InputData.y;

        if (eventData.MixedRealityInputAction == navigationAction)
        {
            deltaMovement = CameraCache.Main.transform.TransformDirection(new Vector3(horiz, 0, vert)) * 2 * translationSpeed;
        }

        if (eventData.MixedRealityInputAction == rotationAndHeightAction)
        {
            // Gestisci la rotazione attorno all'asse Y
            deltaRotation = rotationInput * rotationSpeed;

            // Gestisci la traslazione lungo l'asse Y
            deltaTranslation = height * translationSpeed;
            
        }
    }

    public void Update()
    {
        if (deltaMovement.sqrMagnitude > 0.0001f)
        {
           // Debug.Log("delta" + delta);
            MixedRealityPlayspace.Transform.Translate(deltaMovement * Time.deltaTime);


            // mantiene altezza fissa altriemnti se si guarda in basso si comincia a salire in altezza
            Vector3 currentPosition = MixedRealityPlayspace.Transform.position;
            currentPosition.y = altezza; 
            MixedRealityPlayspace.Transform.position = currentPosition;

            deltaMovement = Vector3.zero; // Azzera delta dopo aver applicato il movimento
        }

        if (Mathf.Abs(deltaRotation) > 0.01f)
        {
            // Ruota il MixedRealityPlayspace intorno all'asse Y
            MixedRealityPlayspace.Transform.Rotate(Vector3.up, deltaRotation * Time.deltaTime);
            deltaRotation = 0f;
        }

        if (Mathf.Abs(deltaTranslation) > 0.01f)
        {
            // Esegui una traslazione lungo l'asse Y del MixedRealityPlayspace
            MixedRealityPlayspace.Transform.Translate(Vector3.up * deltaTranslation * Time.deltaTime);

            // aggiorna valore altezza
            Vector3 position = MixedRealityPlayspace.Transform.position;
            altezza = position.y;

            deltaTranslation = 0f;
        }
    }

    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem.RegisterHandler<Movement>(this);
    }

    protected override void UnregisterHandlers()
    {
        CoreServices.InputSystem.UnregisterHandler<Movement>(this);
    }

}
