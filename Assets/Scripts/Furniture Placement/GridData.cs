using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector2Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector2Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector2Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
        foreach(var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                Debug.Log($"Dictionary already contains {pos}");
                return;
            }

            placedObjects[pos] = data;
        }
    }

    private List<Vector2Int> CalculatePositions(Vector2Int gridPosition, Vector2Int objectSize)
    {
        List<Vector2Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector2Int(x, y));
            }
        }

        return returnVal;
    }

     public bool CanPlaceObjectAt(Vector2Int gridPosition, Vector2Int objectSize)
    {
        List<Vector2Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }

        return true;
    }

    internal void RemoveObjectAt(Vector2Int gridPosTemp)
    {
        foreach (var pos in placedObjects[gridPosTemp].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }

    internal int GetRepresentationIndex(Vector2Int gridPosTemp)
    {
        if(placedObjects.ContainsKey(gridPosTemp) == false)
        {
            return -1;
        }

        return placedObjects[gridPosTemp].placedObjectIndex;
    }
}

public class PlacementData
{
    public List<Vector2Int> occupiedPositions;

    public int id;

    public int placedObjectIndex;

    public PlacementData(List<Vector2Int> occupiedPositions, int id, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.id = id;
        this.placedObjectIndex = placedObjectIndex;
    }
}
