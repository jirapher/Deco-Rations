using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    public ItemButton[] itemButtons;
    public Item[] allItems;
    private int itemButtonPosition = 0;
    private GameObject curItem;


    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (curItem == null) { return; }
            ItemFollowCursor();
        }
    }


    private void ItemFollowCursor()
    {
        curItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void InstantiateItem()
    {
        //called from button
    }

    public void AddItem(Item itemToAdd, int quantityToAdd)
    {
        //first find item against all prefabs
        //crete new button if no item exists
        //otherwise itemQuantity++
    }


}
