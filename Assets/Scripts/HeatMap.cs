using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMap : MonoBehaviour
{
    public int gridLengthX = 135;
    public int gridLengthY = 80;
    private int defaultLengthX = 135;
    private int defaultLengthY = 80;

    private Vector2 mapOffset;

    public int cellSizeX = 1;
    public int cellSizeY = 1;

    int[,] eventCounts;

    public EventHandler eventHandler;

    public GameObject cubePrefab;
    public GameObject arrowPrefab;

    public Gradient colorGradient;

    public int maxCounts = 100;

    private void Start()
    {
        eventCounts = new int[gridLengthX, gridLengthY];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CountEvents();
        }
    }
    void CountEvents()
    {
        cellSizeX = defaultLengthX / gridLengthX;
        cellSizeY = defaultLengthY / gridLengthY;

        mapOffset = new Vector2(30, 0);

        for (int i = 0; i < eventHandler.positionEvents.events.Count; ++i)
        {
            int xGrid, yGrid;
            positionToTileSpace(eventHandler.positionEvents.events[i].position.x, eventHandler.positionEvents.events[i].position.z, out xGrid, out yGrid);
            eventCounts[xGrid, yGrid]++;
        }

        VisualizeEvents();
    }

    void VisualizeEvents()
    {
        for(int i = 0; i < gridLengthX; ++i)
        {
            for(int j = 0; j < gridLengthY; ++j)
            {
                if(eventCounts[i, j ] > 0)
                {
                    SpawnCube(i, j, eventCounts[i, j]);
                }
            }
        }
    }

    private void SpawnCube(int x, int y, int counts)
    {
        Vector3 position;
        tileSpaceToPosition(x, y, out position.x, out position.z);
        position.y = GetHeight(position.x, position.z);
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        float f = Mathf.Clamp01((float)counts / maxCounts);
        Color c = colorGradient.Evaluate(f);
        cube.GetComponent<HeatMapCube>().SetColor(c);
    }

    private float GetHeight(float x, float y)
    {
        Vector3 position = new Vector3(x, 100, y);
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit))
            return hit.point.y;

        return 0;
    }

    private void tileSpaceToPosition(int tx, int ty, out float x, out float y)
    {
        x = tx * cellSizeX + mapOffset.x;
        y = ty * cellSizeY + mapOffset.y;
    }

    private void positionToTileSpace(float x, float y, out int tx, out int ty)
    {
        tx = Mathf.FloorToInt((x - mapOffset.x) / cellSizeX);
        ty = Mathf.FloorToInt((y - mapOffset.y) / cellSizeY);
    }
}
