using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeMap : MonoBehaviour
{
    private Transform GeneratedMapTransfrom;
    private List<GameObject> HexcellObjects = new List<GameObject>();
    public List<HexcellAttribute> AxialCoordinateMap = new List<HexcellAttribute>();

    public static readonly Vector2Int[] axialDirectionVectors = { new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
                                                                  new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) };

    void Awake()
    {
        GeneratedMapTransfrom = transform.Find("Generated Map");
        //BakeMapFromScene();
    }

    // Start is called before the first frame update 
    void Start()
    {
        BakeMapFromScene();
    }

    void BakeMapFromScene()
    {
        if (GeneratedMapTransfrom)
        {
            int childCounter = 0;
            while (GeneratedMapTransfrom.childCount > childCounter)
            { 
                HexcellObjects.Add(GeneratedMapTransfrom.GetChild(childCounter).gameObject);
                childCounter++;
            }
        }

        if (HexcellObjects.Count > 0)
        {
            foreach (GameObject hexcells in HexcellObjects)
            {
                AxialCoordinateMap.Add(hexcells.GetComponent<Hexcell>().attributes);
            }
        }
    }

    public List<HexcellAttribute> GetNeighbours(HexcellAttribute node)
    {
        const int numberOfNeighbourInHex = 6;

        // Create a List of HexCell to Store the Neighbours
        List<HexcellAttribute> neighbours = new List<HexcellAttribute>();

        for (int i = 0; i < numberOfNeighbourInHex; i++)
        {
            HexcellAttribute currentNeighbour = new HexcellAttribute();
            currentNeighbour.axialCoordinate = AxialAddition(node.axialCoordinate, AxialDirection(i));

            // Loop through the entire grid to check if the newly found neighbour exist in the grid
            // If exist add it to the neighbours list
            for (int j = 0; j < AxialCoordinateMap.Count; j++)
            {
                if (AxialCoordinateMap[j].axialCoordinate == currentNeighbour.axialCoordinate)
                {
                    neighbours.Add(AxialCoordinateMap[j]);
                }
            }
        }

        return neighbours;
    }

    public Vector2Int AxialDirection(int direction)
    {
        return axialDirectionVectors[direction];
    }

    public Vector2Int AxialAddition(Vector2Int coord, Vector2Int direction)
    {
        return new Vector2Int(coord.x + direction.x, coord.y + direction.y);
    }
}
