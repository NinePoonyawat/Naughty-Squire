using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject ShockWave;
    public GameObject player;
    RaycastHit hit;
    Vector3 playerLastHit;
    float maxdis = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WaitAndDestroy(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0,transform.position);
        if (Physics.Raycast(transform.position,transform.forward, out hit)) {
            if (hit.collider) {
                lr.SetPosition(1,hit.point);
                float distance = Vector3.Distance(hit.point,player.transform.position);
                if (distance < maxdis) {
                    maxdis = distance;
                    playerLastHit = hit.point;
                }
            }
        } else {
            lr.SetPosition(1,transform.forward*5000);
            float distance = Vector3.Distance(transform.forward*5000,player.transform.position);
            if (distance < maxdis) {
                maxdis = distance;
                playerLastHit = transform.forward*5000;
            }
        }
    }

    IEnumerator WaitAndDestroy(float delay) {
        yield return new WaitForSeconds(delay);
        GameObject ob = Instantiate(ShockWave, lr.GetPosition(1), transform.rotation);
        Destroy(gameObject);
        Destroy(ob,6f);
    }
}
