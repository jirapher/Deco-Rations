using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public float lifespan = 3f;
    public int damageToDo = 1;
    public float moveSpeed = 3f;

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }

    private void Update()
    {
        if(target == null) { Destroy(gameObject); return; }
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyMovement>().TakeDamage(damageToDo);
            Destroy(gameObject);
        }

    }
}
