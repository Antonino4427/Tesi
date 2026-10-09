using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlendShapeDriver
{
    public string name; // solo per leggibilità nell'inspector
    public Transform driverTransform;
    public int blendShapeIndex;
    public Vector3 axis; // Esempio: (0,1,0) per movimento verticale (Y)
    [HideInInspector] public Vector3 lastLocalPosition;
}

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class ControlBlendShapes1 : MonoBehaviour
{
    public Transform parent;
    public int driverMultiplier = 2000;
    public List<BlendShapeDriver> blendShapeDrivers;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    void Start()
    {
        foreach (var driver in blendShapeDrivers)
        {
            driver.lastLocalPosition = parent.InverseTransformPoint(driver.driverTransform.position);
        }
    }

    void Update()
    {
        foreach (var driver in blendShapeDrivers)
        {
            Vector3 currentLocalPos = parent.InverseTransformPoint(driver.driverTransform.position);
            Vector3 delta = currentLocalPos - driver.lastLocalPosition;
            float movement = Vector3.Dot(delta, driver.axis);

            if (movement > 0f) //  Solo movimenti nella direzione desiderata
            {
                float weight = Mathf.Clamp(driverMultiplier * movement, 0f, 100f);
                skinnedMeshRenderer.SetBlendShapeWeight(driver.blendShapeIndex, weight);
            }
            else
            {
                Approximate(driver.blendShapeIndex);
            }

            // Optional: aggiorna solo se vuoi che si "resetti" il punto base
            // driver.lastLocalPosition = currentLocalPos;
        }
    }

    void Approximate(int blendShapeIndex)
    {
        float currentWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float correctedWeight = (Mathf.Abs(currentWeight - 0f) < Mathf.Abs(currentWeight - 100f)) ? 0f : 100f;
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, correctedWeight);
    }
}
