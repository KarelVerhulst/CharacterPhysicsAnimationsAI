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
    [SerializeField]
    private HUD _hudHealth;
    [SerializeField]
    private BoxCollider _swordCollider;
    [SerializeField]
    private bool _isSecondNPC;
    
    private INode _rootNode;
    private int _destPoint;
    private float _walkingspeed = (5.0f * 1000) / (60 * 60); //https://en.wikipedia.org/wiki/Preferred_walking_speed

    //stay for x time at a point
    private float _timer;
    private float _standDefault = 10.0f;
    
    //field of view
    public float maxAngle;
    public float maxRadius;
    private bool isInFov = false;
    private bool _isCharacterHittingOnMe = false;

    private float _deathTimer = 0;
    private bool _isDeath = false;

    //extern script
    private NavMeshAgent _npc;
    private Animator _animator;
    private AnimationController _ac;

    // Use this for initialization
    void Start () {
        _npc = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
        _ac = new AnimationController(_animator);
        _timer = _standDefault;
        
        SetupStartPositionNPC();

        SetupRoot();

        StartCoroutine(RunTree());
    }
    
    // Update is called once per frame
    void Update () {
        ApplyDeath();
        
        //if the character is dying you can stop fighting with it and move on
        if (_character.gameObject.GetComponent<CharacterBehaviour>().IsDead)
        {
            _isCharacterHittingOnMe = false;
        }
    }
    
    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            if (!_isDeath)
            {
                yield return _rootNode.Tick();
            }
            else
            {
                yield return NodeResult.Success;
            }
            
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

        if (isInFov && !_character.gameObject.GetComponent<CharacterBehaviour>().IsDead)
        {
            return true;
        }
        else
        {
            if (_isCharacterHittingOnMe)
            {
                Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, _character.position - this.transform.position, 1f, 0.0f);
                this.transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                _isCharacterHittingOnMe = false;
            }
            
            _ac.FightAnimation(false);
        
            return false;
        }
    }

    private bool IsCloseToCharacter()
    {
        if (Vector3.Distance(this.transform.position, _character.position) < 1.3f)
        {
            return true;
        }
        else
        {
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
                return false;
            }

            return true; //stile stare
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
            return true; //still idle
        }
        else
        {
            return false;
        }
        
    }

    private bool IsTimerReset()
    {
        if (_npc.pathEndPosition.x != _waypoints[_destPoint].position.x) //&& _timer == _standDefault 
        {
            return true;
        }
        
        return false;
    }

    //Action for the actionNode
    private IEnumerator<NodeResult> Focus()
    {
        //Debug.Log("Focus");
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
        if (_destPoint == 0 || _destPoint == 3 || _destPoint == 4 || _destPoint == 6)
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

    private void OnTriggerEnter(Collider other)
    {
        /*
         * https://docs.unity3d.com/Manual/nav-MixingComponents.html
         * add charactercontroller or collider with rigidbody
         * otherwise this code will not excecute
         */
        if (other.gameObject.layer == 11)
        {
            _isCharacterHittingOnMe = true;
            _hudHealth.Health--;
        }
    }

    private void SetupStartPositionNPC()
    {
        if (_isSecondNPC) // for now the 2 npc can t spawn at the same spawnpoint
        {
            _destPoint = Random.Range(4, _waypoints.Length - 1);
        }
        else
        {
            _destPoint = Random.Range(0, 3);
        }
    }

    private void ApplyDeath()
    {
        if (_hudHealth.Health <= 0)
        {
            _isDeath = true;
            _swordCollider.enabled = false; // hide collider so that the sword cannot doe extra damage if he use the death animation
            _ac.DeathAnimation(true);
            _ac.FightAnimation(false);
            _deathTimer += Time.deltaTime;

            if (_deathTimer >= 5f)
            {
                _swordCollider.enabled = true;
                _ac.DeathAnimation(false);
                this.transform.position = _waypoints[Random.Range(0, _waypoints.Length - 1)].position;
                _hudHealth.Health = _hudHealth.StartHealth;
                _isCharacterHittingOnMe = false;
                _isDeath = false;
                _deathTimer = 0;
            }
        }
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
    
    private bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        // https://www.youtube.com/watch?v=BJPSiWNZVow
        Collider[] overlaps = new Collider[100];
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
                  
                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
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
