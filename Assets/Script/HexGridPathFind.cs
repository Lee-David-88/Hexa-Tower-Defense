using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class HexGridPathFind : MonoBehaviour
{
    BakeMap map;
    PathRequestManager requestManager;

    private int weight = 1000;

    void Awake()
    {
        map = GetComponent<BakeMap>();
        requestManager = GetComponent<PathRequestManager>();
    }

    private void Start()
    {
    }

    public void StartFindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector2Int startPos, Vector2Int targetPos)
    {   
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        HexcellAttribute startNode = new HexcellAttribute();
        HexcellAttribute targetNode = new HexcellAttribute();

        Tuple<Vector2Int, bool>[] wayPoints = new Tuple<Vector2Int, bool>[0];
        bool pathSucess = false;

        startNode.axialCoordinate = startPos;
        targetNode.axialCoordinate = targetPos;

        List<HexcellAttribute> openSet = new List<HexcellAttribute>();
        HashSet<HexcellAttribute> closeSet = new HashSet<HexcellAttribute>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            HexcellAttribute currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode.axialCoordinate == targetPos)
            {
                //sw.Stop();
                //print("Path Found: " + sw.ElapsedMilliseconds + "ms");
                pathSucess = true;
                targetNode = currentNode;
                break;
            }

            foreach (HexcellAttribute neighbour in map.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour;

                if (neighbour.hasTurret)
                {
                    newMovementCostToNeighbour = currentNode.gCost + (AxialDistance(currentNode, neighbour) * weight);
                } else
                {
                    newMovementCostToNeighbour = currentNode.gCost + AxialDistance(currentNode, neighbour);
                }

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = AxialDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        yield return null;


        if (pathSucess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishProcessingPath(wayPoints, pathSucess);
    }

    public Tuple<Vector2Int, bool>[] RetracePath(HexcellAttribute startNode, HexcellAttribute targetNode)
    {
        List<HexcellAttribute> path = new List<HexcellAttribute>();
        HexcellAttribute currentNode = targetNode;

        while (currentNode.axialCoordinate != startNode.axialCoordinate)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Tuple<Vector2Int, bool>[] wayPoints = new Tuple<Vector2Int, bool>[path.Count];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Tuple<Vector2Int, bool>(path[i].axialCoordinate, path[i].hasTurret);
        }

        Array.Reverse(wayPoints);

        return wayPoints;
    }


    public Vector2Int AxialSubtract(HexcellAttribute a, HexcellAttribute b)
    {
        return new Vector2Int(a.axialCoordinate.x - b.axialCoordinate.x, a.axialCoordinate.y - b.axialCoordinate.y);
    }

    public int AxialDistance(HexcellAttribute a, HexcellAttribute b)
    {
        Vector2Int v2 = AxialSubtract(a, b);
        return (Mathf.Abs(v2.x) + Mathf.Abs(v2.x + v2.y) + Mathf.Abs(v2.y)) / 2;
    }
}
