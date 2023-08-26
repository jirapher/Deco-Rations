using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefender : MonoBehaviour
{
    public bool shoot = false, aoe = false, freeze = false;

    public GameObject detectionCollider;
    public GameObject curTarget;

    [Header("Shooting")]
    public GameObject projectile;
    public Transform projectileSpawnPoint;

    [Header("AOE")]
    public AOEHit aoeHit;

    public float activationSpeed;
    private float speedHold;

    private TDManager manager;

    private void Start()
    {
        manager = TDManager.instance;
        speedHold = activationSpeed;
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
        if(curTarget == null) { return; }
        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            curTarget = other.gameObject;
        }
    }

}
