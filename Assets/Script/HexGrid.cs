using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{

    public const float outerRadius = 1f;

    public const float innerRadius = outerRadius * 0.866025404f;
}

public class HexGrid
{
    public static readonly Vector2Int[] axialDirectionVectors = { new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
                                                           new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) };

    private int width;
    private int height;
    private HexCellOld[] cells;

    public HexGrid(int _width, int _height, HexCellOld[] _cells)
    {
        width = _width;
        height = _height;
        cells = _cells;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3((x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f), -y * (HexMetrics.outerRadius * 1.5f), 0);
    }

    public Vector2Int AxialDirection(int direction)
    {
        return axialDirectionVectors[direction];
    }

    public HexCoordinates AxialAddition(HexCoordinates coord, Vector2Int direction)
    {
        return new HexCoordinates(coord.X + direction.x, coord.Y + direction.y);
    }

    public List<HexCellOld> GetNeighbours(HexCellOld node)
    {   
        const int numberOfNeighbourInHex = 6;

        // Create a List of HexCell to Store the Neighbours
        List<HexCellOld> neighbours = new List<HexCellOld>();

        for (int i = 0; i < numberOfNeighbourInHex; i++)
        {
            HexCellOld currentNeighbour = new HexCellOld();
            currentNeighbour.coordinates = AxialAddition(node.coordinates, AxialDirection(i));

            // Loop through the entire grid to check if the newly found neighbour exist in the grid
            // If exist add it to the neighbours list
            for (int j = 0; j < cells.Length; j++)
            {
                if (HexCoordinates.isEqual(cells[j].coordinates, currentNeighbour.coordinates))
                {
                    neighbours.Add(currentNeighbour);
                }
            }
        }

        return neighbours;
    }
}
