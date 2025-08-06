using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D theRB;
    public int damageAmount;
    public GameObject impactEffect;


    public void SDirection(Vector2 dir)
    {
        theRB.velocity = dir * moveSpeed;
    }
    void Start()
    {
        Vector3 direction = PlayerHealthController.instance.transform.position - transform.position;
        direction.Normalize();

    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }

    }
}
/* OLD doesnt work how i want = doesnt move at all...
    public float bulletSpeed = 5f;
    private Vector2 moveDir;
    public Rigidbody2D theRB;

    public void SDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
        if (theRB != null)
        {
            theRB.velocity = moveDir * bulletSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamagePlayer(damageAmount);
        Destroy(gameObject);
    }
    void Update()
    {
 
        //transform.position += (Vector3)moveDir * bulletSpeed; // Move to the right

        //theRB.velocity = moveDir * bulletSpeed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
 */