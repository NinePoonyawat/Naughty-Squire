using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    // Update is called once per frame

    void Update()
    {
        agent.SetDestination(player.transform.position);
        
    }
}
