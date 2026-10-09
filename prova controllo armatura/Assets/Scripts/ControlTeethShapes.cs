using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlTeethShapes : MonoBehaviour
{
    public GameObject TeethDriver, parent;
    
    int DriverMultiplier;
    Transform TeethTransform;
    Quaternion TeethRot,TeethLastRotation;
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
        TeethTransform = TeethDriver.transform; //inizializzo le transform dei driver
        TeethRot = TeethTransform.localRotation; //ottengo la posizione locale
        TeethLastRotation = TeethRot;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
           
            //TeethRot = parent.transform.InverseTransformPoint(TeethTransform.position); //usando inverseTransformPOint ricavo le locali dal driver perche quando
                                                                                    //lo grabbo la transform diventa automaitcamente quella globale boh vabbe
            Debug.Log("rotazione: "+(Quaternion.Inverse(parent.transform.rotation) * TeethTransform.rotation).z);
            Debug.Log("ULTIMA rotazione: " + TeethLastRotation.z);

            //Debug.Log("BLEDN SHAPE: " + skinnedMeshRenderer.GetBlendShapeWeight(0));


        }
        TeethControl();


    }
    void TeethControl()
    {
        if (skinnedMeshRenderer.GetBlendShapeWeight(0) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(0) >= 0f
            && (Quaternion.Inverse(parent.transform.rotation) * TeethTransform.rotation).z >= TeethLastRotation.z && (Quaternion.Inverse(parent.transform.rotation) * TeethTransform.rotation).z <= TeethLastRotation.z + 100f/DriverMultiplier)
        {
            //Debug.Log("DEBUG AAAAAAAAAAAAAAAAAAAAAAAAAAA");
            TeethRot = Quaternion.Inverse(parent.transform.rotation) * TeethTransform.rotation;
            skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Abs(DriverMultiplier * (TeethRot.z - TeethLastRotation.z)));
            //MouthLastPosition = parent.transform.InverseTransformPoint(MouthTransform.position);
            

        }
        else //questo else gigante serve per impostare il valore del blend shape automaticamente al valore piu vicino nei limit 0-100
        {
            Approximate(0);
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
