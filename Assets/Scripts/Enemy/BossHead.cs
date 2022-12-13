using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : BossHitbox
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private bool isGlow = false;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private GameObject aura;
    public SkinnedMeshRenderer HeadMesh;
    public bool open;
    // Start is called before the first frame update
    public override void Awake() {
        health = maxHealth;
        meshFilter = GetComponent<MeshFilter>();

        if (HeadMesh != null) SetMesh();
    }
    void Start()
    {
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
        damageRatio = 0;
    }
    void Update() {
        Glow(open);
        Check();
        //Debug.Log("OPEN" + open);
    }
    void Check() {
        if (health <= 0) {
            Me.SetStun();
            health = maxHealth;
        }   
    }
    public override void TakeDamage(float damage)
    { 
        
        health -= damage*damageRatio;
        Me.TakeDamage(damage*damageRatio*0.7f);
    }

    // Update is called once per frame
    public void Glow(bool open)
    {
        if (open) {
            aura = Instantiate(auraPrefab, transform.position, Quaternion.identity, transform);
            Debug.Log("aura");
            isGlow = true;
        }
        if (!open)
        {
            Destroy(aura);
            isGlow = false;
        }
    }

    public void SetMesh()
    {
        if (HeadMesh.sharedMesh != null)
        {
            meshFilter.mesh = HeadMesh.sharedMesh;
        }
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }
    public void SetOpen() {
        open = true;
        damageRatio = 1;
    }
}
