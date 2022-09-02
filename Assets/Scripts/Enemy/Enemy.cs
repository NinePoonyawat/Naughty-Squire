using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    [SerializeField]
    public bool playerIsInLOS = false;
    public float fieldOfViewAngle = 160f;
    public float losRadius = 1f;
    public float degreesPerSecond = 1f;
    public int DirRotate = 1;
    public float timeTilNextMovement = 2f;
    public float timeBetween = 0.01f;

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
    
}
