using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNavMesh : MonoBehaviour {

    [SerializeField]
    private NavMeshSurface[] _surfaces;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < _surfaces.Length; i++)
            {
                _surfaces[i].BuildNavMesh();
            }
        }
    }
}
