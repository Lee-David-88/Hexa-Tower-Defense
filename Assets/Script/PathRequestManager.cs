using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    HexGridPathFind pathfinding;

    bool isProcessing;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<HexGridPathFind>();
    }

    public static void RequestPath(Vector2Int pathStart, Vector2Int pathEnd, Action<Tuple<Vector2Int, bool>[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessing && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishProcessingPath(Tuple<Vector2Int, bool>[] path, bool sucess)
    {
        currentPathRequest.callback(path, sucess);
        isProcessing = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector2Int pathStart;
        public Vector2Int pathEnd;
        public Action<Tuple<Vector2Int, bool>[], bool> callback;

        public PathRequest(Vector2Int _start, Vector2Int _end, Action<Tuple<Vector2Int, bool>[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
