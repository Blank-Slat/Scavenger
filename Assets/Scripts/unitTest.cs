using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class unitTest : MonoBehaviour
{
    public GameObject walkableTileIndicator;
    public GameObject pathCircle;
    private List<GameObject> walkableTiles = new List<GameObject>();
    private List<GameObject> pathCircles = new List<GameObject>();

    public bool findPath = false;
    public bool checkWalk = false;

    public bool move = false;
    private bool moving = false;
    public float stepTime;
    private int currentWalkIndex = 0;

    public Transform target;
    public Tilemap floor;
    [Range(0, 50)]
    public int movement;

    private pathfindData pd;
    void Update()
    {
        target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (moving)
        {
            if (findPath)
            {
                pd = pathfind.path(transform.position, target.position, movement);
                setPath();
            }
            if (checkWalk)
            {
                checkWalk = false;
                walkableCheck();
            }
            if (move)
            {
                move = false;
            }
        }
    }

    void startWalk()
    {
        currentWalkIndex = pd.pathPoints.Count - 1;
        Invoke("walkStep", stepTime);
    }

    void walkStep()
    {
        if(currentWalkIndex > 0)
        {

            currentWalkIndex--;
            Invoke("walkStep", stepTime);
        }
    }

    void setPath()
    {
        foreach (GameObject g in pathCircles)
        {
            Destroy(g);
        }
        pathCircles = new List<GameObject>();
        foreach (Vector2 v in pd.pathPoints)
        {
            GameObject walkTile = Instantiate(pathCircle);
            walkTile.transform.position = v;
            pathCircles.Add(walkTile);
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
