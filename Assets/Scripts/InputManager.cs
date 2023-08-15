using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera cam;
    private Vector3 lastPosition;
    public LayerMask placementLayer;

    public event Action OnClicked, OnRightClick, OnExit;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TryRotateObject();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }


    //if over UI element - disables furniture interaction
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetSelectedMapPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, placementLayer);

        if(hit.collider != null)
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }

    public void TryRotateObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, placementLayer);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.TryGetComponent<Item>(out Item item))
            {
                item.Rotate();
            }
            else
            {
                print("No item script found");
            }
            
        }
    }
}
