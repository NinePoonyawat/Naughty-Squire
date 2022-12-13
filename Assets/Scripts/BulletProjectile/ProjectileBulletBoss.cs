using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletBoss : MonoBehaviour
{
    private float damage;
    private Rigidbody bulletRigidBody;
    public float speed;
    [SerializeField] private GameObject effectPrefab;
    public GameObject player;
    public int ShootAngle;
    public AnimationCurve MoveCurve;
    // Start is called before the first frame update
    void Awake() {
        bulletRigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //CreateBullet();
        StartCoroutine(BulletCurve());
        damage = 20f;
    }
    IEnumerator BulletCurve() {
        float t = 0;
        while (t > 0) {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, MoveCurve.Evaluate(t*speed));
            t += Time.deltaTime * 1;
        }
        yield return null;
    }

    void CreateBullet() {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float sp =  Mathf.Pow(distance*0.98f / Mathf.Sin(2*Mathf.Deg2Rad*ShootAngle),0.5f);
            Debug.Log(new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z) * sp);
            bulletRigidBody.velocity = new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z) * sp;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider hit)
    {
        HitableObject entityHit = hit.GetComponent<HitableObject>();
        Debug.Log(bulletRigidBody.velocity);
        //Debug.Log("this bullet deal " + damage + " to " + entityHit);
        if (entityHit != null)
        {
            entityHit.TakeDamage(damage);
        }
        Instantiate(effectPrefab, transform.position, transform.rotation);
        //FindObjectOfType<AudioManager>().Play("PistolBulletHit");
        Destroy(gameObject);
    }
}
