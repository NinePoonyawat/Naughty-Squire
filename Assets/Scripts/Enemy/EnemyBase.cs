using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public abstract class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public enum State {
        Idle,
        Alert,
        Walk,
        Attack,
        Flee,
    }
    [Header("Essential")]
    public State EnemyState;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    //public bool alert = false;
    public int group;
    public int nextgroup;

    private Vector3 nextPosition;

    [Header("AI Detection")]
    public bool playerIsInLOS;
    public float fieldOfViewAngle;
    public float losRadius;
    public float height = 0.1f;
    public float StopDistance;
    //public Color meshColor = Color.red;


    [Header("AI Memory")]
    public bool  aiMemoriesPlayer = false;
    public float memoryStartTime = 10f;
    private float increasingMemoryTime;

    [Header("AI Hearing")]
    Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    //public float spinspeed = 3f;
    //private bool canspin = false;
    //private float isSpiningTime; // search player noise position
    //public float spinTime = 3f;
    
    [Header("AI Bool Check")]
    public bool alreadyAttacked = false;
    public bool CanAlert = true;
    protected float timeTilAlert = 10f;
    protected float timeTilNextMovement = 2f;
    protected float timeBetweenAttacks = 3.5f;
    public bool FleeAble;


    private StarterAssetsInputs starterAssetInputs;

    
    Mesh mesh;
    
    void Start() {
        EnemyState = State.Idle;
        //meshColor.a = 0.5f;
        //starterAssetInputs = GetComponent<StarterAssetsInputs>();
        if(group == -1) group = Random.Range(0,3);
        //AIManager.Instance.Units.Add(this);
        AIManager.Instance.AddDictList(group,this);
        health = maxHealth;

        player = GameObject.Find("PlayerHitbox");
    }

    void StartNextState() {
        switch(EnemyState) {
            case State.Idle:
                AImove();
                if (playerIsInLOS) EnemyState = State.Alert;
                break;
            case State.Alert:
                CheckAlerting();
                EnemyState = State.Walk;
                break;
            case State.Walk:
                walking();
                if (!playerIsInLOS) CheckWalking();
                break;
            case State.Attack:
                agent.SetDestination(agent.transform.position);
                Attacking();
                break;
            case State.Flee:
                Fleeing();
                break;
        }
    }
    void Update()
    {
        //Debug.Log(player.GetComponent<Collider>().tag);
        //Debug.Log(EnemyState);
        CheckLOS();
        if (FleeAble) CheckFlee();
        StartNextState();
        NoiseCheck();
        
        // if (playerIsInLOS || EnemyState == State.Alert) {
        //     if (EnemyState == State.Idle) EnemyState = State.Alert;
        // } else EnemyState = State.Idle;

        // //Debug.Log(playerIsInLOS);
        // if (EnemyState != State.Idle) {
        //     if (playerIsInLOS && CanAlert) {
        //         Debug.Log("Alert!");
        //         AIManager.Instance.SetGroupAlerts(group);
        //         CanAlert = false;
        //         Invoke("ResetAlert",timeTilAlert);
        //     }
        //     if (EnemyState == State.Alert) EnemyState = State.Walk;
        //     walking();
        //     aiMemoriesPlayer = true;
        // }else if (aiMemoriesPlayer) {
        //     StartCoroutine(AiMemory());
        //     //Debug.Log("MEMO" + increasingMemoryTime);
        // }else if (aiHeardPlayer) {
        //     //Debug.Log("HEard");
        //     GoToNoisePosition();
        // }else {
        //    // Debug.Log("lost");
        //     AImove();
        // }
    }

    // Health logic
    public void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log("i take " + damage + " dmg");
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
        AIManager.Instance.RemoveDictList(group,this);
    }


    // public void SetAlert(bool alert) {
    //     this.alert = alert;
    // }

    public virtual void walking() {
        //if (!playerIsInLOS) EnemyState = State.Idle;
        Debug.Log("STOPWALK!");
        agent.SetDestination(player.transform.position);

        Vector3 targetPosition = new Vector3( player.transform.position.x, 
                                        transform.position.y, 
                                        player.transform.position.z ) ;
        transform.LookAt(targetPosition);
        CheckAttacking();
        //if (EnemyState == State.Attack) Attack();
    }

    protected void Attacking() {
        //Debug.Log("STOP Atack");
        //agent.SetDestination(transform.position);
        Vector3 targetPosition = new Vector3( player.transform.position.x, 
                                        transform.position.y, 
                                        player.transform.position.z ) ;
        transform.LookAt(targetPosition);

        if (!alreadyAttacked) {
            Debug.Log("FIRE!!!++++");         
            AttackMove();    
            alreadyAttacked = true;
            CheckAttacking();
            Invoke("ResetAttack",timeBetweenAttacks);
        }
    }
    protected virtual void AttackMove() {
        HitableObject hit = player.GetComponent<HitableObject>();
        if (hit != null)
        {
            hit.TakeDamage(10);
            //FindObjectOfType<AudioManager>().Play("PistolBulletHit");
        }

        Debug.Log(this.name + ": -10hp");
    }

    void ResetAttack() {
        alreadyAttacked = false;
    }
    void ResetAlert() {
        CanAlert = true;
    }

    void ChangeGroup() {
        nextPosition = AIManager.Instance.GetNearestSpawnPoint(group,out nextgroup);
    }

    void CheckFlee() {
        if (AIManager.Instance.GetListSize(group) < 2) {
            EnemyState = State.Flee;
            ChangeGroup();
        }
    }

    void Fleeing() {
        agent.SetDestination(nextPosition);
        if (Vector3.Distance(agent.transform.position, nextPosition) <= 5) {
            AIManager.Instance.AddDictList(nextgroup,this);
            AIManager.Instance.RemoveDictList(group,this);
            group = nextgroup;
            EnemyState = State.Idle;
        }
    }
    
    //Unity calls when the script is loaded or a value changes in the Inspector
    protected virtual void CheckAlerting() {
        aiMemoriesPlayer = true;
        if (playerIsInLOS && CanAlert) {
            Debug.Log("Alert!");
            AIManager.Instance.SetGroupAlerts(group);
            CanAlert = false;
            Invoke("ResetAlert",timeTilAlert);
        }
    }
    protected virtual void CheckWalking() {
        if (aiMemoriesPlayer) StartCoroutine(AiMemory());
    }
    protected virtual void CheckAttacking() {
        if (Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z),new Vector2(transform.position.x,transform.position.z)) <= StopDistance) {
            EnemyState = State.Attack;
            Debug.Log("change attack state");
        } else {
            EnemyState = State.Walk;
            Debug.Log("change walk state");
        }
    }
    void CheckLOS() 
    {
        Vector3 direction = player.transform.position - transform.position;
        Vector2 direction2d = new Vector2(player.transform.position.x,player.transform.position.z) - new Vector2(transform.position.x,transform.position.z);
        float angle = Vector2.Angle(direction2d, new Vector2(transform.forward.x,transform.forward.z));
        if (angle < fieldOfViewAngle) 
        {   
            //playerIsInLOS = true;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, losRadius)) 
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player") 
                {
                    //Debug.Log("Found");
                    //EnemyState = State.Alert;
                    playerIsInLOS = true;
                } else
                {
                    playerIsInLOS = false;
                }
            }
        }
    }

    void NoiseCheck() {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // if (distance <= noiseTravelDistance) {
        //     var vel = player.GetComponent<Rigidbody>().velocity;
        //     float speed = vel.magnitude; 
        //     //Debug.Log(speed);
        //     if (speed > 0) 
        //     {
        //         noisePosition = player.transform.position;
        //         aiHeardPlayer = true;
        //     }
        //     else 
        //     {
        //         aiHeardPlayer = false;
        //     }
        // }

    }

    void GoToNoisePosition() {
        agent.SetDestination(noisePosition);
        aiHeardPlayer = false;
    }

    IEnumerator AiMemory() {
        increasingMemoryTime = 0;

        while (increasingMemoryTime < memoryStartTime) {
            increasingMemoryTime += Time.deltaTime;
            aiMemoriesPlayer = true;
            yield  return null;
        }
        aiHeardPlayer = false;
        aiMemoriesPlayer = false;
        EnemyState = State.Idle;
    }
    // void OnValidate() {
    //     mesh = CreateWedgeMesh();
    // }

    // void OnDrawGizmos() {
    //     if (mesh) {
    //         Gizmos.color = meshColor;
    //         Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawLine(transform.position, agent.destination);
    //     }
    // }

    // Mesh CreateWedgeMesh() {
    //     Mesh mesh = new Mesh();

    //     int segments = 10;
    //     int numTriangles = (segments *4) + 2 + 2;
    //     int numVertices = numTriangles * 3;

    //     Vector3[] vertices = new Vector3[numVertices];
    //     int[] triangles = new int[numVertices];

    //     Vector3 bottomCenter = Vector3.zero;
    //     Vector3 bottomLeft = Quaternion.Euler(0, -fieldOfViewAngle, 0) * Vector3.forward*losRadius;
    //     Vector3 bottomRight = Quaternion.Euler(0, fieldOfViewAngle, 0) * Vector3.forward*losRadius;

    //     Vector3 topCenter = bottomCenter + Vector3.up*height;
    //     Vector3 topRight = bottomRight + Vector3.up*height;
    //     Vector3 topLeft = bottomLeft + Vector3.up*height;

    //     int vert = 0;

    //     //left side
    //     vertices[vert++]  = bottomCenter;
    //     vertices[vert++] = bottomLeft;
    //     vertices[vert++] = topLeft;

    //     vertices[vert++] = topLeft;
    //     vertices[vert++]  = topCenter;
    //     vertices[vert++] = bottomCenter;
    //     //right side
    //     vertices[vert++]  = bottomCenter;
    //     vertices[vert++] = topCenter;
    //     vertices[vert++] = topRight;

    //     vertices[vert++] = topRight;
    //     vertices[vert++]  = bottomRight;
    //     vertices[vert++] = bottomCenter;
        
    //     float currentAngle = -fieldOfViewAngle;
    //     float deltaAngle = (fieldOfViewAngle *2) / segments;
    //     for (int i =0; i<segments; ++i) {
    //         bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward*losRadius;
    //         bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward*losRadius;

    //         topCenter = bottomCenter + Vector3.up*height;
    //         topRight = bottomRight + Vector3.up*height;
    //         topLeft = bottomLeft + Vector3.up*height;

    //         //far side 
    //         vertices[vert++]  = bottomLeft;
    //         vertices[vert++] = bottomRight;
    //         vertices[vert++] = topRight;

    //         vertices[vert++] = topRight;
    //         vertices[vert++]  = topLeft;
    //         vertices[vert++] = bottomLeft;

    //         //top
    //         vertices[vert++] = topCenter;
    //         vertices[vert++]  = topLeft;
    //         vertices[vert++] = topRight;

    //         //bottom
    //         vertices[vert++] = bottomCenter;
    //         vertices[vert++]  = bottomRight;
    //         vertices[vert++] = bottomLeft;    
            
    //         currentAngle += deltaAngle;
    //     }
        

    //     for (int i =0; i< numVertices; i++) {
    //         triangles[i] = i;
    //     }
    //     mesh.vertices = vertices;
    //     mesh.triangles  = triangles;
    //     mesh.RecalculateNormals();

    //     return mesh;
    // }



    protected abstract void MakeMovementDecision();

    protected abstract void AImove();
}
