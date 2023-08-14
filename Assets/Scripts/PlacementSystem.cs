using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public GameObject mouseIndicator, cellIndicator;
    public InputManager inputMan;

    public Grid grid;

    public FurnitureSO database;
    public int selectedObjectIndex = -1;

    public GameObject gridVisual;

    private void Start()
    {
        StopPlacement();
    }

    private void Update()
    {
        if(selectedObjectIndex < 0) { return; }
        mouseIndicator.transform.position = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseIndicator.transform.position);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        //fancy foreach loop
        selectedObjectIndex = database.furnitureData.FindIndex(data => data.id == ID);

        if(selectedObjectIndex < 0)
        {
            print($"No ID Found {ID}");
            return;
        }

        gridVisual.SetActive(true);
        cellIndicator.SetActive(true);
        inputMan.OnClicked += PlaceStructure;
        inputMan.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputMan.IsPointerOverUI()) { return; }

        Vector2 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        GameObject go = Instantiate(database.furnitureData[selectedObjectIndex].prefab);
        go.transform.position = grid.CellToWorld(gridPosition);

    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisual.SetActive(false);
        cellIndicator.SetActive(false);
        inputMan.OnClicked -= PlaceStructure;
        inputMan.OnExit -= StopPlacement;
    }


}
