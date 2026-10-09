using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

// script per stampare valori focale vicino allo slider
public class ShowFocalLengthValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh = null;
    private float t;

    private void Start()
    {
        t = 85;
    }

    public void OnSliderUpdated(SliderEventData eventData)
    {

        if (eventData.NewValue == 0)
        {
            t = 14;
        }

         else if (eventData.NewValue == 0.125)
        {
            t = 24;
        }

        else if (eventData.NewValue == 0.25)
        {
            t = 35;
        }
        else if (eventData.NewValue == 0.375)
        {
            t = 50;
        }
        else if (eventData.NewValue == 0.5)
        {
            t = 85;
        }
        else if (eventData.NewValue == 0.625)
        {
            t = 105;
        }
        else if (eventData.NewValue == 0.75)
        {
            t = 135;
        }
        else if (eventData.NewValue == 0.875)
        {
            t = 200;
        }
        else if (eventData.NewValue == 1)
        {
            t = 400;
        }



        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"{t} mm";
        }
    }

    public float getValue()
    {
        return t;
    }

    public void setValue(float value)
    {
        if (value == 0)
        {
            t = 14;
        }

        else if (value == 0.125)
        {
            t = 24;
        }

        else if (value == 0.25)
        {
            t = 35;
        }
        else if (value == 0.375)
        {
            t = 50;
        }
        else if (value == 0.5)
        {
            t = 85;
        }
        else if (value == 0.625)
        {
            t = 105;
        }
        else if (value == 0.75)
        {
            t = 135;
        }
        else if (value == 0.875)
        {
            t = 200;
        }
        else if (value == 1)
        {
            t = 400;
        }




        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<PinchSlider>() != null)
            {
                child.GetComponent<PinchSlider>().SliderValue = value;
            }
        }
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"{t} mm";
        }
    }

    public void Increment()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<PinchSlider>() != null && child.GetComponent<PinchSlider>().SliderValue != 1)
            {
                child.GetComponent<PinchSlider>().SliderValue = child.GetComponent<PinchSlider>().SliderValue + 0.125f;
            }
        }
    }

    public void Decrement()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<PinchSlider>() != null && child.GetComponent<PinchSlider>().SliderValue != 0)
            {
                child.GetComponent<PinchSlider>().SliderValue = child.GetComponent<PinchSlider>().SliderValue - 0.125f;
            }
        }
    }

    public void setValueInverso(float value)
    {
        if (value == 14)
        {
            t = 0f;
        }

        else if (value == 24)
        {
            t = 0.125f;
        }

        else if (value == 35)
        {
            t = 0.25f;
        }
        else if (value == 50)
        {
            t = 0.375f;
        }
        else if (value == 85)
        {
            t = 0.5f;
        }
        else if (value == 105)
        {
            t = 0.625f;
        }
        else if (value == 135)
        {
            t = 0.75f;
        }
        else if (value == 200)
        {
            t = 0.875f;
        }
        else if (value == 400)
        {
            t = 1;
        }


        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<PinchSlider>() != null)
            {
                child.GetComponent<PinchSlider>().SliderValue = t;
            }
        }
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"{value} mm";
        }
    }
}

