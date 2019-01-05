using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBehaviour : MonoBehaviour {

    [SerializeField]
    private Transform _character;
    [SerializeField]
    private Transform[] _waypoints;
    [SerializeField]
    private Transform _lookAtPosition;

    //field of view
    [SerializeField]
    private Transform _startPos;
    [SerializeField]
    private float _fieldOfView = 2.0f;
    [SerializeField]
    private float _fieldOfViewDistance = 15.0f;


    //other
    private INode _rootNode;
    private NavMeshAgent _npc;
    private Animator _animator;

    private AnimationController _ac;
    
    private int _destPoint;

    //stay for x time at a point
    private float _timer;
    private float _standDefault = 10.0f;
    

    // Use this for initialization
    void Start () {
        _npc = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
        _ac = new AnimationController(_animator);
        _timer = _standDefault;
        _destPoint = Random.Range(0, _waypoints.Length - 1);
        
        SetupRoot();

        StartCoroutine(RunTree());
    }
	
	// Update is called once per frame
	void Update () {
        NPCFieldOfView();
    }

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
                //LookForCharacter(),
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
        //Debug.Log("npc action");
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
        return true; // go to focus
    }

    private bool IsCloseToCharacter()
    {
        //return true;
        return Random.Range(0, 1) > 0.3;
    }

    private bool IsNpcAtPoint()
    {
        if (_npc.remainingDistance < 0.1f)
        {
            _npc.transform.position = _waypoints[_destPoint].position;
            _timer -= Time.deltaTime;
            _ac.WalkAnimation(false);
            _ac.LookAroundAnimation(true);

            if (_timer <= 5.0f)
            {
                //_timer = _standDefault;
                //destPoint = (destPoint + 1) % _waypoints.Length;
                //int curDestPoint = destPoint;
                //destPoint = Random.Range(0, _waypoints.Length - 1);
                //if (curDestPoint == destPoint)
                //{
                //    destPoint = Random.Range(0, _waypoints.Length - 1);
                //}
               
                return false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsTimerRunning()
    {
        if (_timer <= 0f)
        {
            _timer = _standDefault;
            _destPoint = (_destPoint + 1) % _waypoints.Length;

            return false;
        }
        else if(_npc.remainingDistance < 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private bool IsTimerReset()
    {
        if (_npc.pathEndPosition.x != _waypoints[_destPoint].position.x && _timer == _standDefault) 
        {
            return true;
        }

        return false;
    }

    //Action for the actionNode
    private IEnumerator<NodeResult> Focus()
    {
        Debug.Log("Focus");
        yield return NodeResult.Success; //go to walk to character
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
        //Debug.Log("Stare add animation that rotate");
        //rotate the npc so he idle with his face to the playfield and not agains the wall
        if (_destPoint == 0 || _destPoint == 4 || _destPoint == 6)
        {
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, _lookAtPosition.position - this.transform.position, 2f * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> Idle()
    {
        //Debug.Log("Idle");
        _ac.LookAroundAnimation(false);
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> WalkToNextPoint()
    {
        //Debug.Log("walk to next point");
        _ac.WalkAnimation(true);
        _npc.destination = _waypoints[_destPoint].position;

        yield return NodeResult.Success;
    }

    private void NPCFieldOfView()
    {
        Debug.DrawRay(_startPos.position, _startPos.forward * _fieldOfViewDistance, Color.white);
        Debug.DrawRay(_startPos.position, (_startPos.forward + _startPos.right).normalized * _fieldOfViewDistance, Color.black);
        Debug.DrawRay(_startPos.position, (_startPos.forward - _startPos.right).normalized * _fieldOfViewDistance, Color.black);

    }
}
