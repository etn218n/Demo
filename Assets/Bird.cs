﻿using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState { Null, Idle, OnSlingshot, Aim, Fly, Disappear }

    private BirdState birdState;

    private bool MachineOn;

    public bool LoadintoSlingshot;
    public bool MouseDragged;
    public bool MouseReleased;

    public Rigidbody2D rb2d      { get; private set; }
    public Collider2D collider2d { get; private set; }

    private Animator anim;

    [SerializeField]
    private Transform aimPoint;

    [Range(0, 20)]
    public float flySpeed = 10;

    private void Awake()
    {
        rb2d       = GetComponent<Rigidbody2D>();
        anim       = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Start()
    {
        birdState = BirdState.Idle;
        MachineOn = true;

        StartCoroutine(BirdStateMachine());
    }

    private IEnumerator Idle()
    {
        SetAnimation(BirdState.Idle);

        while (true)
        {
            if (LoadintoSlingshot) { birdState = BirdState.OnSlingshot; break; }

            yield return null;
        }
    }

    private IEnumerator OnSlingshot()
    {
        SetAnimation(BirdState.OnSlingshot);

        // Avoid OnMouseDrag conflict with Slingshot's Collider Aim
        collider2d.enabled = false;

        rb2d.gravityScale = 0f;

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

        // Update State
        while (true)
        {
            if (MouseReleased) { birdState = BirdState.Fly; break; }

            yield return null;
        }

        //Exit State
        collider2d.enabled = true;
    }

    private IEnumerator Fly()
    {
        rb2d.gravityScale = 1f;

        rb2d.velocity = (aimPoint.position - gameObject.transform.position) * flySpeed;

        yield return new WaitForSeconds(0.2f);

        SetAnimation(BirdState.Fly);

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
        rb2d.gravityScale = 0f;

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
                case BirdState.Idle:        yield return StartCoroutine(Idle());        break;
                case BirdState.OnSlingshot: yield return StartCoroutine(OnSlingshot()); break;
                case BirdState.Aim:         yield return StartCoroutine(Aim());         break;
                case BirdState.Fly:         yield return StartCoroutine(Fly());         break;
                case BirdState.Disappear:   yield return StartCoroutine(Disappear());   break;
            }
        }
    }

    private void SetAnimation(BirdState state) { anim.SetInteger("StateID", (int)state); }
}
