using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FurnitureData", menuName = "ScriptableObjects/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public List<FurnitureData> furnitureData;
}

[Serializable]
public class FurnitureData
{
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public bool restoresHP;
    [SerializeField] public int id;
    [SerializeField] public Sprite itemImage;
    [SerializeField] public Vector2Int size = Vector2Int.one;
    [SerializeField] public GameObject prefab;
    [SerializeField] public int quantity = 0;
    [SerializeField] public int[] requiredMaterialType;
    [SerializeField] public int[] requiredMaterialQuantity;
}
