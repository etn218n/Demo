using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private enum SlingshotState { Null, Idle, Load, Aim, Fire }
    private      SlingshotState slingshotState;

    private bool MachineOn;

    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject birdObject;
    private Bird bird;

    [SerializeField]
    private Transform aimPoint;

    [SerializeField]
    private Transform loadPoint;

    public bool MouseDragged;
    public bool MouseReleased;
    public bool BirdLoaded;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        bird = birdObject.GetComponent<Bird>();
    }

    private void Start()
    {
        MachineOn = true;
        slingshotState = SlingshotState.Idle;

        StartCoroutine(SlingshotStateMachine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            BirdLoaded = true;
        }
    }

    private void OnMouseDrag() { MouseDragged  = true; }
    private void OnMouseUp()   { MouseReleased = true; }

    private IEnumerator Idle()
    {
        while (true)
        {
            if (BirdLoaded) { slingshotState = SlingshotState.Load; break; }

            yield return null;
        }
    }

    private IEnumerator Load()
    {
        MouseDragged = false;

        bird.LoadintoSlingshot = true;

        bird.gameObject.transform.position = loadPoint.transform.position;

        // Avoid OnMouseDrag conflict with Slingshot's Collider
        bird.collider2d.enabled = false;

        bird.rb2d.gravityScale  = 0f;

        while (true)
        {
            if (MouseDragged) { slingshotState = SlingshotState.Aim; break; }

            yield return null;
        }

        bird.MouseDragged = true;
    }

    private IEnumerator Aim()
    {
        MouseReleased = false;

        while (true)
        {
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            rb2d.MovePosition(dragPos);

            bird.gameObject.transform.position = gameObject.transform.position;

            // Bird looks at the Aim Point
            bird.gameObject.transform.right    = aimPoint.position - birdObject.transform.position;

            if (MouseReleased) { slingshotState = SlingshotState.Fire; break; }

            yield return null;
        }

        bird.MouseReleased = true;
    }

    private IEnumerator Fire()
    {
        bird.collider2d.enabled = true;

        bird.rb2d.gravityScale  = 1f;

        if (bird.rb2d.gravityScale == 0f)
            Debug.Log("SHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");

        bird.rb2d.velocity      = (aimPoint.position - gameObject.transform.position) * bird.flySpeed;

        slingshotState = SlingshotState.Idle;

        yield return null;
    }

    private IEnumerator SlingshotStateMachine()
    {
        while (MachineOn)
        {
            Debug.Log("Slingshot: " + slingshotState);
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
