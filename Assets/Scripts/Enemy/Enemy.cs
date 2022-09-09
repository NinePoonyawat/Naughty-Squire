using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    public bool playerIsInLOS;
    public float fieldOfViewAngle;
    public float losRadius;
    public float degreesPerSecond = 1f;
    public float height = 0.1f;
    public Color meshColor = Color.red;
    
    private int DirRotate = 1;
    private float timeTilNextMovement = 2f;
    private float timeBetween = 0.25f;

    Mesh mesh;

    void Start() {
        meshColor.a = 0.5f;
    }

    private void MakeMovementDecision() {
        DirRotate = Random.Range(-1,2);
        timeTilNextMovement = 2f;
    }
    // Update is called once per frame
    void Update()
    {
        CheckLOS();
        //Debug.Log(playerIsInLOS);
        if (playerIsInLOS) agent.SetDestination(player.transform.position);
        else {
            agent.velocity = Vector3.zero;
            float angle = transform.rotation.eulerAngles.y;
            if (timeTilNextMovement <= 0) MakeMovementDecision();//Invoke("MakeMovementDecision",timeBetween);
            else {
                timeTilNextMovement -= Time.fixedDeltaTime;
            }
            //Debug.Log(Time.deltaTime);   
            //transform.Rotate(0,degreesPerSecond * Time.deltaTime, 0);
            // if (angle > 45) turnright = false;
            // else if (angle < 305) turnright = true;
            // if (turnright) angle += degreesPerSecond;
            // else angle -= degreesPerSecond; 
            // Debug.Log(angle);
            angle += degreesPerSecond * DirRotate;
            
            transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
            }
    }
    
    //Unity calls when the script is loaded or a value changes in the Inspector
    private void OnValidate() {
        mesh = CreateWedgeMesh();
    }

    void FixedUpdate () {
        
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
                if (hit.collider.tag == "Player") 
                {
                    Debug.Log("Found");
                    playerIsInLOS = true;
                } else
                {
                    playerIsInLOS = false;
                }
            }
        }
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


    private void OnDrawGizmos() {
        if (mesh) {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
        }
    }
    
}
