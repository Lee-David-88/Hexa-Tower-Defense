using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField] private int x, y;

    public int X { get { return x; } }

    public int Y { get { return y; } }

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }
    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ")";
    }

    public static bool isEqual(HexCoordinates left, HexCoordinates right)
    {
        return left.X == right.X && left.Y == right.Y;
    }
}

public class HexCellOld
{
    public HexCoordinates coordinates;

    public int gCost;
    public int hCost;

    public HexCellOld parent;

    public bool walkable = true;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
