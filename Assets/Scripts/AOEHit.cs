using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEHit : MonoBehaviour
{
    public int damageToDo = 1;
    public float fadeTime = .5f;
    private SpriteRenderer sr;
    public CircleCollider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col.enabled = false;
    }

    public IEnumerator Activate()
    {
        col.enabled = true;

        sr.color = Color.clear;

        float alpha = 0;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newCol = new Color(1, 0, 0, Mathf.Lerp(alpha, 0.3f, t));
            sr.color = newCol;
            yield return null;
        }

        alpha = 1;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newCol = new Color(1, 0, 1, Mathf.Lerp(alpha, 0, t));
            sr.color = newCol;
            yield return null;
        }

        col.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            if(other.TryGetComponent<EnemyMovement>(out EnemyMovement e))
            {
                e.TakeDamage(damageToDo);
            }
        }
    }
}
