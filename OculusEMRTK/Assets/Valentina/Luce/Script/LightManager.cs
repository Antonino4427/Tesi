using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    public Light _light;
    public GameObject _sphere;
    public GameObject _colorChangeButton;
    public GameObject _colorChangePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _colorChangeButton.transform.position = new Vector3(_colorChangePosition.transform.position.x, _colorChangePosition.transform.position.y, _colorChangePosition.transform.position.z);
        _colorChangeButton.transform.eulerAngles = new Vector3(_colorChangePosition.transform.eulerAngles.x, _colorChangePosition.transform.eulerAngles.y, _colorChangePosition.transform.eulerAngles.z);
        ChangeLightColor();

        
    }

    public void ChangeLightColor()
    {
        _light.color = _sphere.GetComponent<Renderer>().material.color;
    }
}
