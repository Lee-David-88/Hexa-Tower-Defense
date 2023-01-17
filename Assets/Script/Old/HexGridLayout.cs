using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vector Storage, Axial Constructor
public struct HexCoord
{
    [SerializeField] public int q;
    [SerializeField] public int r;
    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
}

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Setting")]
    public Vector2Int gridSize;

    [Header("Tile Setting")]
    [SerializeField] private float outerSize = 1f;
    [SerializeField] private float innerSize;
    [SerializeField] private bool isflatTopped;
    [SerializeField] private Color hoverColor;
    [SerializeField] public Material material;


    private void LayoutGrid()
    {   
        if (transform.childCount != 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x}, {y}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isflatTopped;
                hexRenderer.innerSize = innerSize;
                hexRenderer.outerSize = outerSize;
                hexRenderer.hoverColor = hoverColor;
                hexRenderer.SetMaterial(material);
                hexRenderer.DrawMesh();

                tile.transform.SetParent(transform, true);
            }
        }
    }

    private Vector2 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isflatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * horizontalDistance) + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = (shouldOffset) ? height / 2 : 0;

            xPosition = column * horizontalDistance;
            yPosition = (row * verticalDistance) - offset;
        }

        return new Vector2(xPosition, -yPosition);
    }

    private void OnEnable()
    {
        LayoutGrid();
    }

    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            LayoutGrid();
        }
    }
}
