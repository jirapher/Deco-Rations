using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDetection : MonoBehaviour
{
    private CircleCollider2D col;
    public TowerDefender defender;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }
    public void RunDetection()
    {
        col.enabled = true;
    }
    public void StopDetection()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            EnemyMovement e = other.GetComponent<EnemyMovement>();
            if(e.HP <= 0) { return; }
            defender.curTarget = e;
            StopDetection();
        }
    }
}
