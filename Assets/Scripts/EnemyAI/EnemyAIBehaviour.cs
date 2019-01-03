using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBehaviour : MonoBehaviour {

    [SerializeField]
    private Transform[] _waypoints;
    
    private INode _rootNode;
    private NavMeshAgent _npc;
    private Animator _animator;

    private float _timer;
    private int destPoint;

    // Use this for initialization
    void Start () {
        _npc = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();

        destPoint = Random.Range(0, _waypoints.Length - 1);
        _timer = 5;

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

    //root selector
    private void SetupRoot()
    {
        _rootNode = new SelectorNode(
                LookForCharacter(),
                StareAtPoint(),
                IdleAtPoint(),
                NPCAction()
            );
    }

    // Sequences
    private INode LookForCharacter()
    {
        return new SequenceNode(
                new ConditionNode(IsCharacterInRange),
                new ActionNode(Focus),
                NPCActionWithCharacter()
            );
    }

    private INode FightCharacter()
    {
        return new SequenceNode(
                new ConditionNode(IsCloseToCharacter),
                new ActionNode(Fight)
            );
    }
    
    private INode StareAtPoint()
    {
        return new SequenceNode(
                new ConditionNode(IsNpcAtPoint),
                new ActionNode(Stare)
            );
    }

    private INode IdleAtPoint()
    {
        return new SequenceNode(
                new ConditionNode(IsTimerRunning),
                new ActionNode(Idle)
            );
    }

    private INode NPCAction()
    {
        Debug.Log("npc action");
        return new SequenceNode(
                new ConditionNode(IsTimerReset),
                new ActionNode(WalkToNextPoint)
            );
    }

    // Selectors
    private INode NPCActionWithCharacter()
    {
        return new SelectorNode(
                FightCharacter(),
                new ActionNode(WalkToCharacter)
            );
    }

    // Conditions
    private bool IsCharacterInRange()
    {
        //return true;
        return Random.Range(0, 1) > 0.3;
    }

    private bool IsCloseToCharacter()
    {
        //return true;
        return Random.Range(0, 1) > 0.3;
    }

    private bool IsNpcAtPoint()
    {
        //return true;
        return Random.Range(0, 1) > 0.3;
    }

    private bool IsTimerRunning()
    {
        //return true;
        return Random.Range(0, 1) > 0.3;
    }

    private bool IsTimerReset()
    {
        _timer -= Time.deltaTime;
       
        //return true;
        if (_timer <= 0)
        {
            _timer = 5;
            return true;
        }

        return false;
    }

    //Action for the actionNode
    private IEnumerator<NodeResult> Focus()
    {
        Debug.Log("Focus");
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> WalkToCharacter()
    {
        Debug.Log("Walk to character");
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> Fight()
    {
        Debug.Log("Fight");
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> Stare()
    {
        Debug.Log("Stare");
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> Idle()
    {
        Debug.Log("Idle");
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> WalkToNextPoint()
    {
        Debug.Log("WalkToNextPoint");
        GotoNextPoint();


        yield return NodeResult.Success;
    }

    
    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (_waypoints.Length == 0)
            return;
        
        // Set the agent to go to the currently selected destination.
        _npc.destination = _waypoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % _waypoints.Length;
    }
}
