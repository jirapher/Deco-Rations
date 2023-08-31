using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //public GameObject mouseIndicator;
    public InputManager inputMan;

    public Grid grid;

    public FurnitureSO database;
    //public int selectedObjectIndex = -1;

    public GameObject gridVisual;

    private GridData floorData, furnitureData;

    public ObjectPlacer placer;
    //private SpriteRenderer previewRenderer;

    //public List<GameObject> placedGameobjects = new();

    public PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;
    public LayerMask furnitureLayer;
    //private int activePieceID = -1;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        //previewRenderer = cellIndicator.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(buildingState == null) { return; }

        Vector3 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        if (lastDetectedPosition == gridPosition) { return; }

        buildingState.UpdateState(gridPosition);
        lastDetectedPosition = gridPosition;
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisual.SetActive(true);
        //activePieceID = ID;
        buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, placer);
        inputMan.OnClicked += PlaceStructure;
        inputMan.OnExit += StopPlacement;
    }

    public void StartRemoval()
    {
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, placer);
        inputMan.OnClicked += PlaceStructure;
        inputMan.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputMan.IsPointerOverUI()) { return; }

        Vector2 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        buildingState.OnAction(gridPosition);
        //placer.SubtractFromDatabase(activePieceID);
        StopPlacement();
    }

    public bool TryRemoveObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, furnitureLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<Item>(out Item item))
            {
                placer.RemoveObject(item);
                return true;
            }
            else
            {
                print("No item found");
                return false;
            }

        }

        return false;
    }

    /*private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //Not sure about this -- P4 @ 13:30
        GridData selectedData = database.furnitureData[selectedObjectIndex].id == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(new Vector2Int(gridPosition.x, gridPosition.y), database.furnitureData[selectedObjectIndex].size);
    }*/

    public void StopPlacement()
    {
        if(buildingState == null) { return; }
        gridVisual.SetActive(false);
        buildingState.EndState();
        inputMan.OnClicked -= PlaceStructure;
        inputMan.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
        //activePieceID = -1;
    }


}
