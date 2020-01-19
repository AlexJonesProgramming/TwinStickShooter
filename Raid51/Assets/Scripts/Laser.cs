using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public enum LaserType { Red, Green, Blue };
    public LaserType type;

    private SpriteRenderer beam; //the laser beam
    private BoxCollider2D trigger;

    // For Green
    private float timer = 0.0f;
    public float blinkTime = 2.0f;
    private bool beamActive = true;

    // For Blue
    private PlayerMovement player;
    private bool playerInBeam;
    private List<GameObject> objectsInBeam;

    void Start()
    {
        beam = GetComponent<SpriteRenderer>();
        trigger = GetComponent<BoxCollider2D>();
        objectsInBeam = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (type == LaserType.Green)
        {
            timer += Time.deltaTime;
            if (timer >= blinkTime)
            {
                timer = 0.0f;
                beamActive = !beamActive;
                beam.enabled = beamActive;
                trigger.enabled = beamActive;
            }
        }
        else if (type == LaserType.Blue)
        {
            if (playerInBeam)
                player.TakeDamage(Vector2.zero, 1, false);

            foreach (GameObject GO in objectsInBeam)
            {
                GO.GetComponent<Enemy>().TakeDamage(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (type == LaserType.Green)
            {
                collision.GetComponent<PlayerMovement>().TakeDamage(Vector2.zero, 100, false);
            }
            else if (type == LaserType.Red)
            {
                collision.GetComponent<PlayerMovement>().TakeDamage(Vector2.zero, 100, false);
            }
            else if (type == LaserType.Blue)
            {
                playerInBeam = true;
            }
        }

        if (collision.tag == "Enemy")
        {
            if (type == LaserType.Green)
            {
                collision.GetComponent<Enemy>().TakeDamage(100);
            }
            else if (type == LaserType.Red)
            {
                collision.GetComponent<Enemy>().TakeDamage(100);
            }
            else if (type == LaserType.Blue)
            {
                objectsInBeam.Add(collision.gameObject);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == LaserType.Blue)
        {
            if (collision.tag == "Enemy")
            {
               objectsInBeam.Remove(collision.gameObject);
            }
            else if (collision.tag == "Player")
            {
                playerInBeam = false;
            }
        }
    }
}
