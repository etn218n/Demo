using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Experiment with state machine using Coroutine
// This class can be simpler with normal implementation
// but is kept this way to see if this kind of state machine
// can be helpful in future
public class Slingshot : MonoBehaviour
{
    private enum SlingshotState { Null, Idle, Load, Aim, Fire }
    private      SlingshotState slingshotState = SlingshotState.Idle;

    private bool MachineOn;

    private Rigidbody2D rb2d;

    [SerializeField] private Bird[] birds;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform loadPoint;

    [SerializeField] private float stringRadius = 0.15f;

    [HideInInspector] public bool MouseDragged;
    [HideInInspector] public bool MouseReleased;

    private Queue<Bird> birdQueue = new Queue<Bird>();
    private Bird loadedBird;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        foreach (Bird bird in birds)
        {
            birdQueue.Enqueue(bird);
        }

        MachineOn = true;

        loadedBird = LoadNextBird();

        StartCoroutine(SlingshotStateMachine());
    }

    private void OnMouseDrag() { MouseDragged  = true; }
    private void OnMouseUp()   { MouseReleased = true; MouseDragged = false; }

    private IEnumerator Idle()
    {
        while (true)
        {
            if (loadedBird != null) { slingshotState = SlingshotState.Load; break; }

            yield return null;
        }
    }

    private IEnumerator Load()
    {
        MouseDragged = false;

        // Avoid OnMouseDrag conflict with Slingshot's Collider
        loadedBird.collider2d.enabled = false;

        // Make sure the Bird stationary on Slingshot
        loadedBird.rb2d.gravityScale = 0f;
        loadedBird.rb2d.velocity     = Vector2.zero;

        loadedBird.OnSlingshot();

        //loadedBird.transform.position = loadPoint.transform.position;

        while (Vector3.Distance(loadedBird.transform.position, loadPoint.transform.position) > 0.1f)
        {
            Vector3 moveDir = (loadPoint.transform.position - loadedBird.transform.position).normalized;

            loadedBird.transform.position += moveDir * 7f * Time.deltaTime;

            yield return null;
        }

        while (true)
        {
            if (MouseDragged) { slingshotState = SlingshotState.Aim; break; }

            yield return null;
        }
    }

    private IEnumerator Aim()
    {
        MouseReleased = false;

        loadedBird.ReadyToFly();

        while (true)
        {
            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragPos.z = 0f;

            if (Vector3.Distance(dragPos, this.transform.position) > stringRadius)
            {
                dragPos = (dragPos - this.transform.position).normalized * stringRadius + this.transform.position;
            }

            rb2d.MovePosition(dragPos);

            loadedBird.transform.position = this.transform.position;

            // Bird looks at the Aim Point
            loadedBird.transform.right = aimPoint.position - loadedBird.transform.position;

            if (MouseReleased) { slingshotState = SlingshotState.Fire; break; }

            yield return null;
        }  
    }

    private IEnumerator Fire()
    {
        loadedBird.collider2d.enabled = true;
        loadedBird.rb2d.gravityScale  = 1f;
        loadedBird.rb2d.velocity      = (aimPoint.position - this.transform.position) * 10;

        loadedBird.Fly();

        loadedBird = LoadNextBird();

        slingshotState = SlingshotState.Idle;

        yield return null;
    }

    private Bird LoadNextBird()
    {
        if (birdQueue.Count == 0)
            return null;

        return birdQueue.Dequeue();
    }

    private IEnumerator SlingshotStateMachine()
    {
        while (MachineOn)
        {
            //Debug.Log("Slingshot: " + slingshotState);
            switch (slingshotState)
            {
                case SlingshotState.Idle: yield return StartCoroutine(Idle()); break;
                case SlingshotState.Aim:  yield return StartCoroutine(Aim());  break;
                case SlingshotState.Load: yield return StartCoroutine(Load()); break;
                case SlingshotState.Fire: yield return StartCoroutine(Fire()); break;
            }
        }
    }
}
