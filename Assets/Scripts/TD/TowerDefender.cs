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

    private void Start()
    {
        manager = TDManager.instance;
    }

    public IEnumerator StartCombat()
    {
        //figure out bpm -- translate to timer
        float aTimer = activationSpeed;

        while(manager.timer > 1)
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

                aTimer = activationSpeed;
            }

            yield return null;

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
