using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjust : MonoBehaviour
{

    private Camera cam;
    public float normalSize, designSize, nightSize;
    public float transitionSpeed = 3f;
    private void Start()
    {
        cam = Camera.main;
    }

    public IEnumerator SetPerspective(bool normal, bool design, bool night)
    {
        float size = 0;
        if (normal) { size = normalSize; }
        if(design) { size = designSize; }
        if(night) { size = nightSize; }

        while(cam.orthographicSize != size)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, size, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        cam.orthographicSize = size;
    }
}
