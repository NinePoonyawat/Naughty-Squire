using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;


public abstract class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float health;
    public float damageRatio = 1;

    public enum State {
        Idle,
        Alert,
        Walk,
        Attack,
        Cooldown,
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
    //public Vector3 destination;

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
    // Vector3 noisePosition;
    public bool aiHeardPlayer = false;
    public float noiseTravelDistance;
    //public float spinspeed = 3f;
    //private bool canspin = false;
    //private float isSpiningTime; // search player noise position
    //public float spinTime = 3f;
    
    [Header("AI Bool Check")]
    protected float timeTilAlert = 10f;
    protected float timeTilNextMovement = 3f;
    // protected float timeBetweenAttacks = 3.5f;

    protected float timeAttack = 2f;
    [Header("Behavior")]
    public bool CanAlert = true;
    public bool FleeAble;
    public bool randomCharging;
    public bool isShooter;
    public AnimationCurve MoveCurve;
    public GameObject healthBarUI;
    public Slider slider;

    [Header("Bomb Shooter")]
    public Transform firePoint;
    public Rigidbody Bullet;
    public Rigidbody Bomb;
    public bool IsBombBullet;
    public float ShootAngle;


    
    Mesh mesh;
    
    void Start() {
        FindObjectOfType<FPSController>().walkEvent += NoiseCheck;
        EnemyState = State.Idle;
        StartCoroutine(WaitRandomSpeed(5f));
        StartNextState();
        //meshColor.a = 0.5f;
        //starterAssetInputs = GetComponent<StarterAssetsInputs>();
        if(group == -1) group = Random.Range(0,3);
        //AIManager.Instance.Units.Add(this);
        nextgroup = 2;
        AIManager.Instance.AddDictList(group,this);
        health = maxHealth;

        player = GameObject.Find("PlayerHitbox");
        if (player == null) player = GameObject.Find("PlayerWithCamera/PlayerArmature");
        slider.value = CalculateHealth();
    }

    void StartNextState() {
        switch(EnemyState) {
            case State.Idle:
                AImove();
                if (playerIsInLOS) EnemyState = State.Alert;  StartCoroutine(Next(0f));
                break;
            case State.Alert:
                CheckAlerting();
                nextPosition = player.transform.position; EnemyState = State.Walk;  
                StartCoroutine(Next(0f));
                break;
            case State.Walk:
//                Debug.Log("State Walk");
                walking();
                if (!playerIsInLOS) CheckWalking();  StartCoroutine(Next(0f));
                break;
            case State.Attack:
                agent.ResetPath();
                //Attacking();
                StartCoroutine(Attacking(timeAttack));
                break;
            case State.Cooldown:
                StartCoroutine(Cooldowning(1f));
                break;
            case State.Flee:
                //Debug.Log(nextPosition);
                Fleeing(); StartCoroutine(Next(0f));
                break;
        }
    }


    IEnumerator Cooldowning(float delay) {
        yield return null;
        agent.ResetPath();
        agent.updateRotation =false;
        Vector3 direction = (transform.position - player.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.transform.position) * Random.Range(5,10);
        Vector3 dest = GetPositionAroundObject(player.transform);
        Debug.Log(dest + ": " + dest);
        StartCoroutine(MoveTo(dest,timeTilNextMovement));
        //transform.position = Vector3.Lerp(transform.position,dest,Time.deltaTime);
        // float speed =  Mathf.Pow(distance*0.98f / Mathf.Sin(2*Mathf.Deg2Rad*15),0.5f);
        // agent.velocity = new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*15),direction.z) * speed;
        
    }

    Vector3 GetPositionAroundObject(Transform tx) {
        float radius = 5f;
	    Vector2 offset = (Random.insideUnitCircle * radius) + new Vector2(10,10);
        float direction = (tx.position - transform.position).x;
        if ((direction >= 0 && offset.x < 0) || (direction < 0 && offset.x < 0)) offset.x = -offset.x;
	    Vector3 pos = new Vector3(tx.position.x+offset.x,tx.position.y,tx.position.z + offset.y);
	    return pos;
}

    IEnumerator Next(float delay) {
        if (delay > 0) yield return new WaitForSeconds(delay);
        else yield return null;
        StartNextState();
    }
    
    void Update()
    {
        //Debug.Log(player.GetComponent<Collider>().tag);
        //Debug.Log(EnemyState);
        slider.value = CalculateHealth();
        CheckLOS();
        if (FleeAble && EnemyState != State.Flee) CheckFlee();
        //StartNextState();
        //NoiseCheck();
        
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

    protected float CalculateHealth() {
        return health/maxHealth;
    }

    // Health logic
    public void TakeDamage (float damage)
    {
        health -= damage * damageRatio;
        //Debug.Log("i take " + damage + " dmg");
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GetComponent<LootDrop>().Drop();
        AIManager.Instance.RemoveDictList(group,this);
        FindObjectOfType<FPSController>().walkEvent -= NoiseCheck;
        Destroy(gameObject);
    }
    // Debuff
    public void Slow (float multiply)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= multiply;
    }
    public void Fragile (float multiply)
    {
        damageRatio *= multiply;
    }


    // public void SetAlert(bool alert) {
    //     this.alert = alert;
    // }

    public virtual void walking() {
        //if (!playerIsInLOS) EnemyState = State.Idle;
//        Debug.Log("Walking");
        //Debug.Log(nextPosition);
        agent.SetDestination(nextPosition);
        lookatPosition();

        

        // Vector3 targetPosition = new Vector3( destination.x, 
        //                                 transform.position.y, 
        //                                 destination.z ) ;
        // transform.LookAt(targetPosition);
        CheckAttacking();
        //if (EnemyState == State.Attack) Attack();
    }
    protected void lookatPosition() {
        Vector3 targetPosition = new Vector3(nextPosition.x, transform.position.y,nextPosition.z );
        //Vector3 targetPosition = nextPosition;
        //Debug.Log(targetPosition - transform.position);
        Quaternion lookOnLook = Quaternion.LookRotation(targetPosition - transform.position); 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 10 * Time.deltaTime);
    }

    IEnumerator MoveTo(Vector3 position, float delay) {
        //Debug.Log("MOVe");
        Vector3 start = transform.position;
        Vector3 end = position;
        float t = 0;
        while(t < delay) {
            //Debug.Log(t);
            agent.SetDestination(Vector3.Lerp(start,end,MoveCurve.Evaluate(t)));
            Quaternion lookOnLook = Quaternion.LookRotation(new Vector3(nextPosition.x, transform.position.y,nextPosition.z ) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, t*2);
            t += Time.deltaTime;
            // Vector3 targetPosition = new Vector3( player.transform.position.x, transform.position.y, player.transform.position.z ) ;
            // transform.LookAt(targetPosition);
            yield return null;
        }
        lookatPosition();
        //agent.SetDestination(player.transform.position);
        //CheckAttacking();
        nextPosition = player.transform.position;
        EnemyState = State.Walk;
        StartCoroutine(Next(0f));
    }
    IEnumerator Attacking(float delay) {
        //Debug.Log("STOP Atack");
        //agent.SetDestination(transform.position);
        lookatPosition();
        
        // Vector3 targetPosition = new Vector3( player.transform.position.x, 
        //                                 transform.position.y, 
        //                                 player.transform.position.z ) ;
        // transform.LookAt(targetPosition);

        if (!isShooter) StartCoroutine(NormalAttackMove(delay));
        else StartCoroutine(ShooterAttackMove(delay));
        yield return new WaitForSeconds(delay);
        EnemyState = State.Cooldown;
        StartCoroutine(Next(0f));
        //yield return null;      
    }
    protected IEnumerator NormalAttackMove(float delay) {
        for (float d = delay; d > 0; d -= Time.deltaTime) {
            lookatPosition();
            yield return null;
        }  
        HitableObject hit = player.GetComponent<HitableObject>();
        if (hit != null && hit.tag == "Player" && (Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z),new Vector2(transform.position.x,transform.position.z)) <= (StopDistance * 1.5)))
        {
            hit.TakeDamage(10);
            //FindObjectOfType<AudioManager>().Play("PistolBulletHit");
        }

        Debug.Log(this.name + ": -10hp");
    }
    void ResetAlert() {
        CanAlert = true;
    }

    protected IEnumerator ShooterAttackMove(float delay) {
        for (float d = delay; d > 0; d -= Time.deltaTime) {
            lookatPosition();
            yield return null;
        }    
        // yield return new WaitForSeconds(delay);
        if (!IsBombBullet) {
            Debug.Log("SHOOOTTTT");
            GameObject bullet = Instantiate(Bullet, firePoint.position, transform.rotation).gameObject;
            //GameObject bullet = Instantiate(pfBulletProjectile, transform.position, Quaternion.identity).gameObject;
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward *32f,ForceMode.Impulse);
            // rb.AddForce(transform.up *8f,ForceMode.Impulse);
        } else {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            //float swAngle = Vector2.Angle(new Vector2(transform.position.x,transform.position.z),new Vector2(player.transform.position.x,player.transform.position.z));
            float distance = Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z), new Vector2(transform.position.x,transform.position.z));
            float speed =  Mathf.Pow(distance*0.98f / Mathf.Sin(2*Mathf.Deg2Rad*ShootAngle),0.5f);
            //transform.LookAt(new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z));
            Rigidbody rb = Instantiate(Bomb, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.velocity = new Vector3(projection * Mathf.Sin(Mathf.Deg2Rad*swAngle), 
            //   speed * Mathf.Sin(Mathf.Deg2Rad*ShootAngle), projection * Mathf.Cos(Mathf.Deg2Rad*swAngle));
            rb.velocity = new Vector3(direction.x,direction.y + speed * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z) * speed;
            //rb.AddForce(direction*speed, ForceMode.Impulse);
        }
    }

    void ChangeGroup() {
        nextPosition = AIManager.Instance.GetNearestSpawnPoint(group,out nextgroup);
        //Debug.Log(nextPosition);
    }

    public void CheckFlee() {
        if (AIManager.Instance.GetListSize(group) < 2) {
            ChangeGroup();
            //Debug.Log("check flee");
            EnemyState = State.Flee;
        }
    }

    void Fleeing() {
        agent.SetDestination(nextPosition);
        //EnemyState = State.Walk;
        if (Vector3.Distance(agent.transform.position, nextPosition) <= 5) {
            AIManager.Instance.AddDictList(nextgroup,this);
            AIManager.Instance.RemoveDictList(group,this);
            group = nextgroup;
            EnemyState = State.Idle;
            //StartCoroutine(Next(0f));
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
        if (aiMemoriesPlayer || aiHeardPlayer) StartCoroutine(AiMemory());
        //if (aiHeardPlayer) StartCoroutine(AiMemory());
    }
    protected virtual void CheckAttacking() {
        //Debug.Log(StopDistance + ": " + Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z),new Vector2(transform.position.x,transform.position.z)));
        if (Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z),new Vector2(transform.position.x,transform.position.z)) <= StopDistance) {
            EnemyState = State.Attack;
            //Debug.Log("change attack state");
        } else if (playerIsInLOS || aiMemoriesPlayer || aiHeardPlayer){
            EnemyState = State.Walk;
            if (playerIsInLOS || aiMemoriesPlayer) nextPosition = player.transform.position;
            //Debug.Log("change walk state");
        } else EnemyState = State.Idle;
        lookatPosition();
    }
    public void CheckLOS() 
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        Vector3 direction = player.transform.position - transform.position;
        Vector2 direction2d = new Vector2(player.transform.position.x,player.transform.position.z) - new Vector2(transform.position.x,transform.position.z);
        float angle = Vector2.Angle(direction2d, new Vector2(transform.forward.x,transform.forward.z));
        //Debug.Log("Angel = " + angle);
        if (angle < fieldOfViewAngle) 
        {   
            //playerIsInLOS = true;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, losRadius,layerMask)) 
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player") 
                {
                    //Debug.Log("Found");
                    //EnemyState = State.Alert;
                    playerIsInLOS = true;
                    nextPosition = player.transform.position;
                } 
                else
                {
                    playerIsInLOS = false;
                }
            } else playerIsInLOS = false;
        }
    }

    public void SetNextPosition(Vector3 next) {
        nextPosition = next;
    }
    public void NoiseAlert(Vector3 Source) {
        EnemyState = State.Walk;
        aiHeardPlayer = true;
        nextPosition = Source;
    }

    void NoiseCheck() {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log("distance = " + distance + " noiseTravel = " + noiseTravelDistance );
        if (distance <= noiseTravelDistance) {
            //Debug.Log("Alert");

            NoiseAlert(player.transform.position);
        }
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

    // void GoToNoisePosition() {
    //     agent.SetDestination(noisePosition);
    //     aiHeardPlayer = false;
    // }

    IEnumerator AiMemory() {
        increasingMemoryTime = 0;

        while (increasingMemoryTime < memoryStartTime) {
            increasingMemoryTime += Time.deltaTime;
            // aiMemoriesPlayer = true;
            yield  return null;
        }
        aiHeardPlayer = false;
        aiMemoriesPlayer = false;
        EnemyState = State.Idle;
    }


    IEnumerator WaitRandomSpeed(float delay) {
        yield return new WaitForSeconds(delay);
        if (Random.value > 0.4 && EnemyState == State.Walk && randomCharging) {
            agent.speed = 10f;
            Debug.Log("Speed");
        }
        else agent.speed = 3.5f;
        StartCoroutine(WaitRandomSpeed(5f));
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
