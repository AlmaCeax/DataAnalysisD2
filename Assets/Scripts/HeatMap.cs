using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatMap : MonoBehaviour
{
    [Header("Grid Options")]
    public int gridLengthX = 135;
    public int gridLengthY = 80;

    public enum HeatMapType { CUBE, ARROW };
    public enum EventType { KILLS, DEATHS, POSITION, LIFELOST, BOXES, JUMPS };

    [Header("Data Visualization Options")]
    public HeatMapType heatMapType = HeatMapType.CUBE;
    public EventType eventType = EventType.POSITION;
    public Gradient colorGradient;
    public int maxCounts = 100;

    [Header("Essentials")]
    public GameObject cubePrefab;
    public GameObject arrowPrefab;

    private int defaultLengthX = 135;
    private int defaultLengthY = 80;

    private Vector2 mapOffset;

    float cellSizeX;
    float cellSizeY;

    int[,] eventCounts;
    float[,] rotations;
    [HideInInspector]
    public int sessionChoice = 0;

    List<GameObject> spawnedObjects;

    private void Start()
    {
        eventCounts = new int[gridLengthX, gridLengthY];
        rotations = new float[gridLengthX, gridLengthY];
        spawnedObjects = new List<GameObject>();
    }

    public void CreateMap()
    {
        ClearHeatMap();

        switch (eventType)
        {
            case EventType.KILLS:
                CountEvents(EventHandler.instance.killEvents.events.Cast<EventData>().ToList());
                break;
            case EventType.DEATHS:
                CountEvents(EventHandler.instance.deathEvents.events.Cast<EventData>().ToList());
                break;
            case EventType.POSITION:
                CountEvents(EventHandler.instance.positionEvents.events.Cast<EventData>().ToList());
                break;
            case EventType.LIFELOST:
                CountEvents(EventHandler.instance.lifeLostEvents.events.Cast<EventData>().ToList());
                break;
            case EventType.BOXES:
                CountEvents(EventHandler.instance.boxDestroyedEvents.events.Cast<EventData>().ToList());
                break;
            case EventType.JUMPS:
                CountEvents(EventHandler.instance.jumpEvents.events.Cast<EventData>().ToList());
                break;
            default:
                break;
        }
        VisualizeEvents();
    }

    public void ClearHeatMap()
    {
        foreach (var item in spawnedObjects)
        {
            Destroy(item);
        }
        spawnedObjects.Clear();

        Array.Clear(eventCounts, 0, eventCounts.Length);
        Array.Clear(rotations, 0, rotations.Length);
    }

    void CountEvents(List<EventData> eventList)
    {
        cellSizeX = (float)defaultLengthX / (float)gridLengthX;
        cellSizeY = (float)defaultLengthY / (float)gridLengthY;
        mapOffset = new Vector2(30 - (defaultLengthX * 0.5f), 0 - (defaultLengthY * 0.5f));

        for (int i = 0; i < eventList.Count; ++i)
        {
            if(eventList[i].pdata.sessionId == EventHandler.instance.sessions[sessionChoice])
            {
                int xGrid, yGrid;
                positionToTileSpace(eventList[i].position.x, eventList[i].position.z, out xGrid, out yGrid);
                eventCounts[xGrid, yGrid]++;
                rotations[xGrid, yGrid] = eventList[i].rotation.eulerAngles.y;
            }
        }
    }

    void VisualizeEvents()
    {
        for(int i = 0; i < gridLengthX; ++i)
        {
            for(int j = 0; j < gridLengthY; ++j)
            {
                if(eventCounts[i, j ] > 0)
                {
                    if(heatMapType == HeatMapType.CUBE)
                        SpawnCube(i, j, eventCounts[i, j]);
                    else
                        SpawnArrow(i, j, eventCounts[i, j], rotations[i,j]);
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
        cube.transform.localScale = new Vector3(cellSizeX, 1, cellSizeY);
        float f = Mathf.Clamp01((float)counts / maxCounts);
        Color c = colorGradient.Evaluate(f);
        cube.GetComponent<HeatMapCube>().SetColor(c);

        spawnedObjects.Add(cube);
    }

    private void SpawnArrow(int x, int y, int counts, float rotation)
    {
        Vector3 position;
        tileSpaceToPosition(x, y, out position.x, out position.z);
        position.y = GetHeight(position.x, position.z);
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.Euler(new Vector3(90, 0, -rotation)));
        float f = Mathf.Clamp01((float)counts / maxCounts);
        Color c = colorGradient.Evaluate(f);
        arrow.GetComponent<HeatMapCube>().SetColor(c);

        spawnedObjects.Add(arrow);
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
