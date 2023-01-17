using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform cellPrefab;
    public TextMeshProUGUI cellLabelPrefab;
    [SerializeField] private Vector2Int mapSize;

    [Range(0, 2)]
    [SerializeField] private float outlinePercentage;

    HexCellOld[] cells;

    public HexGrid grid;
    Canvas gridCanvas;

    private void Awake()
    {
/*        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[mapSize.x * mapSize.y];
        grid = new HexGrid(mapSize.x, mapSize.y, cells);*/
    }
    // Start is called before the first frame update
    void Start()
    {
        //GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCellOld[mapSize.x * mapSize.y];
        grid = new HexGrid(mapSize.x, mapSize.y, cells);

        string mapHolderName = "Generated Map";

        if (transform.Find(mapHolderName))
        {
            DestroyImmediate(transform.Find(mapHolderName).gameObject);
        }

        if (transform.Find("CoordinateDisplay"))
        {
            while (transform.Find("CoordinateDisplay").childCount != 0)
            {
                DestroyImmediate(transform.Find("CoordinateDisplay").GetChild(0).gameObject);
            }
        }

        Transform mapHolder = new GameObject(mapHolderName).transform;
        mapHolder.SetParent(transform, false);

        for (int y = 0, i = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2 pos = grid.GetWorldPosition(x, y);

                HexCellOld cell = new HexCellOld();

                // Convert offset coordinates to axial coordinate
                cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
                cells[i] = cell;

                Transform node = Instantiate(cellPrefab);
                node.SetParent(mapHolder, false);
                node.localPosition = pos;
                node.localScale = new Vector3(2, 2, 1) * (1 - outlinePercentage);

                TextMeshProUGUI label = Instantiate(cellLabelPrefab);
                label.rectTransform.SetParent(gridCanvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);
                label.text = cell.coordinates.ToString();

                i++;
            }
        }
    }
}
