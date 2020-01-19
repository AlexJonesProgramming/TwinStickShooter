using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip sound;
    public string origin; //the tag of the object spawning the bullet
    private Rigidbody2D rb;
    public int power = 34;
    public int speed;
    public float fireRate = 0.5f;
    public GameObject explosion;

    public bool ShotGun = false;
    public float angle;
    public int pellets;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        

        if (ShotGun)
        {
            float changeInAngle = angle / (pellets - pellets % 2);
            int step = (pellets - pellets % 2) / 2;


            for (int i = 1; i <= step; i++)
            {
                GameObject GO = GameObject.Instantiate(gameObject);
                GO.GetComponent<Bullet>().ShotGun = false;
                GO.GetComponent<Bullet>().origin = origin;
                GO.transform.position = transform.position;

                if(Vector2.Angle(Vector2.down, rb.velocity) > 90) // player is shooting up
                    GO.GetComponent<Rigidbody2D>().velocity = Vector2FromAngle(Vector2.Angle(Vector2.right, rb.velocity) - changeInAngle * i) * speed;
                else
                    GO.GetComponent<Rigidbody2D>().velocity = Vector2FromAngle(-Vector2.Angle(Vector2.right, rb.velocity) - changeInAngle * i) * speed;
                Destroy(GO, 3);
            }

            for (int i = 1; i <= step; i++)
            {
                GameObject GO = GameObject.Instantiate(gameObject);
                GO.GetComponent<Bullet>().ShotGun = false;
                GO.GetComponent<Bullet>().origin = origin;
                GO.transform.position = transform.position;
                if (Vector2.Angle(Vector2.down, rb.velocity) > 90) // player is shooting up
                    GO.GetComponent<Rigidbody2D>().velocity = Vector2FromAngle(Vector2.Angle(Vector2.right, rb.velocity) + changeInAngle * i) * speed;
                else
                    GO.GetComponent<Rigidbody2D>().velocity = Vector2FromAngle(-Vector2.Angle(Vector2.right, rb.velocity) + changeInAngle * i) * speed;
                Destroy(GO, 3);
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (origin == "Player")
        {
            if (collision.tag == "Player") { } //do nothing
            else if (collision.tag == "Obstacle")
            {
                Destroy(this.gameObject);
                Explode();
            }
            else if (collision.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(power);
                Explode();
                Destroy(this.gameObject);
            }
            else if (collision.tag == "Bullet")
            { }
            else
                Debug.Log("Error with bullet collision, Unknown object : " + collision.gameObject.name);
        }

        else if (origin == "Enemy")
        {
            if (collision.tag == "Enemy") { } //do nothing
            else if (collision.tag == "Obstacle")
            {
                Destroy(this.gameObject);
                Explode();
            }
            else if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerMovement>().TakeDamage(rb.velocity, power);
                Explode();
                Destroy(this.gameObject);
            }
            else if (collision.tag == "Bullet")
            { }
            else
                Debug.Log("Error with bullet collision, Unknown object : " + collision.gameObject.name);
        }
    }

    private void Explode()
    {
        GameObject GO = GameObject.Instantiate(explosion);
        GO.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        Destroy(GO, GO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }
}
