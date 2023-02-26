using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitData : MonoBehaviour
{
    public unitTest testStuff;
    public bool enemy;
    public int range;
    private SpriteRenderer indicator;
    public GameObject indicatorSetter;
    private static GameObject indicatorObject;
    public LayerMask layerCollide;
    public bool selected;
    private Vector2 checkPos;
    private unitData[] allUnits;

    private void Start()
    {
        if(indicatorObject == null && indicatorSetter != null)
        {
            indicatorObject = indicatorSetter;
        }
        if(indicatorObject != null)
        {
            GameObject newIndicator = Instantiate(indicatorSetter, transform);
            newIndicator.transform.position = transform.position;
            indicator = newIndicator.GetComponent<SpriteRenderer>();
            indicator.enabled = false;
        }
        if(gameObject.GetComponent<unitTest>() != null)
        {
            testStuff = gameObject.GetComponent<unitTest>();
        }
    }
    void Update()
    {
        if (selected)
        {
            rangeCheck();
        }
    }

    void rangeCheck()
    {
        int u = 0;
        checkPos = testStuff.pd.endPoint;
        allUnits = FindObjectsOfType<unitData>();
        foreach (unitData unit in allUnits)
        {
            bool canHit = false;
            if (Vector2.Distance(checkPos, unit.transform.position) <= range && unit.enemy)
            {
                RaycastHit2D rayHit = Physics2D.Raycast(checkPos, new Vector2(unit.transform.position.x, unit.transform.position.y) - checkPos, range, layerCollide);
                if (!rayHit)
                {
                    canHit = true;
                }
            }
            if (canHit)
            {
                u++;
                unit.indicator.enabled = true;
                unit.indicator.color = Color.red;
            }
            else
            {
                unit.indicator.enabled = false;
            }
        }
        //Debug.Log("Enemies in range: " + u);
    }
}
