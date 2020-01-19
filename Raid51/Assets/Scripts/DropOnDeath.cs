using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject droppedItem;

    private void OnDestroy()
    {
        GameObject GO = GameObject.Instantiate(droppedItem);
        GO.transform.position = this.transform.position;
    }
}
