using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjust : MonoBehaviour
{

    private Camera cam;
    public float normalSize, designSize, nightSize;
    private void Start()
    {
        cam = Camera.main;
    }

    public void SetDesignPerspective()
    {
        cam.orthographicSize = designSize;
    }

    public void SetNormalSize()
    {
        cam.orthographicSize = normalSize;
    }

    public void SetNightSize()
    {
        cam.orthographicSize = nightSize;
    }
}
