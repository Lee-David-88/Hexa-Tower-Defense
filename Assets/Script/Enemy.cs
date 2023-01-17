using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private Transform target;

    public Vector3 velocity;
    public int numHexFromTarget = 500;
    public Vector3 previous;
    private Renderer rend;
    private Color OgColour;

    // Enemy Stats
    [SerializeField] public float speed;
    [SerializeField] public float health;

    // Tuple and Queue for path finding 
    private Tuple<Vector2Int, bool>[] path;
    private Queue<Tuple<Vector2Int, bool>> currentWaypointsLeft = new Queue<Tuple<Vector2Int, bool>>();

    // A list of cells as game object. Used to store the cell to subscribe into.
    private List<GameObject> cellList = new List<GameObject>();


    private void Awake()
    { 
        rend = GetComponent<Renderer>();
        previous = transform.position;
    }

    private void Start()
    {
        OgColour = rend.material.color;
        target = GameObject.Find("Target").transform;

        // Get all cells on the map and store it in a list
        int childCount = 0;
        Transform map = GameObject.Find("Generated Map").transform;
        while (map.childCount > childCount)
        {
            SubscribeCells(map.GetChild(childCount).gameObject);
            childCount++;
        }

        // Add this enemy game object into the cell
        NotifyCells(gameObject);

        // Convert unit starting world space to axial coordinate
        Vector2Int axialStartPos = GetAxialFromWorldPosition(transform.localPosition.x, transform.localPosition.y);

        // Convert target world space to axial coordinate
        Vector2Int axialTargetPos = GetAxialFromWorldPosition(target.localPosition.x, target.localPosition.y);

        PathRequestManager.RequestPath(axialStartPos, axialTargetPos, OnPathFound);
    }

    private void Update()
    {   
        // Get the game objects velocity for turret aiming calculation
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;
        this.transform.localScale = new Vector3(0.5f, 0.5f, numHexFromTarget);
    }

    #region Function for gameplay Eg. (Taking Damage)

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine("HitIndicator");
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator HitIndicator()
    {
        rend.material.color = new Color(OgColour.r, OgColour.g, OgColour.b, 0.5f);

        yield return new WaitForSeconds(0.08f);
        yield return new WaitForSeconds(0.04f);

        rend.material.color = OgColour;
    }

    #endregion

    #region Pathfinding functions

    public void OnPathFound(Tuple<Vector2Int, bool>[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {   
            path = newPath;
            string pathString = "";
            for (int i = 0; i < path.Length; i++)
            {
                pathString += path[i].ToString();
            }
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        } else
        {
            StopCoroutine("FollowPath");
        }
    }
    // Request path and follow the path
    IEnumerator FollowPath()
    {
        float currentSpeed = speed;

        currentWaypointsLeft.Clear();

        for (int i = 0; i < path.Length; i++)
        {
            currentWaypointsLeft.Enqueue(path[i]);
        }

        Vector3 currentWaypoint = GetWorldPositionFromAxial(currentWaypointsLeft.Peek().Item1.x, currentWaypointsLeft.Peek().Item1.y);

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                currentWaypointsLeft.Dequeue();
                if (currentWaypointsLeft.Count == 0)
                {
                    Destroy(gameObject);
                    yield break;
                }
                currentWaypoint = GetWorldPositionFromAxial(currentWaypointsLeft.Peek().Item1.x, currentWaypointsLeft.Peek().Item1.y);
                numHexFromTarget = currentWaypointsLeft.Count;
            }
            
            if (currentWaypointsLeft.Peek().Item2 == true)
            {
                currentSpeed = 0f;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, currentSpeed);
            yield return null;
        }
    }

    #endregion

    #region Function for recieving update from hexcell and notifying hexcell

    public void CheckForPathBlock(Vector2Int axialCoord)
    {
        if (currentWaypointsLeft.Count > 0)
        {
            List<Vector2Int> waypointsLeft = new List<Vector2Int>();

            foreach (Tuple<Vector2Int, bool> info in currentWaypointsLeft)
            {
                waypointsLeft.Add(info.Item1);
            }

            for (int i = 0; i < currentWaypointsLeft.Count; i++)
            {
                if (waypointsLeft[i] == axialCoord)
                {
                    PathRequestManager.RequestPath(GetAxialFromWorldPosition(transform.localPosition.x, transform.localPosition.y), GetAxialFromWorldPosition(target.localPosition.x, target.localPosition.y), OnPathFound);
                }
            }
        }
    }
    public void CheckForPathCleared()
    {
        if (transform.position != target.position)
        {
            //Debug.Log("cell cleared! new potential path!");
            PathRequestManager.RequestPath(GetAxialFromWorldPosition(transform.localPosition.x, transform.localPosition.y), GetAxialFromWorldPosition(target.localPosition.x, target.localPosition.y), OnPathFound);
        }
    }

    public void SubscribeCells(GameObject walkableCell)
    {
        cellList.Add(walkableCell);
    }

    public void NotifyCells(GameObject newUnit)
    {
        foreach (GameObject cells in cellList)
        {
            cells.GetComponent<Hexcell>().SubscribeUnits(newUnit);
        }
    }

    #endregion

    #region Helper function for translating between world position, offset coordinate, axial coordinate and visa versa

    public Vector2Int GetAxialFromWorldPosition(float worldPosX, float worldPosY)
    {
        Vector2 offsetPos = GetOffsetPosition(worldPosX, worldPosY);
        return GetAxialFromOffset(offsetPos.x, offsetPos.y);
    }
    public Vector3 GetWorldPositionFromAxial(int axialX, int axialY)
    {
        Vector2 offsetPos = GetOffsetFromAxial(axialX, axialY);
        return GetWorldPosition(offsetPos.x, offsetPos.y);
    }
    public Vector2 GetOffsetPosition(float worldPosX, float worldPosY)
    {
        float offsetY = (-worldPosY / (HexMetrics.outerRadius * 1.5f));
        float offsetX = worldPosX / (HexMetrics.innerRadius * 2f) - offsetY * 0.5f + offsetY / 2f;
        return new Vector2(offsetX, offsetY);
    }
    public Vector2Int GetAxialFromOffset(float offsetX, float offsetY)
    {   
        return new Vector2Int((int)(offsetX - (int)(offsetY / 2f)), (int)offsetY);
    }
    public Vector3 GetWorldPosition(float offsetX, float offsetY)
    {
        return new Vector3((offsetX + offsetY * 0.5f - offsetY / 2f) * (HexMetrics.innerRadius * 2f), -offsetY * (HexMetrics.outerRadius * 1.5f), 0);
    }
    public Vector2 GetOffsetFromAxial(int axialX, int axialY)
    {
        return new Vector2(axialX + axialY / 2f, axialY);
    }

    #endregion
}
