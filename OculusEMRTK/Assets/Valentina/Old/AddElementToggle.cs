using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddElementToggle : MonoBehaviour
{
    public GameObject _element;
    private GameObject _positionObjectsAndCharacters;
    private GameObject _positionFloor;

    void Start()
    {
        _positionObjectsAndCharacters = GameObject.FindGameObjectWithTag("PositionNewObjectFromScroll");
        _positionFloor = GameObject.FindGameObjectWithTag("PositionNewFloorFromScroll");
    }

    public void InstantiateObjectOrCharacter(bool isOn)
    {
        if (isOn)
        {
            Instantiate(_element, _positionObjectsAndCharacters.transform.position, Quaternion.identity);
        }
    }

    public void InstantiateFloor(bool isOn)
    {
        if (isOn)
        {
            Instantiate(_element, _positionFloor.transform.position, Quaternion.identity);
        }
    }
}
