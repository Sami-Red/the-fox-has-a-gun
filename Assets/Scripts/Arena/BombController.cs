using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeToExplode = 0.5f;
    public float explosionRadius = 3f;
    public int damageAmount = 10;
    public LayerMask damageableLayers;
    public GameObject explosion;
    public SpriteRenderer bombSprite;
    public Animator anim;
    private bool timerStarted = false;

    void Update()
    {
        if(!timerStarted)
        {
            timerStarted = true;
            anim.SetBool("timerStarted", true);
        }
        timeToExplode -= Time.deltaTime;
        
        if (timeToExplode <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Detect nearby objects
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayers);

        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealthController.instance.DamagePlayer(damageAmount);
            }
            else if (hit.GetComponent<EnemyHealthController>() != null)
            {
                hit.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            }
        }
        anim.SetBool("timerStarted", false);
        // Spawn explosion effect
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}