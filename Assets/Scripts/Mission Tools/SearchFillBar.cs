using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchFillBar : MonoBehaviour
{
    private Slider bar;
    public MissionManager manager;

    public int areaNum = -1;
    public float baseTime = 3;
    private float baseTimeHold;
    public bool inUse = false;
    //0:woods, 1:cave, 2:beach, 3:river

    private void Start()
    {
        baseTimeHold = baseTime;
        if(areaNum == -1) { print("LIST AREAS DUFUS"); }
        bar = GetComponent<Slider>();
    }

    public IEnumerator FillBar()
    {

        float dice = Random.Range(0.9f, 1.2f);

        baseTime *= dice;


        while (bar.value < bar.maxValue)
        {
            yield return new WaitForSeconds(dice);
            bar.value++;
        }

        manager.MissionCompleteCheck();
        baseTime = baseTimeHold;
        inUse = false;
    }

    public void ProficientAdj()
    {
        baseTime -= 1f;
    }

    public void InproficientAdj()
    {
        baseTime += 1f;
    }

    public void ResetBar()
    {
        baseTime = baseTimeHold;
        inUse = false;
        bar.value = 0;
    }
}
