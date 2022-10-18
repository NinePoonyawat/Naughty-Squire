using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(0)]
public class AIManager : MonoBehaviour
{
    public GameObject player;
    public int AlertGroup = -1;
    //public float RadiusAroundTarget = 0.5f;
    public List<EnemyBase> Units;
    private Dictionary<Vector3,List<EnemyBase>> SpawnPointList = new Dictionary<Vector3,List<EnemyBase>>();

    private static AIManager _instance;
    
    public static AIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private void Update()
    {
        //Debug.Log(SpawnPointList.Count);
        if (AlertGroup > -1) {
            Units = GetListbyIndex(AlertGroup);
            MakeAgentsAlert();
        }
    }

    // private void Start() {
    //     for (int i =0; i <3; i++) {
    //         SpawnPointList.Add(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),new List<EnemyBase>());
    //         Debug.Log("ADD DICT");

    //     }
    // }

    private void Awake()
    {
        for (int i =0; i <3; i++) {
            SpawnPointList.Add(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),new List<EnemyBase>());
            //Debug.Log("ADD DICT");
        }
        
        if (Instance == null) {
            Instance = this;
            return;
        }
        //Destroy(gameObject);
    }

    // private void MakeAgentsCircleTarget()
    // {
    //     for (int i = 0; i < Units.Count; i++) {
    //         Units[i].MoveTo(new Vector3(
    //             player.transform.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count),
    //             player.transform.position.y,
    //             player.transform.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / Units.Count))
    //         );
    //         //Debug.Log(player.transform.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count));
    //     }
    // }
    public void SetGroupAlerts(int group) {
        AlertGroup = group;
    }

    private List<EnemyBase> GetListbyIndex(int group) {
        int i = 0;
        //Debug.Log(SpawnPointList.Count);
        foreach (Vector3 key in SpawnPointList.Keys) {
            if (group == i++) {
                return SpawnPointList[key];
            }
        }
        //Debug.Log("CANT FIND");
        return new List<EnemyBase>();
    }

    private Vector3 GetSpawnPoint(int group) {
        int i = 0;
        foreach (Vector3 key in SpawnPointList.Keys) {
            if (group == i++) {
                return key;
            }
        }
        return new Vector3(0,0,0);
    }

    public Vector3 GetNearestSpawnPoint(int group, out int nextgroup) {
        Vector3 spawnpoint = GetSpawnPoint(group);
        Vector3 min_spawnpoint = spawnpoint;
        float min_distance = float.PositiveInfinity;
        nextgroup = group;
        int i = 0;
        foreach (Vector3 key in SpawnPointList.Keys) {
            float distance = Vector3.Distance(spawnpoint,key);
            if (group != i && distance < min_distance) {
                nextgroup = i;
                min_spawnpoint = key;
                min_distance = distance;
            }
            i++;
        }
        return spawnpoint;
    }

    public int GetListSize(int group) {
        return GetListbyIndex(group).Count;
    }

    public void AddDictList(int group,EnemyBase Enemy) {
        GetListbyIndex(group).Add(Enemy);
        //Debug.Log("ADDed");
    }

    public void RemoveDictList(int group,EnemyBase Enemy) {
        //Debug.Log("Size before Remove" + GetListbyIndex(group).Count);
        GetListbyIndex(group).Remove(Enemy);
        //Debug.Log("Size after Remove" +GetListbyIndex(group).Count);
    }
    private void MakeAgentsAlert()
    {
        //Debug.Log(Units.Count);
        for (int i = 0; i < Units.Count; i++) {
            //Debug.Log("Alert!" + i);
            Units[i].EnemyState = EnemyBase.State.Alert;
        }
        SetGroupAlerts(-1);
    }
    
}
