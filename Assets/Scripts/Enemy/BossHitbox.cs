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
    }
    void FixedUpdate()
    {
    //update the mesh collider
        // SkinMesh.BakeMesh(bakedMesh);
        //collider.sharedMesh = null;
        // collider.sharedMesh = bakedMesh;
    }

    public override void TakeDamage(float damage)
    { 
        //if (health <= 0) return;
        
        Me.TakeDamage(damage*damageRatio);
    }
}
