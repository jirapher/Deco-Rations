using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDManager : MonoBehaviour
{
    public CamAdjust camSettings;
    public FakeSun sunSettings;

    public GameObject[] enemies;

    public Transform[] spawnPoints;

    public FurnitureSO database;

    private List<GameObject> allCurEnemies = new();
    public void Sunset()
    {
        StartCoroutine(sunSettings.ChangeColor(false));
        StartCoroutine(camSettings.SetPerspective(false, false, true));
    }

    public void CreateEnemy(int numToMake)
    {
        Instantiate(enemies[numToMake], spawnPoints[Random.Range(0, spawnPoints.Length)]);
    }


    //waves of animals
    //spawn offscreen, target a random piece of furniture, move towards it
    //whatever animals remain at the end of a 60 second timer, that's the damage dealt



}
