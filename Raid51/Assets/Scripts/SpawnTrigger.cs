using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

    public GameObject SpawnGroup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpawnGroup.SetActive(true);

            collision.GetComponent<PlayerMovement>().hitCheckpoint(collision.transform);

            Destroy(this.gameObject);
        }
    }
}
