using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private new Camera camera;
    private float targetZoom;
    [SerializeField] private float zoomMin = 4.5f;
    [SerializeField] private float zoomMax = 8f;
    [SerializeField] private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        targetZoom = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, zoomMin, zoomMax);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
