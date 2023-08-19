using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignUIManager : MonoBehaviour
{
    public GameObject designButtonPrefab;
    public GameObject buttonParent;
    public FurnitureSO database;

    public List<GameObject> allButtons = new();

    private void Start()
    {
        ResetAllFurnitureQuantity();
    }

    public void ResetAllFurnitureQuantity()
    {
        foreach(FurnitureData d in database.furnitureData)
        {
            d.quantity = 0;
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
        b.GetComponent<DesignButtons>().SetDetails(item.itemImage, item.quantity, id);
        allButtons.Add(b);
    }




}
