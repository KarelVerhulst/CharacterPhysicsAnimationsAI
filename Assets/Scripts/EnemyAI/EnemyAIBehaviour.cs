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
    private float _walkingspeed = (5.0f * 1000) / (60 * 60); //https://en.wikipedia.org/wiki/Preferred_walking_speed

    //stay for x time at a point
    private float _timer;
    private float _standDefault = 10.0f;

    //test
    public float maxAngle;
    public float maxRadius;

    private bool isInFov = false;

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
       // isInFov = inFOV(transform, _character, maxAngle, maxRadius);
        
        //NPCFieldOfView();
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
        //Debug.Log("npc action");
        return new SequenceNode(
                new ConditionNode(IsTimerReset),
                new ActionNode(WalkToNextPoint)
            );
    }

    // Selectors
    private INode NPCActionWithCharacter()
    {
        //Debug.Log("NPCActionWithCharacter");
        return new SelectorNode(
                FightCharacter(),
                new ActionNode(WalkToCharacter)
            );
    }

    // Conditions
    private bool IsCharacterInRange()
    {
       
        isInFov = inFOV(transform, _character, maxAngle, maxRadius);

        if (isInFov)
        {
            //Debug.Log("focus on character");
            //Debug.Log("IsCharacterInRange TRUE");
            return true;
        }
        else
        {
            _ac.FightAnimation(false);
            //Debug.Log("go to next node");
           // Debug.Log("IsCharacterInRange FALSE");
            return false;
        }
    }

    private bool IsCloseToCharacter()
    {
        
        if (Vector3.Distance(this.transform.position, _character.position) < 1.3f)
        {
           // Debug.Log("isCloseTocharacter TRUE");
            return true;
        }
        else
        {
            //Debug.Log("isCloseTocharacter FALSE");
            return false;
        }
    }

    private bool IsNpcAtPoint()
    {
        
        if (_npc.remainingDistance < 0.1f)
        {
            //Debug.Log("NPCisAtPoint");
            _npc.transform.position = _waypoints[_destPoint].position;
            _timer -= Time.deltaTime;
            _ac.WalkAnimation(false);
            _ac.LookAroundAnimation(true);

            if (_timer <= 5.0f)
            {
                //Debug.Log("isNPC at point TIMER < 5 FALSE");
                return false;
            }
           // Debug.Log("isNPC at point");
            return true; //stile stare
        }
        else
        {
            //Debug.Log("isNPC at point REMAININGDISTANCE < 0.1f FALSE");
            return false;
        }
    }

    private bool IsTimerRunning()
    {
        
        if (_timer <= 0f)
        {
            _timer = _standDefault;
            _destPoint = (_destPoint + 1) % _waypoints.Length;
            //Debug.Log("Is timer running TIMER < 5 FALSE");
            return false;
        }
        else if(_npc.remainingDistance < 0.1f)
        {
            //Debug.Log("Is timer running TRUE");
            return true; //still idle
        }
        else
        {
            //Debug.Log("Is timer running ELSE FALSE");
            return false;
        }
        
    }

    private bool IsTimerReset()
    {
        Debug.Log("npc != waypointsposition | " + (_npc.pathEndPosition.x != _waypoints[_destPoint].position.x));
        Debug.Log("_timer == standdefault | " + (_timer == _standDefault));
        Debug.Log(_timer);

        if (_npc.pathEndPosition.x != _waypoints[_destPoint].position.x) //&& _timer == _standDefault 
        {
           // Debug.Log("istimerReset TRUE");
            return true;
        }
        //Debug.Log("istimerReset FALSE");
        return false;
    }

    //Action for the actionNode
    private IEnumerator<NodeResult> Focus()
    {
        //Debug.Log("Focus");

        //_npc.speed = 0;
        //_ac.WalkAnimation(false);
        //_ac.LookAroundAnimation(true);

        //Vector3 characterDirection = Vector3.RotateTowards(this.transform.forward, _character.position - this.transform.position, 2f * Time.deltaTime, 0.0f);
        //this.transform.rotation = Quaternion.LookRotation(characterDirection);

        yield return NodeResult.Success; //go to walk to character
    }

    private IEnumerator<NodeResult> WalkToCharacter()
    {
        //Debug.Log("Walk to character");
        _ac.WalkAnimation(true);
        _ac.FightAnimation(false);
        _npc.destination = _character.position;
        _npc.speed = _walkingspeed;

        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> Fight()
    {
      // Debug.Log("Fight");
        _ac.WalkAnimation(false);
        _ac.FightAnimation(true);
        _npc.speed = 0;
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
      //  Debug.Log("Idle");
        _ac.LookAroundAnimation(false);
        yield return NodeResult.Success;
    }

    private IEnumerator<NodeResult> WalkToNextPoint()
    {
        //Debug.Log("walk to next point");
        _ac.WalkAnimation(true);
        _npc.speed = _walkingspeed;
        _npc.destination = _waypoints[_destPoint].position;

        yield return NodeResult.Success;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFov)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, (_character.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);


    }

    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        // https://www.youtube.com/watch?v=BJPSiWNZVow
        Collider[] overlaps = new Collider[20];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    /* 
                     * info about Direction and Distance from One Object to Another
                     * https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
                     */
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);
                   // Debug.Log(angle);
                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            //Debug.Log("hit  " + hit.transform.tag);
                            //Debug.Log("target  " + target.tag);
                            if (hit.transform.tag == target.tag)
                                return true;

                        }
                    }
                }
            }
        }

        return false;
    }
}
