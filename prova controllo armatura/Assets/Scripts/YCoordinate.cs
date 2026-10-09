using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YCoordinate : MonoBehaviour
{
    public float yValue => transform.localEulerAngles.y; // Property to get the Y coordinate of the GameObject
}
