using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(0)]
public class AIManager : MonoBehaviour
{
    public GameObject player;
    public bool Alerts;
    //public float RadiusAroundTarget = 0.5f;
    public List<EnemyBase> Units = new List<EnemyBase>();

    private static AIManager _instance;
    
    public static AIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private void Update()
    {
        if (Alerts) MakeAgentsAlert();
    }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
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
    public void SetAlerts(bool Alerts) {
        this.Alerts = Alerts;
    }
    private void MakeAgentsAlert()
    {
        Debug.Log("Alert!");
        for (int i = 0; i < Units.Count; i++) {
            Units[i].SetAlert(true);
        }
        SetAlerts(false);
    }
    
}
