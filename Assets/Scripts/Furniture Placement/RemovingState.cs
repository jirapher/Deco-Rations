using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameobjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer placer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer placer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.placer = placer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        Vector2Int gridPosTemp = new Vector2Int(gridPosition.x, gridPosition.y);
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(gridPosTemp, Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if (floorData.CanPlaceObjectAt(gridPosTemp, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if(selectedData == null)
        {
            return;
        }
        else
        {
            gameobjectIndex = selectedData.GetRepresentationIndex(gridPosTemp);

            if(gameobjectIndex == -1) { return; }

            selectedData.RemoveObjectAt(gridPosTemp);

            //placer.RemoveObjectAt(gameobjectIndex);
        }

        Vector2 cellPos = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPos, CheckIfSelectionIsValid(gridPosTemp));
    }

    private bool CheckIfSelectionIsValid(Vector2Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        Vector2Int gridPosTemp = new Vector2Int(gridPosition.x, gridPosition.y);
        bool validity = CheckIfSelectionIsValid(gridPosTemp);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
