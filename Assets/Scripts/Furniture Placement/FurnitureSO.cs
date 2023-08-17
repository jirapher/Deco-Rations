using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FurnitureData", menuName = "ScriptableObjects/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public List<FurnitureData> furnitureData;
}

[Serializable]
public class FurnitureData
{
    [SerializeField] public string name;
    [SerializeField] public int id;
    [SerializeField] public Vector2Int size = Vector2Int.one;
    [SerializeField] public GameObject prefab;
}
