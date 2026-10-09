using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshSurface navMeshSurface;
    void BakeNavMesh()
    {
        GameObject plane = GameObject.FindGameObjectWithTag("PlaneTable");
        navMeshSurface = plane.GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
    
}
