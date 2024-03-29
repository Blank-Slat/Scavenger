using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class node
{
    public node parentNode;
    public Vector2 pos;
    public int g = 0;
    public int h = 0;
    public int f = 0;
}

public class pathfindData 
{
    public bool withinRange = false;
    public List<Vector2> walkablePoints = new List<Vector2>();
    public List<Vector2> pathPoints = new List<Vector2>();
    public Vector2 endPoint;
    public float stepsTaken;
}

public class pathfind
{
    public static Vector3[] adjacentPoints;

    public static pathfindData path(Vector3 startPos, Vector3 endPos, float movement)
    {
        bool endReached = false;
        node endNode = new node();
        node currentNode = new node();
        currentNode.pos = startPos;

        List<node> openNodes = new List<node>();
        List<node> closedNodes = new List<node>();

        openNodes.Add(currentNode);
        int c = 0;

        node startNode = currentNode;

        while (openNodes.Count > 0)
        {
            c++;
            if(c > 1000)
            {
                Debug.Log("Overflow");
                break;
            }
            int lowestF = 10000;
            foreach(node n in openNodes)
            {
                if(n.f < lowestF)
                {
                    currentNode = n;
                    lowestF = n.f;
                }
            }
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if(Vector2.Distance(currentNode.pos, endPos) < 0.5f)
            {
                Debug.DrawLine(endPos, endPos + new Vector3(0, 0.1f, 0));
                endNode = currentNode;
                endReached = true;
            }
            foreach(Vector3 v in adjacentPoints)
            {
                Vector2 v2 = currentNode.pos + new Vector2(v.x, v.y);
                bool inClosed = false;
                bool blocked = false;
                RaycastHit2D[] collisions = Physics2D.LinecastAll(v2, currentNode.pos);
                if(currentNode == startNode)
                {
                    if(collisions.Length > 1)
                    {
                        Debug.Log(collisions.Length);
                        blocked = true;
                    }
                }
                else if(collisions.Length > 0)
                {
                    Debug.Log(collisions.Length);
                    blocked = true;
                }
                if (!blocked)
                {
                    foreach (node n in closedNodes)
                    {
                        if (n.pos == v2)
                        {
                            inClosed = true;
                            break;
                        }
                    }
                    if (!inClosed)
                    {

                        node newNode = new node();
                        newNode.pos = v2;
                        newNode.g = currentNode.g + Mathf.RoundToInt(v.z);
                        if (newNode.g <= movement * 10)
                        {
                            bool inOpen = false;
                            bool shorter = false;
                            foreach (node n in openNodes)
                            {
                                if (n.pos == v2)
                                {
                                    inOpen = true;
                                    if (newNode.g < n.g)
                                    {
                                        shorter = true;
                                        openNodes.Remove(n);
                                    }
                                    break;
                                }
                            }
                            if (!inOpen || shorter)
                            {
                                newNode.h = Mathf.RoundToInt(Vector2.Distance(v2, new Vector2(endPos.x, endPos.y)));
                                newNode.f = newNode.g + newNode.h;
                                newNode.parentNode = currentNode;
                                openNodes.Add(newNode);
                            }
                        }
                    }
                }
            }
        }

        closedNodes.Remove(startNode);

        pathfindData pD = new pathfindData();

        foreach (node n in closedNodes)
        {
            Debug.DrawLine(n.pos, n.pos + new Vector2(0, .1f));
            pD.walkablePoints.Add(n.pos);
        }
        pD.withinRange = endReached;
        if (endReached)
        {
            pD.endPoint = endNode.pos;
            pD.stepsTaken = endNode.g * 0.1f;
            currentNode = endNode;
            while (currentNode.parentNode != null)
            {
                Debug.DrawLine(currentNode.pos, currentNode.parentNode.pos);
                pD.pathPoints.Add(currentNode.pos);
                currentNode = currentNode.parentNode;
            }
        }
        else
        {
            pD.endPoint = startPos;
        }

        return pD;
    }
}
