using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarHelper : MonoBehaviour
{
    public TextAsset graphData;
    void Start()
    {
        AstarPath.active.data.DeserializeGraphs(graphData.bytes);
    }

    private void Update()
    {
        //AstarPath.active.Scan();
    }
}
