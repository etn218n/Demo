using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState { Null, Init, Aim, Launch, Disappear }

    private BirdState birdState;

    private bool MachineOn;

    private bool MouseDragged;
    private bool MouseReleased;

    private Rigidbody2D rb2d;
    private Collider2D collider2d;
    private Animator anim;

    [SerializeField]
    private Transform aimPoint;

    private void Awake()
    {
        rb2d          = GetComponent<Rigidbody2D>();
        anim          = GetComponent<Animator>();
        collider2d    = GetComponent<Collider2D>();
    }

    private void Start()
    {
        birdState = BirdState.Init;
        MachineOn = true;
        StartCoroutine(BirdStateMachine());
    }

    private void OnMouseDrag() { MouseDragged  = true; }
    private void OnMouseUp()   { MouseReleased = true; }

    private IEnumerator Init()
    {
        MouseDragged  = false;
        MouseReleased = false;

        rb2d.gravityScale = 0f;

        gameObject.transform.right = aimPoint.position - this.gameObject.transform.position;

        SetAnimation(BirdState.Init);

        while (true)
        {
            if (MouseDragged) { birdState = BirdState.Aim; break; }

            yield return null;
        }
    }

    private IEnumerator Aim()
    {
        // Enter State
        SetAnimation(BirdState.Aim);

        //Update State
        while (true)
        {
            gameObject.transform.right = aimPoint.position - gameObject.transform.position;

            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb2d.MovePosition(dragPos);

            if (MouseReleased) { birdState = BirdState.Launch; break; }

            yield return null;
        }

        //Exit State
    }

    private IEnumerator Launch()
    {
        rb2d.gravityScale = 1f;

        rb2d.velocity = (aimPoint.position - gameObject.transform.position) * 10f;

        yield return new WaitForSeconds(0.2f);

        SetAnimation(BirdState.Launch);

        float maxWaitTime = 4f;
        float waitTime    = 0f;

        while (true)
        {
            waitTime += Time.deltaTime;

            if (waitTime > maxWaitTime)          { birdState = BirdState.Disappear; break; }
            if (rb2d.velocity.magnitude < 0.01f) { birdState = BirdState.Disappear; break; }

            yield return null;
        }
    }

    private IEnumerator Disappear()
    {
        collider2d.enabled = false;
        rb2d.gravityScale  = 0f;

        SetAnimation(BirdState.Disappear);

        // wait for Disappear Animation to complete
        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

    private IEnumerator BirdStateMachine()
    {
        while (MachineOn)
        {
            switch (birdState)
            {
                case BirdState.Init:      yield return StartCoroutine(Init());      break;
                case BirdState.Aim:       yield return StartCoroutine(Aim());       break;
                case BirdState.Launch:    yield return StartCoroutine(Launch());    break;
                case BirdState.Disappear: yield return StartCoroutine(Disappear()); break;
            }
        }
    }

    private void SetAnimation(BirdState state) { anim.SetInteger("StateID", (int)state); }
}
