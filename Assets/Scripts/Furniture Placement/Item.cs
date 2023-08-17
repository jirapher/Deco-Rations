using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] spriteDirections;
    public int curDirection = 0;
    public bool canRotate;
    //Directions 0:up, 1:right* (doesn't exist - flip SR), 2:down, 3:left
    //all start up?

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        curDirection = 1;
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

    private void UpdateSprite()
    {
        if(curDirection == 1)
        {
            sr.flipX = true;
            return;
        }
        sr.flipX = false;
        sr.sprite = spriteDirections[curDirection];
    }
}
