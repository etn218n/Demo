using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState { Null, Init, Aim, Launch, Disappear }

    private BirdState birdState;

    private bool MachineOn;

    private bool MouseDragged;
    private bool MouseReleased;

    private Rigidbody2D rb2d;
    private SpringJoint2D springJoint2d;
    private Animator anim;
    private Transform pivotPoint;

    private void Awake()
    {
        rb2d          = GetComponent<Rigidbody2D>();
        anim          = GetComponent<Animator>();
        springJoint2d = GetComponent<SpringJoint2D>();

        pivotPoint = springJoint2d.connectedBody.transform;
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
        springJoint2d.enabled = false;

        gameObject.transform.right = pivotPoint.position - this.gameObject.transform.position;

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
            gameObject.transform.right = pivotPoint.position - gameObject.transform.position;

            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb2d.MovePosition(dragPos);

            if (MouseReleased) { birdState = BirdState.Launch; break; }

            yield return null;
        }

        //Exit State
        springJoint2d.enabled = true;
    }

    private IEnumerator Launch()
    {
        rb2d.gravityScale = 1f;
        rb2d.constraints  = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(0.2f);

        rb2d.constraints = RigidbodyConstraints2D.None;
        springJoint2d.breakForce = 20f;

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
