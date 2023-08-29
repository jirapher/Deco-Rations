using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TDManager : MonoBehaviour
{
    public static TDManager instance;
    public CamAdjust camSettings;
    public FakeSun sunSettings;
    public ObjectPlacer objectPlacer;
    public List<GameObject> placedFurniture = new();
    public List<GameObject> allCurEnemies = new();

    [Header("Spawning")]
    private List<Transform> allSpawnPoints = new();
    public Transform[] landSpawnPoints;
    public Transform[] seaSpawnPoints;
    public GameObject[] enemies;
    //0 = land, 1 = sea, 2 = air

    

    public TMP_Text timerText;
    public float timer = 45f;

    private void Start()
    {
        PopulateAllSpawnPoints();
        instance = this;
    }

    private void PopulateAllSpawnPoints()
    {
        for(int i = 0; i < seaSpawnPoints.Length; i++)
        {
            allSpawnPoints.Add(seaSpawnPoints[i]);
        }

        for (int i = 0; i < landSpawnPoints.Length; i++)
        {
            allSpawnPoints.Add(landSpawnPoints[i]);
        }
    }
    public void Sunset()
    {
        timerText.text = "00:00";
        StartCoroutine(sunSettings.ChangeColor(false));
        StartCoroutine(camSettings.SetPerspective(false, false, true));
        GetPlacedFurniture();
        StartCoroutine(AudioManager.instance.DayToNightTransition());
        ActivateTowers();
        EnemyCalc(GameManager.instance.curDay);
    }

    private void GetPlacedFurniture()
    {
        placedFurniture.Clear();
        placedFurniture = new List<GameObject>(objectPlacer.GetAllPlacedFurniture());
        
    }

    private void ActivateTowers()
    {
        for (int i = 0; i < placedFurniture.Count; i++)
        {
            if (placedFurniture[i].TryGetComponent<TowerDefender>(out TowerDefender td))
            {
                StartCoroutine(td.StartCombat());
            }
        }
    }

    public void CreateEnemy(int numToMake)
    {
        GameObject g = Instantiate(enemies[numToMake], null);
        EnemyMovement e = g.GetComponent<EnemyMovement>();
        e.target = placedFurniture[Random.Range(0, placedFurniture.Count)].transform;

        if (e.isLand)
        {
            g.transform.position = landSpawnPoints[Random.Range(0, landSpawnPoints.Length)].position;
        }

        if (e.isSea)
        {
            g.transform.position = seaSpawnPoints[Random.Range(0, seaSpawnPoints.Length)].position;
        }

        if (e.isAir)
        {
            g.transform.position = allSpawnPoints[Random.Range(0, allSpawnPoints.Count)].position;
        }

        e.canMove = true;
        //needs to access list of currently placed furniture (placement system?) *not database..
        allCurEnemies.Add(g);
    }

    public void EnemyDestroySelf(GameObject enemy)
    {
        for(int i = 0; i < allCurEnemies.Count; i++)
        {
            if(enemy == allCurEnemies[i])
            {
                allCurEnemies.RemoveAt(i);
                //allCurEnemies.Sort();
            }
        }
    }

    private IEnumerator InvasionStart(float spawnTime, int highestToMake, int dayNum)
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(RunTimer());
        highestToMake += 1;
        while(timer > 25)
        {
            float t = spawnTime;
            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }

            CreateEnemy(Random.Range(0, highestToMake));
            yield return null;
        }

        while(timer > 0)
        {
            if (AllDead())
            {
                EndOfNight();
                yield break;
            }

            yield return null;
        }

        EndOfNight();
        yield break;
    }

    private bool AllDead()
    {
        if(allCurEnemies.Count <= 0)
        {
            return true;
        }
        else
        {
            allCurEnemies.RemoveAll(x => x == null);
        }

        return false;
    }

    private IEnumerator RunTimer()
    {
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            SetTimerUI();
            yield return null;
        }

        timerText.text = "00:00";
    }

    private void SetTimerUI()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndOfNight()
    {
        timer = 45f;
        DeactivateTowers();
        //print("Come back here...");
        Camera.main.orthographicSize = 10;
        //StartCoroutine(camSettings.SetPerspective(true, false, false));
        //cut to night scene
        GameManager.instance.NewDay();
        sunSettings.BackToNormal();
    }

    private void DeactivateTowers()
    {
        for (int i = 0; i < placedFurniture.Count; i++)
        {
            if (placedFurniture[i].TryGetComponent<TowerDefender>(out TowerDefender td))
            {
                td.EndCombat();
            }
        }
    }

    public int HPCalc()
    {
        int HPminus = 0;
        foreach(GameObject g in allCurEnemies)
        {
            if(g != null)
            {
                HPminus += 1;
            }   
        }

        DestroyAllEnemies();

        return HPminus;
    }

    private void DestroyAllEnemies()
    {
        foreach(GameObject g in allCurEnemies)
        {
            Destroy(g);
        }

        allCurEnemies.Clear();
    }
    private void EnemyCalc(int dayNum)
    {
        switch (dayNum)
        {
            case 1:
                StartCoroutine(InvasionStart(6.5f, 0, dayNum));
            break;

            case 2:
                StartCoroutine(InvasionStart(5f, 0, dayNum));
                break;

            case 3:
                StartCoroutine(InvasionStart(4.5f, 1, dayNum));
                break;

            case 4:
                StartCoroutine(InvasionStart(4.25f, 1, dayNum));
                break;

            case 5:
                StartCoroutine(InvasionStart(3.5f, 2, dayNum));
                break;

            case 6:
                StartCoroutine(InvasionStart(3f, 2, dayNum));
                break;

            case 7:
                StartCoroutine(InvasionStart(2.6f, 2, dayNum));
                break;

            case 8:
                StartCoroutine(InvasionStart(2f, 2, dayNum));
                break;
        }
    }

    //waves of animals
    //spawn offscreen, target a random piece of furniture, move towards it
    //whatever animals remain at the end of a 60 second timer, that's the damage dealt



}
