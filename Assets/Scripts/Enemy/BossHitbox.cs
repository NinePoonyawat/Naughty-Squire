using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : HitableObject
{
    [SerializeField] protected EnemyBossHealth Me;
    [SerializeField] protected MeshFilter meshFilter;
    [SerializeField] protected MeshCollider collider;
    public SkinnedMeshRenderer SkinMesh;
    public Mesh bakedMesh;
    public bool CanStun;
    // Start is called before the first frame update
    void Start()
    {
        bakedMesh = new Mesh();
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
        SkinMesh = GetComponent<SkinnedMeshRenderer>();
        collider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    void FixedUpdate()
    {
    //update the mesh collider
        // SkinMesh.BakeMesh(bakedMesh);
        //collider.sharedMesh = null;
        // collider.sharedMesh = bakedMesh;
    }

    protected void Check() {
        if (CanStun && health <= 0) {
            Me.SetStun();
            if (!canDestroy)health = maxHealth;
        }   
    }

    public override void TakeDamage(float damage)
    { 
        //if (health <= 0) return;
        
        Me.TakeDamage(damage*damageRatio);
    }
}
