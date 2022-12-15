using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : BossHitbox
{
    //[SerializeField] private MeshFilter meshFilter;
    [SerializeField] private bool isGlow = false;
    [SerializeField] private bool isImmune = false;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private GameObject aura;
    
    // Start is called before the first frame update
    public override void Awake() {
        health = maxHealth;
        meshFilter = GetComponent<MeshFilter>();

        if (SkinMesh != null) SetMesh();
    }
    void Start()
    {
        bakedMesh = new Mesh();
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
        SkinMesh = GetComponent<SkinnedMeshRenderer>();
        collider = GetComponent<MeshCollider>();
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
    }
    // void Update() {
    //     //Glow(open);
    //     Check();
    //     //Debug.Log("OPEN" + open);
    // }
    
    public override void TakeDamage(float damage)
    { 
        
        if (!isImmune) health -= damage*damageRatio;
        // Me.TakeDamage(damage*damageRatio*0.1f);
    }

    // Update is called once per frame
    public void Glow()
    {
        aura = Instantiate(auraPrefab, transform.position, Quaternion.identity, transform);
        // if (open) {
        //     //Debug.Log(aura.transform.position);
        //     //Destroy(aura,2f);
        //     //isGlow = true;
        // }
        // if (!open)
        // {
        //     Destroy(aura);
        //     isGlow = false;
        // }
    }

    public void Destroyaura() {
        Destroy(aura);
    }

    public void SetMesh()
    {
        if (SkinMesh.sharedMesh != null)
        {
            meshFilter.mesh = SkinMesh.sharedMesh;
        }
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }


    public void SetIsImmune(bool im) {
        isImmune = im;
    } 
}
