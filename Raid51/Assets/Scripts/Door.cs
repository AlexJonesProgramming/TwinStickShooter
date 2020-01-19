using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Door : MonoBehaviour
{

    public GameObject closedDoor;
    public GameObject openDoor;

    public GameObject cardReaderOff;
    public GameObject cardReaderOn;

    public int keyNumber;

    public bool isLaser = false;
    public GameObject laserBeam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().hasKeyCard(keyNumber))
            {
                cardReaderOff.SetActive(false);
                cardReaderOn.SetActive(true);
                if (isLaser)
                {
                    laserBeam.SetActive(false);
                }
                else
                {
                    openDoor.SetActive(true);
                    closedDoor.SetActive(false);
                }
                
                Destroy(this.gameObject);
            }
        }
    }
}
