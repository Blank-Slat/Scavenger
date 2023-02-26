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
    private int currentMovement;

    public pathfindData pd;
    private void Start()
    {
        setData();
    }
    void Update()
    {
        target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!moving)
        {
            if (Input.GetMouseButtonDown(0) && pd.withinRange)
            {
                move = true;
            }
            if (findPath)
            {
                pd = pathfind.path(transform.position, target.position, currentMovement);
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
                startWalk();
            }
        }
    }
    void setData()
    {
        Debug.Log("Set data");
        currentMovement = movement;
    }

    void startWalk()
    {
        moving = true;
        currentMovement -= pd.pathPoints.Count;
        currentWalkIndex = pd.pathPoints.Count - 1;
        transform.position = pd.pathPoints[currentWalkIndex];
        Invoke("walkStep", stepTime);
    }

    void walkStep()
    {
        currentWalkIndex--;
        if (currentWalkIndex >= 0)
        {
            transform.position = pd.pathPoints[currentWalkIndex];
            Invoke("walkStep", stepTime);
        }
        else
        {
            moving = false;
            checkWalk = true;
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
