using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public abstract class EnemyBase : MonoBehaviour
{
    // Health
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public enum State {
        Idle,
        Alert,
        Walk,
        Attack
    }
    public State EnemyState;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public bool alert = false;
    public int group;

    //ai sight
    public bool playerIsInLOS;
    public float fieldOfViewAngle;
    public float losRadius;
    public float height = 0.1f;
    public Color meshColor = Color.red;

    //ai sight and memory
    private bool  aiMemoriesPlayer = false;
    public float memoryStartTime = 10f;
    private float increasingMemoryTime;

    //ai hearing
    Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    //public float spinspeed = 3f;
    //private bool canspin = false;
    //private float isSpiningTime; // search player noise position
    //public float spinTime = 3f;
    
    public bool alreadyAttacked = false;
    protected float timeTilNextMovement = 2f;
    protected float timeBetweenAttacks = 0.5f;

    private StarterAssetsInputs starterAssetInputs;

    
    Mesh mesh;
    
    void Start() {
        EnemyState = State.Idle;
        meshColor.a = 0.5f;
        //starterAssetInputs = GetComponent<StarterAssetsInputs>();
        if(group == -1) group = Random.Range(0,3);
        //AIManager.Instance.Units.Add(this);
        AIManager.Instance.AddDictList(group,this);
        health = maxHealth;
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

    public void SetAlert(bool alert) {
        this.alert = alert;
    }

    public virtual void walking() {
        agent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.GetComponent<Collider>().tag);
        CheckLOS();
        NoiseCheck();
        //Debug.Log(playerIsInLOS);
        if (playerIsInLOS || alert) {
            if (playerIsInLOS) AIManager.Instance.SetGroupAlerts(group);
            else alert = false;
            walking();
            aiMemoriesPlayer = true;
        }else if (aiMemoriesPlayer) {
            StartCoroutine(AiMemory());
            //Debug.Log("MEMO" + increasingMemoryTime);
        }else if (aiHeardPlayer) {
            //Debug.Log("HEard");
            GoToNoisePosition();
        }else {
           // Debug.Log("lost");
            AImove();
        }
    }
    
    //Unity calls when the script is loaded or a value changes in the Inspector
    void OnValidate() {
        mesh = CreateWedgeMesh();
    }

    void OnDrawGizmos() {
        if (mesh) {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }

    void CheckLOS() 
    {
        Vector3 direction = player.transform.position - transform.position;
        //Debug.Log(direction);
        float angle = Vector3.Angle(direction, transform.forward);
        //Debug.Log(angle);
        if (angle < fieldOfViewAngle * 0.5f) 
        {   
            //playerIsInLOS = true;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, losRadius)) 
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player") 
                {
                    //Debug.Log("Found");
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
        alert = false;
        aiHeardPlayer = false;
        aiMemoriesPlayer = false;
    }

    Mesh CreateWedgeMesh() {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments *4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -fieldOfViewAngle, 0) * Vector3.forward*losRadius;
        Vector3 bottomRight = Quaternion.Euler(0, fieldOfViewAngle, 0) * Vector3.forward*losRadius;

        Vector3 topCenter = bottomCenter + Vector3.up*height;
        Vector3 topRight = bottomRight + Vector3.up*height;
        Vector3 topLeft = bottomLeft + Vector3.up*height;

        int vert = 0;

        //left side
        vertices[vert++]  = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++]  = topCenter;
        vertices[vert++] = bottomCenter;
        //right side
        vertices[vert++]  = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++]  = bottomRight;
        vertices[vert++] = bottomCenter;
        
        float currentAngle = -fieldOfViewAngle;
        float deltaAngle = (fieldOfViewAngle *2) / segments;
        for (int i =0; i<segments; ++i) {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward*losRadius;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward*losRadius;

            topCenter = bottomCenter + Vector3.up*height;
            topRight = bottomRight + Vector3.up*height;
            topLeft = bottomLeft + Vector3.up*height;

            //far side 
            vertices[vert++]  = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++]  = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++]  = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++]  = bottomRight;
            vertices[vert++] = bottomLeft;    
            
            currentAngle += deltaAngle;
        }
        

        for (int i =0; i< numVertices; i++) {
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles  = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }



    protected abstract void MakeMovementDecision();

    protected abstract void AImove();
}
