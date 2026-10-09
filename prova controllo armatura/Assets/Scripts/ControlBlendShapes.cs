using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlBlendShapes : MonoBehaviour
{
    public GameObject MouthDriver,LeftLipDriver, RightLipDriver,
           LeftEyebrowDriver,RightEyebrowDriver, parent;
    
    int DriverMultiplier;
    Transform MouthTransform,LLipTransform,RLipTransform, LEyebrowTransform, REyebrowTransform;
    Vector3 MouthPos,MouthLastPosition,LLipPos,RLipPos, LLipLastPosition, RLipLastPosition
            ,LEyebrowPosition,LEyebrowLastPosition,REyebrowPosition,REyebrowLastPosition;
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
        DriverMultiplier = 2000;
        //blendShapeCount = skinnedMesh.blendShapeCount;
        MouthTransform = MouthDriver.transform; LLipTransform = LeftLipDriver.transform; RLipTransform = RightLipDriver.transform;
        LEyebrowTransform = LeftEyebrowDriver.transform; REyebrowTransform = RightEyebrowDriver.transform; //inizializzo le transform dei driver
        MouthPos = MouthTransform.localPosition; LLipPos = LLipTransform.localPosition; RLipPos = RLipTransform.localPosition; 
        LEyebrowPosition = LEyebrowTransform.localPosition; REyebrowPosition = REyebrowTransform.localPosition; //ottengo la posizione locale
        MouthLastPosition = MouthPos; LLipLastPosition = LLipPos; RLipLastPosition = RLipPos;
        LEyebrowLastPosition = LEyebrowPosition; REyebrowLastPosition = REyebrowPosition; // assegno alle variabili lastPosition
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
           
            MouthPos = parent.transform.InverseTransformPoint(MouthTransform.position); //usando inverseTransformPOint ricavo le locali dal driver perche quando
                                                                                    //lo grabbo la transform diventa automaitcamente quella globale boh vabbe
            Debug.Log("ULTIMA POSIZIONE: " + LLipLastPosition.x);
            //Debug.Log("BLEDN SHAPE: " + skinnedMeshRenderer.GetBlendShapeWeight(0));


        }
        MouthControl();
        LipsControl();
        EyebrowControl();


    }
    void MouthControl() 
    {
        if (skinnedMeshRenderer.GetBlendShapeWeight(0) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(0) >= 0f
            && parent.transform.InverseTransformPoint(MouthTransform.position).y >= MouthLastPosition.y && parent.transform.InverseTransformPoint(MouthTransform.position).y <= MouthLastPosition.y+100f/DriverMultiplier)
        {
            MouthPos = parent.transform.InverseTransformPoint(MouthTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Abs(DriverMultiplier * (MouthPos.y- MouthLastPosition.y)));
            //MouthLastPosition = parent.transform.InverseTransformPoint(MouthTransform.position);
            //Debug.Log("DEBUG AAAAAAAAAAAAAAAAAAAAAAAAAAA");

        }
        else //questo else gigante serve per impostare il valore del blend shape automaticamente al valore piu vicino nei limit 0-100
        {
            Approximate(0);
        }
    }

    void LipsControl()
    {
        if (//parent.transform.InverseTransformPoint(LLipTransform.position) != LLipLastPosition && //cosi se il driver non è stato spostato affatto non entra nell if
             skinnedMeshRenderer.GetBlendShapeWeight(3) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(3) >= 0f
            && parent.transform.InverseTransformPoint(LLipTransform.position).y >= LLipLastPosition.y && parent.transform.InverseTransformPoint(LLipTransform.position).y <= LLipLastPosition.y+100f / DriverMultiplier)
        {
            LLipPos = parent.transform.InverseTransformPoint(LLipTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(3, Mathf.Abs(DriverMultiplier * (LLipPos.y - LLipLastPosition.y))); 
            //LLipLastPosition = parent.transform.InverseTransformPoint(LLipTransform.position);

        }
        else 
        {
            Approximate(3);
        }
         //sempre controllo del labbro pero nel movimento laterale
        if (skinnedMeshRenderer.GetBlendShapeWeight(1) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(1) >= 0f
            && parent.transform.InverseTransformPoint(LLipTransform.position).x <= LLipLastPosition.x && parent.transform.InverseTransformPoint(LLipTransform.position).x >= LLipLastPosition.x-100f/DriverMultiplier)
        {
            LLipPos = parent.transform.InverseTransformPoint(LLipTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Abs(DriverMultiplier * (LLipPos.x-LLipLastPosition.x))); 

        }
        else 
        {
            Approximate(1);
        }
        //  QUI PER IL CONTROLLO DEL LATO DESTRO DELLE LABBRA

        if (skinnedMeshRenderer.GetBlendShapeWeight(4) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(4) >= 0f
            && parent.transform.InverseTransformPoint(RLipTransform.position).y >= 0f && parent.transform.InverseTransformPoint(RLipTransform.position).y <= 100f / DriverMultiplier)
        {
            RLipPos = parent.transform.InverseTransformPoint(RLipTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(4, DriverMultiplier * RLipPos.y); //<=100 ? DriverMultiplier * LLipPos.y : 100f
            //LLipLastPosition = parent.transform.InverseTransformPoint(LLipTransform.position);

        }
        else
        {
            Approximate(4);
        }
        //sempre controllo del labbro pero nel movimento laterale
        if (skinnedMeshRenderer.GetBlendShapeWeight(2) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(2) >= 0f
            && parent.transform.InverseTransformPoint(RLipTransform.position).x >= RLipLastPosition.x && parent.transform.InverseTransformPoint(RLipTransform.position).x <= RLipLastPosition.x + 100f / DriverMultiplier)
        {
            RLipPos = parent.transform.InverseTransformPoint(RLipTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(2, Mathf.Abs(DriverMultiplier * (RLipPos.x - RLipLastPosition.x)));

        }
        else
        {
            Approximate(2);
        }
    }

    void EyebrowControl()
    {
        if (//parent.transform.InverseTransformPoint(LLipTransform.position) != LLipLastPosition && //cosi se il driver non è stato spostato affatto non entra nell if
            skinnedMeshRenderer.GetBlendShapeWeight(5) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(5) >= 0f
           && parent.transform.InverseTransformPoint(LEyebrowTransform.position).y >= LEyebrowLastPosition.y && parent.transform.InverseTransformPoint(LEyebrowTransform.position).y <= LEyebrowLastPosition.y + 100f / DriverMultiplier)
        {
            LEyebrowPosition = parent.transform.InverseTransformPoint(LEyebrowTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(5, Mathf.Abs(DriverMultiplier * (LEyebrowPosition.y - LEyebrowLastPosition.y)));
            //LLipLastPosition = parent.transform.InverseTransformPoint(LLipTransform.position);

        }
        else
        {
            Approximate(5);
        }

        if (//parent.transform.InverseTransformPoint(LLipTransform.position) != LLipLastPosition && //cosi se il driver non è stato spostato affatto non entra nell if
           skinnedMeshRenderer.GetBlendShapeWeight(6) <= 100f && skinnedMeshRenderer.GetBlendShapeWeight(6) >= 0f
          && parent.transform.InverseTransformPoint(REyebrowTransform.position).y >= REyebrowLastPosition.y && parent.transform.InverseTransformPoint(REyebrowTransform.position).y <= REyebrowLastPosition.y + 100f / DriverMultiplier)
        {
            REyebrowPosition = parent.transform.InverseTransformPoint(REyebrowTransform.position);
            skinnedMeshRenderer.SetBlendShapeWeight(6, Mathf.Abs(DriverMultiplier * (REyebrowPosition.y - REyebrowLastPosition.y)));
            //LLipLastPosition = parent.transform.InverseTransformPoint(LLipTransform.position);

        }
        else
        {
            Approximate(6);
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
