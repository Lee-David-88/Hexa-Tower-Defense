using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexcell : MonoBehaviour
{
    public HexcellAttribute attributes;
    [Header("Coordinate Setting")]
    [SerializeField] private Vector2Int axial_Coordinate;
    [SerializeField] private Vector2 offset_Coordinate;

    [SerializeField] public bool walkable;
    [SerializeField] public bool buildable;
    [SerializeField] private bool hasTurret;

    [Header("Colour Setting")]
    [SerializeField] private Color unwalkableColor;
    [SerializeField] private Color unBuildableColor;
    [SerializeField] private Color hasTurretColor;
    [SerializeField] private Color hoverColor;

    private List<GameObject> unitList = new List<GameObject>();
    private Color originalColor;

    Renderer rend;

    private GameObject turret;

    private void Awake()
    {
        attributes = new HexcellAttribute();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        attributes.offsetCoordinate = GetOffsetPosition(transform.localPosition.x, transform.localPosition.y);
        attributes.axialCoordinate = GetAxialFromOffset(attributes.offsetCoordinate.x, attributes.offsetCoordinate.y);

        attributes.walkable = walkable;
        attributes.buildable = buildable;

        axial_Coordinate = attributes.axialCoordinate;
        offset_Coordinate = attributes.offsetCoordinate;
    }

    private void Update()
    {   
        if (!attributes.walkable)
        {
            rend.material.color = unwalkableColor;
            attributes.buildable = false;
        }

        if (!attributes.buildable)
        {   
            if (attributes.walkable)
            {
                rend.material.color = unBuildableColor;
            }
        }

        if (hasTurret)
        {
            rend.material.color = hasTurretColor;
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

    private void OnMouseDown()
    {   
        if (!walkable || !buildable)
        {
            return;
        }

        if (turret != null)
        {
            Destroy(turret);
            hasTurret = false;
            attributes.hasTurret = hasTurret;
            NotifyUnitsCellAvailable();
            rend.material.color = originalColor;
            return;
        }

        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        turret = Instantiate(turretToBuild, transform.position, transform.rotation);
        hasTurret = true;
        attributes.hasTurret = hasTurret;
        NotifyUnitCellRemoved();
        rend.material.color = hasTurretColor;
    }

    public void SubscribeUnits(GameObject newUnit)
    {
        unitList.Add(newUnit);
    }

    public void NotifyUnitCellRemoved()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i] == null)
            {
                unitList.Remove(unitList[i]);
            }
            else
            {
                unitList[i].GetComponent<Enemy>().CheckForPathBlock(axial_Coordinate);
            }
        }
    }

    public void NotifyUnitsCellAvailable()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i] == null)
            {
                unitList.Remove(unitList[i]);
            }
            else
            {
                unitList[i].GetComponent<Enemy>().CheckForPathCleared();
            }
        }
    }

    public Vector2 GetOffsetPosition(float worldPosX, float worldPosY)
    {
        float offsetY = (-worldPosY / (HexMetrics.outerRadius * 1.5f));
        float offsetX = worldPosX / (HexMetrics.innerRadius * 2f) - offsetY * 0.5f + offsetY / 2f;
        return new Vector2(offsetX, offsetY);
    }

    public Vector2Int GetAxialFromOffset(float offsetX, float offsetY)
    {
        return new Vector2Int((int)(offsetX - offsetY / 2f), (int)offsetY);
    }
}
