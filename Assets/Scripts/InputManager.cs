using System;
using UnityEngine;
using UnityEngine.EventSystems;
using FMOD;

public class InputManager : MonoBehaviour
{
    private Camera cam;
    private Vector3 lastPosition;
    public LayerMask furnitureLayer;
    public LayerMask groundLayer;

    public event Action OnClicked, OnExit;

    public FMODUnity.EventReference onClickSFX;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlaySFX(0);
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
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, groundLayer);

        if(hit.collider != null)
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }

    public void TryRotateObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, furnitureLayer);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.TryGetComponent<Item>(out Item item))
            {
                if (!item.canRotate) { return; }
                item.Rotate();
                AudioManager.instance.PlaySFX(13);
            }
            else
            {
                print("No item found");
            }
            
        }
    }
}
