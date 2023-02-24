using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class unitTest : MonoBehaviour
{
    public GameObject walkableTileIndicator;
    private List<GameObject> walkableTiles = new List<GameObject>();

    public bool findPath = false;
    public bool checkWalk = false;
    public Transform target;
    public Tilemap floor;
    [Range(0, 50)]
    public int move;

    private pathfindData pd;
    void Update()
    {
        target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (findPath)
        {
            pd = pathfind.path(transform.position, target.position, move);
        }
        if (checkWalk)
        {
            checkWalk = false;
            walkableCheck();
        }
    }

    void walkableCheck()
    {
        foreach (GameObject g in walkableTiles)
        {
            Destroy(g);
        }
        walkableTiles = new List<GameObject>();
        foreach (Vector2 v in pd.walkablePoints)
        {
            GameObject walkTile = Instantiate(walkableTileIndicator);
            walkTile.transform.position = v;
            walkableTiles.Add(walkTile);
        }
    }
}
