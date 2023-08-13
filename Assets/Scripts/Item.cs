using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] spriteDirections;
    public int curDirection = 0;

    //Directions 0:up, 1:right, 2:down, 3:left
    //all start up?

    private void Start()
    {
        curDirection = 1;
    }

    public void Rotate()
    {
        curDirection++;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        sr.sprite = spriteDirections[curDirection];
    }
}
