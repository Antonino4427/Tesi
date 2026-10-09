using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.EventSystems;


public class BuildingManager : MonoBehaviour
{

    public GameObject[] objects;
    public GameObject pendingObject;
    public float rotateAmount;
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public GameObject ob;

    public float gridSize;
    public bool gridOn;
    [SerializeField] private Toggle gridToggle;

    public bool canPlace;
    //[SerializeField] private Material redMaterial;
    //[SerializeField] private Material selectedObjectMaterial;

    private string hitName;

    public Toggle dayToggle;
    public bool dayOn;
    public Material dayMat;
    public Material nightMat;

    public GameObject rotateInstructions;
    public GameObject sceneName;
    int UILayer;


    // Start is called before the first frame update
    void Start()
    {
        gridOn = true;
        canPlace = true;

        hitName = "";
        UILayer = LayerMask.NameToLayer("UI");


    }

    // Update is called once per frame
    void Update()
    {
        if (pendingObject != null) {
            rotateInstructions.SetActive(true);


            if (gridOn) {
                if (hitName != pendingObject.name) pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    pos.y,
                    RoundToNearestGrid(pos.z)
                    );
            }
            else
            {
                if (hitName != pendingObject.name) pendingObject.transform.position = pos;
                
            }
            if (pendingObject.name == "plan")
            {
                if (hitName != pendingObject.name) pendingObject.transform.position = new Vector3(pendingObject.transform.position.x, pendingObject.transform.position.y + 2.2f, pendingObject.transform.position.z);
            }

            if (Input.GetMouseButtonDown(0) && canPlace){
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                RotateObject();
            }
            //UpdateMaterials();
        }
    }

    void FixedUpdate() //migliore per la fisica
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
            hitName = hit.transform.name;
        }
    }

    public void SelectObject(string n)
    {

        /*foreach (GameObject o in objects)
        {
            if (n == o.name)
                ob = o;
        }

        
        pendingObject = Instantiate(ob, pos, Quaternion.Euler(0,0,0));

        //Setta come figlio di BuildingManager
        pendingObject.transform.parent = gameObject.transform;

        pendingObject.name = ob.name; //Rimuove "(Clone)" dal nome delle nuove istanze

        //Preparazione dei componenti dell'oggetto da posizionare
        if (!pendingObject.CompareTag("tile"))
        {
            pendingObject.AddComponent<CheckPlacement>();
            pendingObject.AddComponent<State>();
            pendingObject.transform.GetComponent<Collider>().isTrigger = true;

            if (pendingObject.GetComponent<Rigidbody>() == null) pendingObject.AddComponent<Rigidbody>();
            pendingObject.transform.GetComponent<Rigidbody>().isKinematic = true;
            //selectedObjectMaterial = pendingObject.GetComponent<MeshRenderer>().material;
        }*/
         
    }

    public void PlaceObject() {

       // if (!IsPointerOverUIElement(GetEventSystemRaycastResults())) {
            pendingObject = null;
            rotateInstructions.SetActive(false);
       // }

    }
    /*
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer && curRaysastResult.gameObject == this.gameObject)
                return true;
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    */

    public void RotateObject() {

        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    //Grid System
    public void ToggleGrid() 
    {
        if (gridToggle.isOn)
            gridOn = true;
        else
            gridOn = false;
    }

    public void ToggleDay()
    {
        if (dayToggle.isOn)
            dayOn = false;
        else
            dayOn = true;

        if (dayOn)
        {
            RenderSettings.skybox = dayMat;
        }
        else RenderSettings.skybox = nightMat;
    }

    private float RoundToNearestGrid(float pos) 
    {
        float xDif = pos % gridSize;
        pos -= xDif;
        if (xDif > (gridSize / 2))
            pos += gridSize;
        return pos;
    }

    /* public void UpdateMaterials() 
     {
         if (canPlace)
             pendingObject.GetComponent<MeshRenderer>().material = selectedObjectMaterial;
         else
             pendingObject.GetComponent<MeshRenderer>().material = redMaterial;
     }
    */

    public void LoadScene(string scene)
    {
        Debug.Log("Building Manager: loading scene " + scene);
        sceneName.GetComponent<TMP_InputField>().text = scene;

        using (var reader = new StreamReader(@"Assets\Scripts\saved_stages.csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Split('|')[0] == scene)
                {
                    //istanzia ogni oggetto della linea

                    var objs = line.Split('|');
                    var i = 0;

                    foreach (string toInstantiate in objs)
                    {
                        //Debug.Log(toInstantiate);
                        if (toInstantiate != "" && i != 0)
                        { 
                            var n = toInstantiate.Split(';')[0];
                            var x = float.Parse(toInstantiate.Split(';')[1]);
                            var y = float.Parse(toInstantiate.Split(';')[2]);
                            var z = float.Parse(toInstantiate.Split(';')[3]);
                            var r = float.Parse(toInstantiate.Split(';')[4]);

                            foreach (GameObject o in objects)
                            {
                                if (n == o.name)
                                    ob = o;
                            }
                            pos = new Vector3(x, y, z);
                            var ogg = Instantiate(ob, pos, Quaternion.Euler(0, r, 0));



                            ogg.transform.parent = gameObject.transform;
                            ogg.name = n;

                            if (!ogg.CompareTag("tile"))
                            {
                                //ogg.AddComponent<CheckPlacement>();
                                ogg.AddComponent<State>();
                                ogg.transform.GetComponent<Collider>().isTrigger = true;

                                if (ogg.GetComponent<Rigidbody>() == null) ogg.AddComponent<Rigidbody>();
                                ogg.transform.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                        i++;
                    }
                }

            }
        }
    }
}
