using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float stoppingDistance;
    public bool canMove = false;
    public bool isLand, isSea, isAir;
    public int HP = 1;
    public SpriteRenderer sr;
    private CircleCollider2D col;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //make vector 2 with direction moving - grab x, if it's negative, flip sprite.

        if(target == null || !canMove) { return; }

        if(Vector2.Distance(transform.position, target.position) < stoppingDistance)
        {
            canMove = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if(target.position.x < transform.position.x)
            {
                //moving left
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }

    public void TakeDamage(int damageToTake)
    {
        if (!col.enabled) { return; }
        col.enabled = false;
        Invoke("ColOn", .5f);
        HP -= damageToTake;
        if(HP <= 0)
        {
            TDManager.instance.EnemyDestroySelf(gameObject);
        }
        StartCoroutine(FadeOutDeath());
    }

    private void ColOn()
    {
        col.enabled = true;
    }

    private IEnumerator FadeOutDeath()
    {
        float alpha = 1;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newCol = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            sr.color = newCol;
            yield return null;
        }

        Destroy(gameObject);
    }


}
