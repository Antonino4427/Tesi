using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScene : MonoBehaviour
{
    public GameObject menu;
    public GameObject luce;

    public GameObject mixedRealityPlayspace;

    public Transform left;

    public Transform right;



    public void Start()
    {


        // i controller non devono proiettare ombre
        left = mixedRealityPlayspace.transform.Find("MRTK-Quest_OVRCameraRig(Clone)").transform.Find("TrackingSpace").transform.Find("LeftHandAnchor").transform.Find("LeftControllerAnchor").transform.Find("OVRControllerPrefab").transform.Find("left_touch_controller_model_skel").transform.Find("left_touch_mesh");
        left.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        right = mixedRealityPlayspace.transform.Find("MRTK-Quest_OVRCameraRig(Clone)").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").transform.Find("RightControllerAnchor").transform.Find("OVRControllerPrefab").transform.Find("right_touch_controller_model_skel").transform.Find("right_touch_mesh");
        right.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

   
    


    public void LoadCreateStoryboard()
    {
        StartCoroutine(LoadYourAsyncScene());
        //SceneManager.LoadScene(1);
        
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        menu.SetActive(false);
        luce.SetActive(false);
    }

    public void UnloadTutorial()
    {
        SceneManager.UnloadSceneAsync(2);
        //menu.SetActive(true);
        //luce.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        menu.SetActive(false);
        luce.SetActive(false);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
