using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    public ItemButton[] itemButtons;
    public Item[] allItems;
    private int curItemButton = 0;
    private GameObject curItem;

    private Vector3 mousePos;
    public float itemMoveSpeed = 0.1f;

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
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        curItem.transform.position = Vector2.Lerp(curItem.transform.position, mousePos, itemMoveSpeed);
    }

    public void InstantiateItem()
    {

    }

    public void AddItem(Item itemToAdd, int quantityToAdd)
    {

    }


}
