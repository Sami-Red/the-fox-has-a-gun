using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;
    public float moveSpeed, waitAtPoint, jumpForce;
    private float waitCounter;

    public Rigidbody2D RB;
    public Animator anim;
    
    //---={Ground Checks}=---//
    public Transform GroundCheck;
    private bool isOnGround;
    public LayerMask WhatIsGround;

    //---={Bot State related}=---//
    public float shootingRadius, firingRate, bulletSpeed;
    public Transform firingPoint;
    public GameObject bullet;

    private float firingCooldown;
    private State currentState;
    public Transform player;

    //---={Bot State displayer}=---//
    public TextMeshProUGUI display;
    public int botID;
    public static int IDCounter = 0;
    private EnemyHealthController healthController;
    private GameObject canvas;

    

    void Start()
    {
        currentState = State.Patrol; // setting bot to patrol by default;
        //player = PlayerHealthController.instance.transform; 

        healthController = GetComponent<EnemyHealthController>();
        healthController.botID = botID;

        botID = ++IDCounter;

        waitCounter = waitAtPoint;

        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }

        healthController = GetComponent<EnemyHealthController>();
        canvas = GameObject.Find("Enemy");

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
            case State.Patrol:
                PatrolState();
                break;
            case State.Idle:
                IdleState();
                break;
            case State.Shooting:
                ShootingState();
                break;
        }

        //---={Ground Checks}=---//
        isOnGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, WhatIsGround);
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > 0.2f)
        {
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, -1f);
            }
            else
            {
                RB.velocity = new Vector2(-moveSpeed, RB.velocity.y);
                transform.localScale = Vector3.one;
            }
            //--={Jump}=--//
            if (transform.position.y < patrolPoints[currentPoint].position.y - 0.5f && RB.velocity.y < 0.1f && isOnGround)
            {
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            }
        }
        else
        {
            RB.velocity = new Vector2(0f, RB.velocity.y);

            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoint;

                currentPoint++;

                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }

            }
        }
        anim.SetFloat("speed", Mathf.Abs(RB.velocity.x));

        if (display != null)
        {
            display.text = "Patrol ID: " + botID + "\nState: " + currentState + "\nHP: " + healthController.health;
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
        if(Vector3.Distance(transform.position, player.position) <= shootingRadius)
        {
            currentState = State.Shooting;
        }
    }
    void ShootingState()
    {
        if(firingCooldown <= 0)
        {
            AttackPlayer();
            firingCooldown = firingRate;
        }
        else { firingCooldown -= Time.deltaTime;}

        if(Vector3.Distance(transform.position, player.position) > shootingRadius)
        {
            currentState = State.Patrol;
        }
    }
    void PatrolState()
    {
        if (Vector3.Distance(transform.position, player.position) <= shootingRadius)
        {
            currentState = State.Shooting;
        }
    }
    void AttackPlayer()
    {
        if (bullet != null && firingPoint != null)
        {
            Vector2 dir = player.position - firingPoint.position;
            dir.Normalize();
            GameObject bulletInstance = Instantiate(bullet, firingPoint.position, Quaternion.identity);

            EnemyBulletController bulletController = bulletInstance.GetComponent<EnemyBulletController>();
            if (bulletController != null)
            {
                bulletController.SDirection(dir);
            }
        }
    }

}
public enum State
{
    Idle,
    Patrol,
    Shooting
}