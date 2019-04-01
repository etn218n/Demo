using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState { Null, Idle, OnSlingshot, Aim, Fly, Disappear }
    private      BirdState birdState;

    private bool MachineOn;

    [HideInInspector] public bool LoadintoSlingshot;
    [HideInInspector] public bool MouseDragged;
    [HideInInspector] public bool MouseReleased;

    public Rigidbody2D rb2d      { get; private set; }
    public Collider2D collider2d { get; private set; }

    private Animator anim;

    [Range(0, 20)] public float flySpeed = 10;

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
    }

    private IEnumerator Fly()
    {
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
        SetAnimation(BirdState.Disappear);

        // wait for Disappear Animation to complete
        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

    private IEnumerator BirdStateMachine()
    {
        while (MachineOn)
        {
            //Debug.Log("Bird: " + birdState);
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
