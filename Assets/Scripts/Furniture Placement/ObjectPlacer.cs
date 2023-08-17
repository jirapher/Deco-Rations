using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedGameobjects = new();

    public int PlaceObject(GameObject prefab, Vector2 gridPosition)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = gridPosition;
        placedGameobjects.Add(go);
        return placedGameobjects.Count - 1;
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

        Destroy(placedGameobjects[gameobjectIndex]);
        placedGameobjects[gameobjectIndex] = null;
        //CleanList();
        print("remove object at success");
    }
}
