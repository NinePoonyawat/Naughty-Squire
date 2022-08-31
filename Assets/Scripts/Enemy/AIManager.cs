using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(0)]
public class AIManager : MonoBehaviour
{
    public GameObject player;
    public float RadiusAroundTarget = 0.5f;
    public List<AIUnit> Units = new List<AIUnit>();

    private static AIManager _instance;
    public static AIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private void Update()
    {
        MakeAgentsCircleTarget();
    }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 200, 50), "Move to player"))
        {
            MakeAgentsCircleTarget();
        }
    }

    private void MakeAgentsCircleTarget()
    {
        for (int i = 0; i < Units.Count; i++) {
            Units[i].MoveTo(new Vector3(
                player.transform.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count),
                player.transform.position.y,
                player.transform.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / Units.Count))
            );
            //Debug.Log(player.transform.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count));
        }
    }
}
