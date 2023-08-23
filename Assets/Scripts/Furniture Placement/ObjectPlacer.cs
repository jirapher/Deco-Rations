using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedGameobjects = new();
    public GameObject furnitureParent;
    public FurnitureSO database;
    public DesignUIManager designMan;
    public int PlaceObject(GameObject prefab, Vector2 gridPosition, int id)
    {
        GameObject go = Instantiate(prefab, furnitureParent.transform);
        go.transform.position = gridPosition;
        //go.GetComponent<Item>().SetID(id);
        SubtractFromDatabase(id);
        placedGameobjects.Add(go);
        return placedGameobjects.Count - 1;
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
        placedGameobjects.RemoveAll(s => s == null);
    }

    internal void RemoveObjectAt(int gameobjectIndex)
    {
        if(placedGameobjects.Count <= gameobjectIndex || placedGameobjects[gameobjectIndex] == null)
        {
            print("snag in remove object at");
            return;
        }

        AddToDatabase(placedGameobjects[gameobjectIndex].GetComponent<Item>().itemID);
        Destroy(placedGameobjects[gameobjectIndex]);
        placedGameobjects[gameobjectIndex] = null;
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
