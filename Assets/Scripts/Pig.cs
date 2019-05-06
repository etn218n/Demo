using System.Collections;
using System;
using UnityEngine;

public delegate void PigEvent();

public class Pig : MonoBehaviour
{
    public EventHandler OnDisappeared;

    private Animator anim;
    private Collider2D collider2d;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        anim       = GetComponent<Animator>();
        rb2d       = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 2.5f)
            StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        collider2d.enabled = false;
        rb2d.gravityScale  = 0f;

        anim.SetBool("Disappear", true);

        yield return new WaitForSeconds(0.3f);

        OnDisappeared?.Invoke(this, null);

        Destroy(this.gameObject);
    }
}
