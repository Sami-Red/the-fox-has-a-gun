using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRB;
    public int damage;
    public Vector2 moveDir;
    public GameObject impactEffect;
    
    void Update()
    {
        theRB.velocity = moveDir * bulletSpeed;
        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-3f, 3f, 3f);
        }
        else if (theRB.velocity.x > 0)
        {
            transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
