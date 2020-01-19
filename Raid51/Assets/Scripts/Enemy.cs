using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Enemy : MonoBehaviour
{
    public bool isMeleeType = false;
    public float meleeDistance = 1.0f;
    public int meleeDamage = 10;
    public float meleeTimer = 1.5f; // the time between enemy hits
    private float hitTimer = 0.5f; // the internal timer to track hits

    public int health = 100;

    public GameObject bullet;
    private float bulletTimer = 0.5f;
    public float minFireTime = 0.5f;
    public float maxFireTime = 10;

    AudioSource audioPlayer;

    public Transform player;

    public LayerMask raycastMask;

    // animation
    public AIPath aiPath;
    private int playerDirection = 1;
    private bool moving = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<AIDestinationSetter>().target = player;
        aiPath = GetComponent<AIPath>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Animation stuff



        // player movement
        Vector2 direction = aiPath.desiredVelocity;

        float upAngle = Vector2.Dot(Vector2.up, direction);
        float downAngle = Vector2.Dot(Vector2.down, direction);
        float leftAngle = Vector2.Dot(Vector2.left, direction);
        float rightAngle = Vector2.Dot(Vector2.right, direction);

        float testAngle = 9999;

        // Player animation
        if (direction != Vector2.zero)
        {

            if (upAngle < testAngle)
                testAngle = upAngle;
            if (downAngle < testAngle)
                testAngle = downAngle;
            if (leftAngle < testAngle)
                testAngle = leftAngle;
            if (rightAngle < testAngle)
                testAngle = rightAngle;

            if (upAngle == testAngle)
                playerDirection = 3;
            else if (downAngle == testAngle)
                playerDirection = 1;
            else if (leftAngle == testAngle)
                playerDirection = 2;
            else if (rightAngle == testAngle)
                playerDirection = 4;


            moving = true;
        }
        else
            moving = false;

        animator.SetBool("isWalking", moving);


        //Death
        if (health < 1)
            Destroy(this.gameObject);

        //Weapon stuff 
        if (isMeleeType)
        {
             if (hitTimer <= 0)
            {
                Vector2 playerDirection = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(this.transform.position.x, this.transform.position.y); //Direction towards the player
                if(playerDirection.magnitude < meleeDistance)
                {
                    Debug.Log("Enemy punched the player");
                    player.gameObject.GetComponent<PlayerMovement>().TakeDamage(playerDirection, meleeDamage);
                    hitTimer = meleeTimer;
                }
            }
            else
                hitTimer -= Time.deltaTime;
        }
        else
        {
            if (bulletTimer <= 0) // enemy is allowed to shoot
            {
                Vector2 bulletDirection = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(this.transform.position.x, this.transform.position.y); //Direction towards the player
                RaycastHit2D hit = Physics2D.Raycast(transform.position, bulletDirection, Mathf.Infinity, raycastMask); // test if the enemey can see the player
                if (hit.collider != null)
                {

                    if (hit.collider.tag == "Player") //the enemy does see the player
                    {
                        GameObject GO = GameObject.Instantiate(bullet);
                        Destroy(GO, 3);

                        GO.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                        GO.GetComponent<Bullet>().origin = this.tag;
                        GO.GetComponent<Rigidbody2D>().velocity = bulletDirection.normalized * GO.GetComponent<Bullet>().speed;

                        if (Vector2.Angle(Vector2.right, bulletDirection) < 90) // the player is shooting right
                            GO.transform.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.up, bulletDirection));
                        else
                            GO.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, bulletDirection));

                        audioPlayer.clip = GO.GetComponent<Bullet>().sound;
                        audioPlayer.Play(0);

                        bulletTimer = Random.Range(minFireTime, maxFireTime);
                    }
                }

            }
            else
                bulletTimer -= Time.deltaTime;
        }
    }


    public void LateUpdate()
    {
        animator.SetInteger("WalkingDirection", playerDirection); // this is here to make sure the animation goes into the proper state weh the player stops walking

        if (!moving)
        {
            if (playerDirection == 1)
                animator.Play("LookUp");
            else if (playerDirection == 2)
                animator.Play("LookRight");
            else if (playerDirection == 3)
                animator.Play("LookDown");
            else if (playerDirection == 4)
                animator.Play("LookLeft");
        }
    }

    public void TakeDamage(int ammount)
    {
        health -= ammount;
    }
}
