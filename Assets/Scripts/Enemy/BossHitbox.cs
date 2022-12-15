using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : HitableObject
{
    [SerializeField] protected EnemyBossHealth Me;
    // [SerializeField] protected MeshFilter meshFilter;
    // [SerializeField] protected MeshCollider collider;
    // public SkinnedMeshRenderer SkinMesh;
    // public Mesh bakedMesh;
    public bool CanStun;
    public bool isImmune = false;
    // Start is called before the first frame update
    void Start()
    {
        // bakedMesh = new Mesh();
        // SkinMesh = GetComponent<SkinnedMeshRenderer>();
        // collider = GetComponent<MeshCollider>();
        EnemyBossHealth eb = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
        if (eb != null) {
            Me = eb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    protected void Check() {
        if (CanStun && health <= 0) {
            Me.SetStun();
            if (!canDestroy) health = maxHealth;
            else if (CanStun) Destroy(gameObject);
        }   
    }

    public override void TakeDamage(float damage)
    { 
        
        if (!isImmune) health -= damage*damageRatio;
        if (!CanStun) Me.TakeDamage(damage*damageRatio);
        // Me.TakeDamage(damage*damageRatio*0.1f);
    }

    public void SetCanStun(bool c) {
        CanStun = c;
    }
}
