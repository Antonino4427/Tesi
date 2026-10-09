using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlBlinkShapes : MonoBehaviour
{
    public GameObject LeftEyeDriver,RightEyeDriver, parent;
    
    int DriverMultiplier;
    Transform LeftEyeTransform,RightEyeTransform;
    Quaternion LEyeRot,LEyeLastRotation, REyeRot, REyeLastRotation;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    //float blendOne = 0f;
    //float blendTwo = 0f;
    //float blendSpeed = 1f;
    //bool blendOneFinished = false;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    void Start()
    {
        DriverMultiplier = 500;
        //blendShapeCount = skinnedMesh.blendShapeCount;
        LeftEyeTransform = LeftEyeDriver.transform; //inizializzo le transform dei driver
        LEyeRot = LeftEyeTransform.localRotation; //ottengo la posizione locale
        LEyeLastRotation = LEyeRot;

        RightEyeTransform = RightEyeDriver.transform; // inizializzo la transform del driver per l'occhio destro
        REyeRot = RightEyeTransform.localRotation; // ottengo la rotazione locale dell'occhio destro
        REyeLastRotation = REyeRot; 
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
           
            //TeethRot = parent.transform.InverseTransformPoint(TeethTransform.position); //usando inverseTransformPOint ricavo le locali dal driver perche quando
                                                                                    //lo grabbo la transform diventa automaitcamente quella globale boh vabbe
            Debug.Log("rotazione: "+(Quaternion.Inverse(parent.transform.rotation) * LeftEyeTransform.rotation));
            Debug.Log("ULTIMA rotazione: " + LEyeLastRotation);

            //Debug.Log("BLEDN SHAPE: " + skinnedMeshRenderer.GetBlendShapeWeight(0));


        }
        EyesControl();


    }
    void EyesControl()
    {
        if (skinnedMeshRenderer.GetBlendShapeWeight(1) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(1) >= 0f
            && (Quaternion.Inverse(parent.transform.rotation) * LeftEyeTransform.rotation).z >= LEyeLastRotation.z && (Quaternion.Inverse(parent.transform.rotation) * LeftEyeTransform.rotation).z <= LEyeLastRotation.z + 100f/DriverMultiplier)
        {
            //Debug.Log("DEBUG AAAAAAAAAAAAAAAAAAAAAAAAAAA");
            LEyeRot = Quaternion.Inverse(parent.transform.rotation) * LeftEyeTransform.rotation;
            skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Abs(DriverMultiplier * (LEyeRot.z - LEyeLastRotation.z)));
            //MouthLastPosition = parent.transform.InverseTransformPoint(MouthTransform.position);
        }
        else //questo else gigante serve per impostare il valore del blend shape automaticamente al valore piu vicino nei limit 0-100
        {
            Approximate(1);
        }

        if (skinnedMeshRenderer.GetBlendShapeWeight(2) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(2) >= 0f
            && (Quaternion.Inverse(parent.transform.rotation) * RightEyeTransform.rotation).z >= REyeLastRotation.z && (Quaternion.Inverse(parent.transform.rotation) * RightEyeTransform.rotation).z <= REyeLastRotation.z + 100f / DriverMultiplier)
        {
            //Debug.Log("DEBUG AAAAAAAAAAAAAAAAAAAAAAAAAAA");
            REyeRot = Quaternion.Inverse(parent.transform.rotation) * RightEyeTransform.rotation;
            skinnedMeshRenderer.SetBlendShapeWeight(2, Mathf.Abs(DriverMultiplier * (REyeRot.z - REyeLastRotation.z)));
            //MouthLastPosition = parent.transform.InverseTransformPoint(MouthTransform.position);


        }
        else 
        {
            Approximate(2);
        }
    }


    void Approximate(int blendshape) //questo  serve per impostare il valore del blend shape automaticamente al valore piu vicino nei limit 0-100
    {
        float currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendshape);

        // Trova il valore più vicino tra 0 e 100
        float correctedWeight = (Mathf.Abs(currentWeight - 0f) < Mathf.Abs(currentWeight - 100f)) ? 0f : 100f;

        skinnedMeshRenderer.SetBlendShapeWeight(blendshape, correctedWeight);
    }
}
