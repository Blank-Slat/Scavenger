using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathSet : MonoBehaviour
{
    public Vector3[] adjacentPoints;

    private void Start()
    {
        pathfind.adjacentPoints = adjacentPoints;
    }

    private void Update()
    {
        pathfind.adjacentPoints = adjacentPoints;
    }
}
