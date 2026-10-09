using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionDurationScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh = null;
    private float t;

    public void OnSliderUpdated(SliderEventData eventData)
    {
        t = eventData.NewValue * 10;
        int time= Mathf.RoundToInt(t);
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"{time} sec";
        }
    }

    public void Increment()
    {
        
            if (gameObject.GetComponent<PinchSlider>() != null && gameObject.GetComponent<PinchSlider>().SliderValue < 1)
            {
                gameObject.GetComponent<PinchSlider>().SliderValue = gameObject.GetComponent<PinchSlider>().SliderValue + 0.1f;
            }
        
    }

    public void Decrement()
    {
        
            if (gameObject.GetComponent<PinchSlider>() != null && gameObject.GetComponent<PinchSlider>().SliderValue > 0.019f)
            {
                gameObject.GetComponent<PinchSlider>().SliderValue = gameObject.GetComponent<PinchSlider>().SliderValue - 0.1f;
            }
        
    }
}
