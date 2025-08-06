using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    //Movement
    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashDuration;
    public int jumps;
    private bool isDashing;
    private float dashTimer;
    private Vector2 dashDirection;
    private bool canDoubleJump;
    //animation
    public Animator anim;
    //GroundChecks
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    // shooting
    public Transform firingPoint;
    public BulletController shotToFire;


    private void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        SpawnLocation();
    }

    private void Update()
    {
        Movement();
        Animation();
        Attack();
    }


    private void Movement()
    {
        // checks if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // checks if not dashing
        if (isDashing)
        {
            return;
        }

        // move side to side 
        theRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y);
        // direction
        if(theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-4f, 4f, 4f);
        } else if (theRB.velocity.x > 0)
        {
            transform.localScale = new Vector3(4f, 4f, 4f);
        }

        // jumps
        if (Input.GetButtonDown("Jump") && (isGrounded || canDoubleJump))
        {
            if (isGrounded)
            {
                canDoubleJump = true;
            } else
            {
                canDoubleJump = false;
            }
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }
        // dashs
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
            dashTimer = dashDuration;
            theRB.velocity = new Vector2(dashDirection.x * dashSpeed, theRB.velocity.y);
            Invoke("EndDash", dashDuration);

        }
    }
    private void EndDash()
    {
        isDashing = false;
    }
    void Animation()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
    void Attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotToFire, firingPoint.position, firingPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
            anim.SetBool("isShooting", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Gem")
        {
            Destroy(other.gameObject);
        }
    }
    public void SpawnLocation()
    {
        float spawnX = PlayerPrefs.GetFloat("SpawnX", transform.position.x);
        float spawnY = PlayerPrefs.GetFloat("SpawnY", transform.position.y);
        float spawnZ = PlayerPrefs.GetFloat("SpawnZ", transform.position.z);

        transform.position = new Vector3(spawnX, spawnY, spawnZ);
    }
}
