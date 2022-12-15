using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject[] enemySpawner;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private Transform player;

    private float timeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 60)
        {
            RandomSpawn();
            timeCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            RandomSpawn();
        }
    }

    void RandomSpawn()
    {
        if (enemySpawner.Length == 1)
        {
            Spawn(enemySpawner[0]);
            return;
        }
        int a,b;
        if (enemySpawner.Length == 2)
        {
            a = 0; b = 1;
        }
        else
        {
            a = Random.Range(0,enemySpawner.Length);
            b = Random.Range(0,enemySpawner.Length);
            while (a == b) b = Random.Range(0,enemySpawner.Length);
        }
        float aDistance = Vector3.Distance(enemySpawner[a].transform.position,player.position);
        float bDistance = Vector3.Distance(enemySpawner[b].transform.position,player.position);
        int c = (aDistance > bDistance)? a : b;
        Spawn(enemySpawner[c]);
    }

    void Spawn(GameObject spawnPoint)
    {
        int enemyIndex = Random.Range(0,enemyPrefab.Length);
        GameObject GO = Instantiate(enemyPrefab[enemyIndex],spawnPoint.transform.position,Quaternion.identity,enemyParent);
    }
}
