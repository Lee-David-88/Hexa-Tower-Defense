using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexGridDrawer : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private TextMeshProUGUI cellLabelPrefab;
    Canvas gridCanvas;

    public void DrawHexCoordinateInWorldPosition()
    {
        gridCanvas = GetComponentInChildren<Canvas>();

        if (transform.Find("CoordinateDisplay"))
        {
            while (transform.Find("CoordinateDisplay").childCount != 0)
            {
                DestroyImmediate(transform.Find("CoordinateDisplay").GetChild(0).gameObject);
            }
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {   
                // From the offset coordinate to world position
                Vector3 pos = new Vector3((x + y * 0.5f - y / 2) * (0.866025404f * 2f), -y * (1f * 1.5f), 0);

                // Convert offset coordinates to axial coordinate
                Vector2 axialCoord = new Vector2(x - y / 2, y);

                // Instantiate coordinate text on to scene
                TextMeshProUGUI label = Instantiate(cellLabelPrefab);
                label.rectTransform.SetParent(gridCanvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);
                label.text = axialCoord.ToString();
            }
        }
    }
}
