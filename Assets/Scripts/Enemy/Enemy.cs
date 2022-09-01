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
    public float losRadius = 45f;
    public float degreesPerSecond = 1f;


    // Update is called once per frame
    void Update()
    {
        CheckLOS();
        //Debug.Log(playerIsInLOS);
        if (playerIsInLOS) agent.SetDestination(player.transform.position);
        else {
            agent.velocity = Vector3.zero;
            float angle = transform.rotation.eulerAngles.y;
            //Debug.Log(Time.deltaTime);   
            //transform.Rotate(degreesPerSecond * Time.deltaTime, 0, 0);
            angle += degreesPerSecond;
            Debug.Log(angle);
            transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
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
            playerIsInLOS = true;
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
