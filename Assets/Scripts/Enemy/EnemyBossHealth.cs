using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public float HealthCon;
    public bool HealthUpdated1;
    public bool HealthUpdated2;
    public bool HealthUpdated3;

    [Header("Skill Parameters")]
    public float ChargingTime = 10f;
    public float CooldownAfterCharge = 2f;
    public float JumpDamage;
    bool RandomTrigger = false;
    public Slider slider;
    public Slider sliderState1;
    public Slider sliderState2;
    public Slider sliderState3;
    public Slider tmeslider;
    public Slider headslider;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
        HealthCon = HealthCon1;
        HealthUpdated1 = false;
        HealthUpdated2 = false;
        HealthUpdated3 = false;
        ShowTimer(false);
        ShowHeadHealth(false);
        slider = sliderState1;
    }

    // Update is called once per frame
    void Update()
    {

        if (health > HealthCon1) {
            GetComponent<Animator>().SetBool("ChangeCharge", false);
            GetComponent<Animator>().SetBool("ChangeLaser", false);
            if (!HealthUpdated1) {
                //slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.cyan;
                slider = sliderState1;
                HealthCon = HealthCon1;
                maxHealth = health - HealthCon1;
                HealthUpdated1 = true;
            }

        }
        else if (health > HealthCon2) {
            if (!RandomTrigger) RandomState();
            if (!HealthUpdated2) {
                //slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
                sliderState1.value = 0;
                slider = sliderState2;
                HealthCon = HealthCon2;
                maxHealth = health - HealthCon2;
                HealthUpdated2 = true;
            }
            // GetComponent<Animator>().SetBool("ChangeCharge", true);
            // GetComponent<Animator>().SetBool("ChangeLaser", false);
        }
        else {
            damageRatio = 0;
            // Head.GetComponent<BossHead>().SetOpen();
            GetComponent<Animator>().SetBool("ChangeCharge", true);
            GetComponent<Animator>().SetBool("ChangeLaser", true);
            if (!HealthUpdated3) {
                //slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
                sliderState1.value = 0;
                sliderState2.value = 0;
                slider = sliderState3;
                HealthCon = 0;
                maxHealth = health - 0;
                HealthUpdated3 = true;
            }
        }
        slider.value = CalculateHealth();
        headslider.value = CalculateHeadHealth();
    }

    protected float CalculateHealth() {
        return (health-HealthCon)/maxHealth;
    }
    protected float CalculateHeadHealth() {
        return Head.GetComponent<BossHead>().getHealth()/Head.GetComponent<BossHead>().getMaxHealth();
    }

    void RandomState() {
        RandomTrigger = true;
        GetComponent<Animator>().SetBool("ChangeCharge", Random.value > 0.5f);
        GetComponent<Animator>().SetBool("ChangeLaser", false);
        StartCoroutine(WaitRandomState(6f));
    }

    IEnumerator WaitRandomState(float delay) {
        yield return new WaitForSeconds(delay);
        RandomState();
    }
    public void lookatPosition() {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        // transform.LookAt(targetPosition);
        Quaternion lookOnLook = Quaternion.LookRotation(targetPosition - agent.transform.position); 
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookOnLook, Time.deltaTime*2);
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

    public void ShowTimer(bool ShowTimer) {
        tmeslider.gameObject.SetActive(ShowTimer);
    }
    public void ShowHeadHealth(bool ShowTimer) {
        headslider.gameObject.SetActive(ShowTimer);
    }
    IEnumerator Shooting(float delay) {
        //StopAllCoroutinesFunc();
        for (float d = delay; d > 0; d -= Time.deltaTime) {
            lookatPosition();
            yield return null;
        }

        FindObjectOfType<BossAudioManager>().Play("Shoot");

        GameObject rc = Instantiate(bullet, bulletspawn.transform.position, transform.rotation).gameObject;
        // Debug.Log(rc.transform.position);
        // rc.AddForce(agent.transform.forward *32f,ForceMode.Impulse);
        // rc.AddForce(agent.transform.up *8f,ForceMode.Impulse);
        if (!GetComponent<Animator>().GetBool("ChangeCharge") && !IsStun) StartCoroutine(Shooting(delay));
    }
    IEnumerator WaitCharge(float delay) {
        //StopAllCoroutinesFunc();
        for (float d = delay; d > 0; d -= Time.deltaTime) {
            lookatPosition();
            yield return null;
        }  
        if (IsAttack) {
            GetComponent<Animator>().SetTrigger("Attacking");
        }
        else GetComponent<Animator>().SetBool("EnragedAttack",false);
    }

    IEnumerator WaitLaser(float delay) {
        //StopAllCoroutinesFunc();
        for(float TimeLeft = delay; TimeLeft > 0; TimeLeft -= Time.deltaTime) {
            tmeslider.value = TimeLeft/delay;
            lookatPosition();
            yield return null;
        }
        //yield return new WaitForSeconds(delay);
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
    public void DoStun() {
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
        FindObjectOfType<BossAudioManager>().Play("JumpImpact");
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
