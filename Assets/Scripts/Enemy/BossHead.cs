using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : BossHitbox
{
    //[SerializeField] private MeshFilter meshFilter;
    [SerializeField] private bool isGlow = false;
    
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private GameObject aura;
    
    // Start is called before the first frame update
    public override void Awake() {
        health = maxHealth;
        // meshFilter = GetComponent<MeshFilter>();

        // if (SkinMesh != null) SetMesh();
    }
    void Start()
    {
        // bakedMesh = new Mesh();
        // SkinMesh = GetComponent<SkinnedMeshRenderer>();
        // collider = GetComponent<MeshCollider>();
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
        CanStun = false;
    }
    // void Update() {
    //     //Glow(open);
    //     Check();
    //     //Debug.Log("OPEN" + open);
    // }
    
    
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

    // public void SetMesh()
    // {
    //     if (SkinMesh.sharedMesh != null)
    //     {
    //         meshFilter.mesh = SkinMesh.sharedMesh;
    //     }
    //     GetComponent<MeshCollider>().sharedMesh = null;
    //     GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    // }


    public void SetIsImmune(bool im) {
        isImmune = im;
    } 
}
