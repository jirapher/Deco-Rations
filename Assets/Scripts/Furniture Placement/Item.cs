using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] spriteDirections;
    public int curDirection = 0;
    public bool canRotate;
    public int itemID = -1;

    public int instanceID = 0;
    //Directions 0:up, 1:right, 2:down, 3:left
    //all start up?

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        curDirection = 1;
        SetInstanceID();
    }

    public void SetInstanceID()
    {
        instanceID = Random.Range(1000, 9999);
        print(instanceID + "ID");
    }

    public void Rotate()
    {
        if (!canRotate) { return; }
        curDirection++;

        if(curDirection > spriteDirections.Length - 1)
        {
            curDirection = 0;
        }

        UpdateSprite();
    }

    public void SetID(int id)
    {
        itemID = id;
    }

    private void UpdateSprite()
    {
        sr.sprite = spriteDirections[curDirection];
    }
}
