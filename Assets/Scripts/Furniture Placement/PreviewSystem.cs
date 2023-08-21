using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewSystem : MonoBehaviour
{
    public GameObject cellIndicator;
    private Vector3 cellIndLocalScale;
    public GameObject previewObject;

    //Grid visual in here?

    //"transparent material"
    public Color transparentColor;

    public Color invalidPlacementColor;

    private SpriteRenderer cellIndicatorRenderer;

    public Image previewItemImage;
    private Image previewItemImageBlank;

    private void Start()
    {
        cellIndicatorRenderer = cellIndicator.GetComponent<SpriteRenderer>();
        cellIndLocalScale = cellIndicator.transform.localScale;
        previewItemImageBlank = previewItemImage;
        //transparentColor = ogColor;
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        cellIndicator.transform.localScale = cellIndLocalScale;
        previewObject = Instantiate(prefab);
        previewItemImage.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        SetTransparent(previewObject);
        CellIndicatorSize(size);
        cellIndicator.SetActive(true);
    }

    public void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        cellIndicator.transform.localScale = cellIndLocalScale;
        ApplyFeedbackToCursor(false);
    }

    private void SetTransparent(GameObject previewObject)
    {
        previewObject.GetComponent<SpriteRenderer>().color = transparentColor;
    }

    private void CellIndicatorSize(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x *2, size.y *2, 1);
            //cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        ClearPreviewImage();
        if(previewObject == null) { return; }

        Destroy(previewObject);
    }

    public void ClearPreviewImage()
    {
        previewItemImage = previewItemImageBlank;
    }

    public void UpdatePosition(Vector2 position, bool validity)
    {

        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    public void MovePreview(Vector2 position)
    {
        previewObject.transform.position = position;
    }

    public void MoveCursor(Vector2 position)
    {
        cellIndicator.transform.position = position;
    }

    public void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : invalidPlacementColor;
        previewObject.GetComponent<SpriteRenderer>().color = c;
        cellIndicatorRenderer.color = c;
    }

    public void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : invalidPlacementColor;
        cellIndicatorRenderer.color = c;
    }
}
