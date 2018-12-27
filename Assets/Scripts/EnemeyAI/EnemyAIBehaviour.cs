using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBehaviour : MonoBehaviour {

    private INode _rootNode;

	// Use this for initialization
	void Start () {
       // Debug.Log("Start");
        SetupRoot();

        StartCoroutine(RunTree());
	}
	
	// Update is called once per frame
	void Update () {}

    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }

    private void SetupRoot()
    {
        //INode respawnAI = new SequenceNode();

        //INode fieldToViewAI = new SequenceNode();

        //INode walkToCharacterAI = new SequenceNode();

        //ActionNode walkAI = new ActionNode(Walk);

        //_rootNode = new SelectorNode(
        //        respawnAI,
        //        fieldToViewAI,
        //        walkToCharacterAI,
        //        walkAI
        //    );
        
        _rootNode = new SelectorNode(
                RespawnAI(),
                FieldToViewAI(),
                WalkToCharacterAI(),
                new ActionNode(Walk)
            );
    }

    // Sequences
    private INode RespawnAI()
    {
        return new SequenceNode(
                new ConditionNode(IsHealthZero),
                new ActionNode(RespawnNPC)
            );
    }

    private INode FieldToViewAI()
    {
        /*
         * tree updaten 
         */
        return new SequenceNode();
    }

    private INode WalkToCharacterAI()
    {
        /*
         * tree updaten
         */
        return new SequenceNode();
    }
    
    // Conditions
    private bool IsHealthZero()
    {
        /*
         * todo
         *      check if healht is zero
         *      return true or false
        */
        bool isHealthZero = Random.Range(0, 1.0f) <= 0.3;

        Debug.Log("isHealthZero: " + isHealthZero);
        return isHealthZero;
    }

    //Action for the actionNode
    IEnumerator<NodeResult> Walk()
    {
        /*
            todo
                implent enemy walk (locomotion)
         */

        Debug.Log("WALK");

        yield return NodeResult.Success; //always succes so the enemy walk around in the level
    }

    IEnumerator<NodeResult> RespawnNPC()
    {
        /*
         * todo
         *  respawn the npc to a point
         */

        Debug.Log("RESPAWN   NPC");

        yield return NodeResult.Success; // or NodeResult.False ????? kan dit false zijn ???
    }


}
