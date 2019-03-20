using System.Collections;
using UnityEngine;

public class Pig : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 2.5f)
            StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        anim.SetBool("Disappear", true);

        yield return new WaitForSeconds(0.3f);

        Destroy(this.gameObject);
    }
}
