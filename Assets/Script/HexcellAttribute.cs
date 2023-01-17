using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexcellAttribute
{
    public bool walkable;
    public bool hasTurret;
    public bool buildable;
    public Vector2Int axialCoordinate;
    public Vector2 offsetCoordinate;

    public int gCost;
    public int hCost;

    public HexcellAttribute parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
