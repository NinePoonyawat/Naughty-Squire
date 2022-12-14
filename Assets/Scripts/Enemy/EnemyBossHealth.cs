using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossHealth : HitableObject
{
    [Header("Prefabs")]
    public NavMeshAgent agent;
    public Rigidbody bullet;
    public Rigidbody laser;
    public GameObject player;
    [SerializeField] private GameObject JumpEffect;
    public GameObject Head;
    public bool IsAttack = false;
    public bool IsStun = false;
    // float damageRatio = 1;
    private IEnumerator coroutine;

    public GameObject bulletspawn;
    public GameObject LaserLine;

    [Header("Health Phase Paremeters")]
    public float HealthCon1 = 200f;
    public float HealthCon2 = 120f;

    [Header("Skill Parameters")]
    public float ChargingTime = 10f;
    public float CooldownAfterCharge = 2f;
    public float JumpDamage;
    bool RandomTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > HealthCon1) {
            GetComponent<Animator>().SetBool("ChangeCharge", false);
            GetComponent<Animator>().SetBool("ChangeLaser", false);
        }
        else if (health > HealthCon2) {
            if (!RandomTrigger) RandomState();
            // GetComponent<Animator>().SetBool("ChangeCharge", true);
            // GetComponent<Animator>().SetBool("ChangeLaser", false);
        }
        else {
            Head.GetComponent<BossHead>().SetOpen();
            GetComponent<Animator>().SetBool("ChangeCharge", true);
            GetComponent<Animator>().SetBool("ChangeLaser", true);
        }
    }

    void RandomState() {
        RandomTrigger = true;
        Debug.Log("LOOP");
        GetComponent<Animator>().SetBool("ChangeCharge", Random.value > 0.5f);
        GetComponent<Animator>().SetBool("ChangeLaser", false);
        StartCoroutine(WaitRandomState(6f));
    }

    IEnumerator WaitRandomState(float delay) {
        yield return new WaitForSeconds(delay);
        RandomState();
    }
    public void lookatPosition() {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);
        // Quaternion lookOnLook = Quaternion.LookRotation(targetPosition - agent.transform.position); 
        // agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookOnLook, Time.deltaTime*2);
    }

    public void StopCoroutinesFunc() {
        agent.speed = 3.5f;
        if (coroutine == null) return;
        Debug.Log("Stop :" + coroutine);
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
        FindObjectOfType<BossAudioManager>().Play("Charge");
        StartCoroutine(WaitLaser(delay));
     }

    public void DoStunWait(float delay) {
        coroutine = Stun(delay);
        StartCoroutine(Stun(delay));
    }
    IEnumerator Shooting(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);

        FindObjectOfType<BossAudioManager>().Play("Shoot");

        GameObject rc = Instantiate(bullet, bulletspawn.transform.position, transform.rotation).gameObject;
        // Debug.Log(rc.transform.position);
        // rc.AddForce(agent.transform.forward *32f,ForceMode.Impulse);
        // rc.AddForce(agent.transform.up *8f,ForceMode.Impulse);
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
        if (!IsStun) GetComponent<Animator>().SetTrigger("Lasering"); // demo
    }

    IEnumerator Stun(float delay) {
        //StopAllCoroutinesFunc();
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetBool("NotStun",true);
    }

    public void SetIsAttack(bool isattack) {
        IsAttack = isattack;
    }
    public void SetStun() {
        StopCoroutinesFunc();
        IsStun = true;
        agent.ResetPath();
        GetComponent<Animator>().SetTrigger("Stunning");
        //IsStun = isStun;
    }
    public void setIsStun(bool s) {
        IsStun = s;
    }

    // public void TakeDamage (float damage)
    // {
    //     health -= damage * damageRatio;
    //     //Debug.Log("i take " + damage + " dmg");
    //     if (health <= 0)
    //     {
    //         Die();
    //     }
    // }

    void Die()
    {
        GetComponent<LootDrop>().Drop();
        Destroy(gameObject);
    }

    public void JumpAttack() {
        GameObject grenade = Instantiate(JumpEffect, this.transform.position, Quaternion.identity);
        grenade.transform.localScale = new Vector3(10, 10, 10);
         if (Vector3.Distance(agent.transform.position,player.transform.position) < 10) {
            HitableObject entityHit = player.GetComponent<HitableObject>();
            if (entityHit != null)
            {
            Debug.Log("hit player");
            entityHit.TakeDamage(JumpDamage);
            }
         }
        Destroy(grenade, 3f);
    }

    public void LaserAttack() {
        //FindObjectOfType<BossAudioManager>().Play("Shoot");
        
        GameObject grenade = Instantiate(LaserLine, bulletspawn.transform.position, transform.rotation).gameObject;
    }
    
}
