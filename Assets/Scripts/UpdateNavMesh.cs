using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNavMesh : MonoBehaviour {

    /*
     * if you want to use this script you have to use the navigation-static on the objects that not moves
     * https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html
     */
    [SerializeField]
    private NavMeshSurface[] _surfaces;

    private float _seconds = 2f;
    private int _playerLayer = 9;
    private bool _isPlayerInAIArea = false;
    

    // Use this for initialization
    void Start () {
        /*
         * after x seconds you update the nav mesh
         * the mesh will only update if the character is in the area
         * 
         * to do
         *  only update when the boxes are moving
         */
        StartCoroutine(UpdateNavMeshAfterXSeconds());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator UpdateNavMeshAfterXSeconds()
    {
        while (true)
        {
            if (_isPlayerInAIArea)
            {
                for (int i = 0; i < _surfaces.Length; i++)
                {
                    _surfaces[i].BuildNavMesh();
                }
            }
            yield return new WaitForSeconds(_seconds);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _isPlayerInAIArea = true;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _isPlayerInAIArea = false;
        }
    }
}
