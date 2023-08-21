using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int id;
    Grid grid;
    PreviewSystem previewSystem;
    FurnitureSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer placer;

    public PlacementState(int id, Grid grid, PreviewSystem previewSystem, FurnitureSO database, GridData floorData, GridData furnitureData, ObjectPlacer placer)
    {
        this.id = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.placer = placer;


        //fancy foreach loop
        selectedObjectIndex = database.furnitureData.FindIndex(data => data.id == id);

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.furnitureData[selectedObjectIndex].prefab, database.furnitureData[selectedObjectIndex].size);
        }
        else
        {
            throw new System.Exception($"No object with ID {id}");
        }


    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!CheckPlacementValidity(gridPosition, selectedObjectIndex))
        {
            return;
        }

        int index = placer.PlaceObject(database.furnitureData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition), database.furnitureData[selectedObjectIndex].id);

        //Opposite these lines for removal!
        GridData selectedData = database.furnitureData[selectedObjectIndex].id == 0 ? floorData : furnitureData;

        selectedData.AddObjectAt(new Vector2Int(gridPosition.x, gridPosition.y), database.furnitureData[selectedObjectIndex].size, database.furnitureData[selectedObjectIndex].id, index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //Not sure about this -- P4 @ 13:30
        GridData selectedData = database.furnitureData[selectedObjectIndex].id == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(new Vector2Int(gridPosition.x, gridPosition.y), database.furnitureData[selectedObjectIndex].size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
