using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesignUIManager : MonoBehaviour
{
    public GameObject designButtonPrefab;
    public GameObject buttonParent;
    public FurnitureSO database;
    public PlacementSystem placementSys;
    public List<GameObject> allButtons = new();

    public GameObject newDayButton;

    public Color lockedColor;

    private void Start()
    {
        ResetAllFurnitureQuantity();
        AddFreeTower();
    }

    private void AddFreeTower()
    {
        FurnitureData f = database.furnitureData[9];
        f.quantity++;
        f.totalInCirculation++;
        AddItemToInventory(f);
    }

    public void ResetAllFurnitureQuantity()
    {
        foreach(FurnitureData d in database.furnitureData)
        {
            d.quantity = 0;
            d.totalInCirculation = 0;
        }
    }
    public void AddItemToInventory(FurnitureData item)
    {
        //we've established that quantity > 0 && item isn't locked...

        int id = item.id;

        for(int i = 0; i < allButtons.Count; i++)
        {
            if (allButtons[i].GetComponent<DesignButtons>().GetItemID() == id)
            {
                allButtons[i].GetComponent<DesignButtons>().AdjustQuantity(item.quantity);
                return;
            }
        }

        GameObject b = Instantiate(designButtonPrefab, buttonParent.transform);
        b.GetComponent<DesignButtons>().SetDetails(item.itemImage, item.quantity, id, placementSys);
        allButtons.Add(b);
    }

    public void UpdateItemDisplayQuantity(int itemID, int quantity)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            if (allButtons[i].GetComponent<DesignButtons>().GetItemID() == itemID)
            {
                allButtons[i].GetComponent<DesignButtons>().AdjustQuantity(quantity);
                return;
            }
        }
    }

    public void SetDayButtonActive()
    {
        newDayButton.SetActive(true);
    }

    public void SetDayButtonInactive()
    {
        newDayButton.SetActive(false);
    }




}
