using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefender : MonoBehaviour
{
    public bool shoot = false, aoe = false, freeze = false;

    
    public EnemyMovement curTarget;
    public float activationSpeed;

    [Header("Shooting")]
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public RangedDetection detectionCollider;

    [Header("AOE")]
    public AOEHit aoeHit;

    private TDManager manager;

    public bool whole, half, quarter;
    private float musicTime = 0;
    private void Start()
    {
        manager = TDManager.instance;
        NoteAssignment();
        //half note = twice per measure.
        // half = 4; 
        //set wait time to random either 
    }

    private void NoteAssignment()
    {
        float time = 0;
        if (whole) { time = 2; }
        if (half) { time = 1; }
        if (quarter) { time = 0.5f; }
        musicTime = time;
    }

    public IEnumerator StartCombat()
    {
        yield return new WaitForSeconds(0.5f);
        bool go = true;
        float aTimer = musicTime;

        while(go)
        {
            aTimer -= Time.deltaTime;
            if(aTimer <= 0f)
            {
                if (shoot)
                {
                    Shoot();   
                }

                if (aoe)
                {
                    StartCoroutine(aoeHit.Activate());
                }

                aTimer = musicTime;
            }

            yield return null;

        }
    }

    public void EndCombat()
    {
        StopAllCoroutines();
        if (aoe)
        {
            aoeHit.Deactivate();
        }
    }

    private void Shoot()
    {
        if(curTarget == null || curTarget.HP <= 0)
        {
            detectionCollider.RunDetection();
            return;
        }

        GameObject g = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        g.GetComponent<Projectile>().target = curTarget.gameObject;

    }

}
