using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SwitchControl : MonoBehaviour
{
    public GameObject[] IKControl;
    public GameObject[] FKControl;
    //public GameObject FKControl;


    // Start is called before the first frame update
    void Start()
    {

        //cons = ArmRig.GetComponent<TwoBoneIKConstraint>();


    }
    public void SwitchC()
    {
        //cons.SetActive(!cons.activeSelf);
        //IKControl.SetActive(!IKControl.activeSelf);
        int i;
        for (i = 0;  i <= IKControl.Length-1;  i++)
        {
           IKControl[i].SetActive(!IKControl[i].activeSelf);
        }
        for (i = 0; i <= FKControl.Length - 1; i++)
        {
            FKControl[i].SetActive(!FKControl[i].activeSelf);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Quando il tasto Space viene premuto
        {
            //Console.WriteLine("Spazio premuto!");
            SwitchC();
        }
    }
}
