using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ObjectPlacer : MonoBehaviour
{
    public List<GameObject> placedFurniture = new();
    public GameObject furnitureParent;
    public FurnitureSO database;
    public DesignUIManager designMan;
    public int PlaceObject(GameObject prefab, Vector2 gridPosition, int id)
    {
        GameObject go = Instantiate(prefab, furnitureParent.transform);
        go.transform.position = gridPosition;
        //go.GetComponent<Item>().SetID(id);
        SubtractFromDatabase(id);
        placedFurniture.Add(go);
        AudioManager.instance.PlaySFX(12);
        return placedFurniture.Count - 1;
    }

    private bool CanPlace(int id)
    {
        for(int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].id == id)
            {
                if (database.furnitureData[i].quantity > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void CleanList()
    {
        placedFurniture.RemoveAll(s => s == null);
    }

    internal void RemoveObjectAt(int gameobjectIndex)
    {
        if(placedFurniture.Count < gameobjectIndex || placedFurniture[gameobjectIndex] == null)
        {
            return;
        }

        GameObject g = placedFurniture[gameobjectIndex];
        AddToDatabase(g.GetComponent<Item>().itemID);
        Destroy(g);

        placedFurniture.RemoveAll(x => x == null);

    }

    public void RemoveObject(Item item)
    {
        AddToDatabase(item.itemID);
        Destroy(item.gameObject);
        placedFurniture.RemoveAll(x => x == null);
    }

    public void SubtractFromDatabase(int itemID)
    {
        for(int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].id == itemID)
            {
                int q = database.furnitureData[i].quantity;
                q--;
                if (q < 0)
                {
                    q = 0;
                }

                database.furnitureData[i].quantity = q;

                designMan.UpdateItemDisplayQuantity(itemID, q);
            }
        }

        
    }

    public List<GameObject> GetAllPlacedFurniture()
    {
        return placedFurniture;
    }

    public void AddToDatabase(int itemID)
    {
        for (int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].id == itemID)
            {
                int q = database.furnitureData[i].quantity;

                q++;

                database.furnitureData[i].quantity = q;

                designMan.UpdateItemDisplayQuantity(itemID, q);
            }
        }

    }
}
