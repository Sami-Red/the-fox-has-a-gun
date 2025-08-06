using UnityEngine;
using TMPro;

public class EnemyFlyingController : MonoBehaviour
{
    public float rangeToChase, moveSpeed;
    private bool chasing;
    private Transform player;
    public Animator anim;
    public Rigidbody2D theRB;
    public TextMeshProUGUI display;
    public int botID;
    public static int IDCounter = 1;
    private EnemyHealthController healthController;
    private GameObject canvas;
    //---={Bot State related}=---//
    public float firingRate, bulletSpeed;
    public Transform firingPoint;
    public GameObject bullet;
    public float hoverTime = 2f;

    private float firingCooldown;
    private ChaserState currentState;
    private float hoverTimer;
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        healthController = GetComponent<EnemyHealthController>();
        canvas = GameObject.Find("Enemy");

        botID = ++IDCounter;


        if (canvas != null && display != null)
        {
            display.transform.SetParent(canvas.transform);
        }
    }

    void Update()
    {
        //---={States}=---//
        switch (currentState)
        {
            case ChaserState.Chasing:
                ChasingState();
                break;
            case ChaserState.Idle:
                IdleState();
                break;
            case ChaserState.Retreat:
                RetreatState();
                break;
        }

        if (display != null)
        {
            display.text = "Chaser ID: " + botID + "\nState: " + currentState + "\nHP: " + healthController.health;
            display.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        }

        if (healthController.health <= 0 && display != null)
        {
            display.transform.SetParent(null);
            Destroy(display.gameObject);
        }
    }
    void IdleState()
    {
        anim.SetBool("chasing", false);
        if (Vector3.Distance(transform.position, player.position) <= rangeToChase)
        {
            currentState = ChaserState.Chasing;
        }
    }
    void ChasingState()
    {
        anim.SetBool("chasing", true); 

        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Facing right
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Facing left
        }

        if (Vector3.Distance(transform.position, player.position) > rangeToChase)
        {
            currentState = ChaserState.Retreat;
        }
    }

    void RetreatState()
    {
        anim.SetBool("chasing", true); // Play flying animation

        float horiDir = transform.position.x > player.position.x ? 1f : -1f;

        transform.position += new Vector3(horiDir * moveSpeed * Time.deltaTime, 0f, 0f);

        if (horiDir > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Facing right
        }
        else if (horiDir < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Facing left
        }

        if (Vector3.Distance(transform.position, player.position) > rangeToChase)
        {
            hoverTimer += Time.deltaTime;

            if (hoverTimer >= hoverTime)
            {
                hoverTimer = 0f; // Reset hover timer
                currentState = ChaserState.Idle;
            }
        }
    }

}
public enum ChaserState
{
    Idle,
    Chasing,
    Retreat
}
