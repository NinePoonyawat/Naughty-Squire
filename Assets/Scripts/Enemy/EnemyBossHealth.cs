using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float health;
    public NavMeshAgent agent;
    public Rigidbody projectile;
    public GameObject player;
    public bool IsAttack = false;
    float damageRatio = 1;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 150) {
            GetComponent<Animator>().SetBool("ChangeCharge", false);
            GetComponent<Animator>().SetBool("ChangeLaser", false);
        }
        else if (health > 100) {
            GetComponent<Animator>().SetBool("ChangeCharge", true);
            GetComponent<Animator>().SetBool("ChangeLaser", false);
        }
        else {
            GetComponent<Animator>().SetBool("ChangeCharge", false);
            GetComponent<Animator>().SetBool("ChangeLaser", true);
        }
    }
    public void lookatPosition() {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Quaternion lookOnLook = Quaternion.LookRotation(targetPosition - agent.transform.position); 
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookOnLook, Time.deltaTime*2);
    }

    public void StopCoroutinesFunc() {
        agent.speed = 3.5f;
        if (coroutine == null) return;
        Debug.Log(coroutine);
        StopCoroutine(coroutine);
    }

    public void DoShooting(float delay) {
        coroutine = Shooting(delay);
        StartCoroutine(Shooting(delay));
    }

    public void DoCharge(float delay) {
        coroutine = WaitCharge(delay);
        StartCoroutine(WaitCharge(delay));
     }

    public void DoChargeLaser(float delay) {
        coroutine = WaitLaser(delay);
        StartCoroutine(WaitLaser(delay));
     }

    public void DoStunWait(float delay) {
        coroutine = Stun(delay);
        StartCoroutine(Stun(delay));
    }
    IEnumerator Shooting(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);
        Rigidbody rc = Instantiate(projectile, agent.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rc.AddForce(agent.transform.forward *32f,ForceMode.Impulse);
        rc.AddForce(agent.transform.up *8f,ForceMode.Impulse);
        if (!GetComponent<Animator>().GetBool("ChangeCharge")) StartCoroutine(Shooting(delay));
    }
    IEnumerator WaitCharge(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);
        if (IsAttack) {
            GetComponent<Animator>().SetTrigger("Attacking");
        }
        else GetComponent<Animator>().SetBool("EnragedAttack",false);
    }

    IEnumerator WaitLaser(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetTrigger("Lasering"); // demo
    }

    IEnumerator Stun(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetBool("NotStun",true);
    }

    public void SetIsAttack(bool isattack) {
        IsAttack = isattack;
    }

    public void TakeDamage (float damage)
    {
        health -= damage * damageRatio;
        //Debug.Log("i take " + damage + " dmg");
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<LootDrop>().Drop();
        Destroy(gameObject);
    }
    
}
