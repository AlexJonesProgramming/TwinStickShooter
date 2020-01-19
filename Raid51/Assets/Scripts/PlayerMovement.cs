using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    private Joystick rightJoystick;
    private Joystick leftJoystick;

    //player moevemnt
    public Rigidbody2D player;
    public float speed;
    public Animator animator;
    private bool moving = false;
    private int playerDirection = 1;

    //knockback
    private float knockBackTimer = 0.3f;
    private bool inKnockBack = false;
    
    //healthBar
    public Text healthText;
    public RectTransform healtBar;

    //gun stuff
    public GameObject bullet;
    private float timer = 0.0f;
    private float fireRate = 0.5f;

    //Sounds
    AudioSource audioPlayer;

    //PLayer health
    public int health = 100;

    //Door management;
    public int[] keyCards;

    //Check points
    private Vector3 lastCheckpoint;
    private Vector3 secondToLastCheckpoint;

    public void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        keyCards = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        fireRate = bullet.GetComponent<Bullet>().fireRate;
        DontDestroyOnLoad(this);

        
    }

    public void Update()
    {
        // player movement 
        if (leftJoystick == null || rightJoystick == null)
            ResetComponents();
        else
        {
            Vector2 direction = (Vector2.up * leftJoystick.Vertical + Vector2.right * leftJoystick.Horizontal) * speed;

            float magnitude = Mathf.Clamp(direction.magnitude, 0, 1);


            if (!inKnockBack)
                player.velocity = direction * speed * magnitude; //the magnitude allows for finer movement controll
            else
            {
                if (knockBackTimer <= 0)
                    inKnockBack = false;
                else
                    knockBackTimer -= Time.deltaTime;
            }

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

            // weapon controlls
            Vector2 bulletDirection = (Vector2.up * rightJoystick.Vertical + Vector2.right * rightJoystick.Horizontal); // get the direction
            if (timer < fireRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (bulletDirection != Vector2.zero) // there is a direction
                {
                    GameObject GO = GameObject.Instantiate(bullet);


                    if (Vector2.Angle(Vector2.right, bulletDirection) < 90) // the player is shooting right
                        GO.transform.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.up, bulletDirection));
                    else
                        GO.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, bulletDirection));

                    GO.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);

                    audioPlayer.clip = GO.GetComponent<Bullet>().sound;
                    audioPlayer.Play(0);

                    Destroy(GO, 3);

                    GO.GetComponent<Bullet>().origin = this.tag;

                    GO.GetComponent<Rigidbody2D>().velocity = bulletDirection.normalized * GO.GetComponent<Bullet>().speed;
                    timer = 0;
                }
            }

            //rotate the bullet sprite
            if (bulletDirection != Vector2.zero) //the player is using the right joystick
            {
                upAngle = Vector2.Dot(Vector2.up, bulletDirection);
                downAngle = Vector2.Dot(Vector2.down, bulletDirection);
                leftAngle = Vector2.Dot(Vector2.left, bulletDirection);
                rightAngle = Vector2.Dot(Vector2.right, bulletDirection);


                // Player animation

                testAngle = 9999;
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
            }
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


    public void TakeDamage(Vector2 hitDirection, int ammount, bool useKnockBack = true)
    {
        if (useKnockBack)
        {
            inKnockBack = true;
            knockBackTimer = 0.3f;
            player.AddForce(hitDirection.normalized * (200 + (ammount * 8)));
        }

        health -= ammount;
        UpdateHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Acid")
            health = 0;
        else if (collision.tag == "MedKit")
        {
            if (health < 100)
            {
                health += 50;
                Destroy(collision.gameObject);
            }
            if (health > 100)
                health = 100;

            UpdateHealth();
        }
    }

    private void UpdateHealth()
    {
        if(healthText != null)
            healthText.text = "Health : " + health.ToString();
        if(healtBar != null)
            healtBar.sizeDelta = new Vector2(health * 5.5f, healtBar.sizeDelta.y);
    }

    public bool hasKeyCard(int k)
    {
        for (int i = 0; i < 10; i++)
        {
            if (keyCards[i] == k)
            {
                keyCards[i] = 0;
                return true;
            }
        }

        return false;
    }

    public void addKeyCard(int k)
    {
        for (int i = 0; i < 10; i++)
        {
            if (keyCards[i] == 0)
            {
                keyCards[i] = k;
                break;
            }
        }
    }

    public void UpdateWeapon(GameObject b)
    {
        bullet = b;
        fireRate = b.GetComponent<Bullet>().fireRate;
    }

    private void ResetComponents()
    {
        if (GameObject.FindWithTag("RightJoystick") != null)
        {
            rightJoystick = GameObject.FindWithTag("RightJoystick").GetComponent<Joystick>();
        }
        if (GameObject.FindWithTag("LeftJoystick") != null)
        {
            leftJoystick = GameObject.FindWithTag("LeftJoystick").GetComponent<Joystick>();
        }

        if (GameObject.FindWithTag("HealthText") != null)
        {
            healthText = GameObject.FindWithTag("HealthText").GetComponent<Text>();
        }
        if (GameObject.FindWithTag("HealthBar") != null)
        {
            healtBar = GameObject.FindWithTag("HealthBar").GetComponent<RectTransform>();
        }

        UpdateHealth();

        keyCards = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public void Respawn()
    {

        if (secondToLastCheckpoint != Vector3.zero)
        {
            print("PM");
            this.transform.position = secondToLastCheckpoint;
        }
        else if (lastCheckpoint != Vector3.zero)
        {
            print("PM");
            this.transform.position = lastCheckpoint;
            health = 150;
            UpdateHealth();
        }
        else
        {
            print("PM");
            this.transform.position = GameObject.FindWithTag("PlayerSpawn").transform.position;
            health = 200;
            UpdateHealth();
        }

        Time.timeScale = 1;
    }


    public void ResetCheckpoints()
    {
        secondToLastCheckpoint = Vector3.zero;
        lastCheckpoint = Vector3.zero;
    }

    public void hitCheckpoint(Transform location)
    {
        if (lastCheckpoint == Vector3.zero)
            lastCheckpoint = location.position;
        else
        {
            secondToLastCheckpoint = lastCheckpoint;
            lastCheckpoint = location.position;
        }
    }

}
