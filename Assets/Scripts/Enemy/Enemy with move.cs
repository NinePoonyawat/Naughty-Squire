using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemywithMove : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    public bool playerIsInLOS;
    public float fieldOfViewAngle;
    public float losRadius;
    
    private Vector3 currentRandomPos;
    private float timeTilNextMovement = 3f;

    private void MakeMovementDecision() {
        timeTilNextMovement = 3f;
        currentRandomPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-4,5));

    }

    void Start() {
        currentRandomPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        CheckLOS();
        if (playerIsInLOS) agent.SetDestination(player.transform.position);
        else {
            if (timeTilNextMovement <= 0) {
                agent.velocity = Vector3.zero;
                MakeMovementDecision();
            }
            else {
                timeTilNextMovement -= Time.fixedDeltaTime;
            }
            agent.SetDestination(currentRandomPos);
            }
    }

    void CheckLOS() 
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f) 
        {   
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
    
}
