using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject bullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Player")
       {
            collision.GetComponent<PlayerMovement>().UpdateWeapon(bullet);
       }
    }
}
