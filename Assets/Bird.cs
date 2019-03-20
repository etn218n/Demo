using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState { Null, Init, Aim, Launch }

    private BirdState birdState;

    private bool MachineOn;

    private bool MouseDragged;
    private bool MouseReleased;

    private Rigidbody2D rb2d;
    private SpringJoint2D springJoint2d;
    private Transform pivotPoint;

    private void Awake()
    {
        rb2d          = GetComponent<Rigidbody2D>();
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

        this.gameObject.transform.right = pivotPoint.position - this.gameObject.transform.position;

        while (true)
        {
            if (MouseDragged) { birdState = BirdState.Aim; break; }

            yield return null;
        }
    }

    private IEnumerator Aim()
    {
        while (true)
        {
            if (MouseReleased) { birdState = BirdState.Launch; break; }

            this.gameObject.transform.right = pivotPoint.position - this.gameObject.transform.position;

            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb2d.MovePosition(dragPos);

            yield return null;
        }

        springJoint2d.enabled = true;
    }

    private IEnumerator Launch()
    {
        rb2d.gravityScale = 1f;
        rb2d.constraints  = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(0.2f);

        rb2d.constraints = RigidbodyConstraints2D.None;
        springJoint2d.breakForce = 20f;

        MachineOn = false;
        birdState = BirdState.Null;
    }

    private IEnumerator BirdStateMachine()
    {
        while (MachineOn)
        {
            switch (birdState)
            {
                case BirdState.Init:   yield return StartCoroutine(Init());   break;
                case BirdState.Aim:    yield return StartCoroutine(Aim());    break;
                case BirdState.Launch: yield return StartCoroutine(Launch()); break;
            }
        }
    }
}
