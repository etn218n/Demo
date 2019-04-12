using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private enum SlingshotState { Null, Idle, Load, Aim, Fire }
    private      SlingshotState slingshotState = SlingshotState.Idle;

    private bool MachineOn;

    private Rigidbody2D rb2d;

    [SerializeField] private Bird bird;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform loadPoint;

    [SerializeField] private float stringRadius = 0.15f;

    [HideInInspector] public bool MouseDragged;
    [HideInInspector] public bool MouseReleased;
    [HideInInspector] public bool BirdLoaded;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MachineOn  = true;
        BirdLoaded = true;

        StartCoroutine(SlingshotStateMachine());
    }

    private void OnMouseDrag() { MouseDragged  = true; }
    private void OnMouseUp()   { MouseReleased = true; MouseDragged = false; }

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

        // Avoid OnMouseDrag conflict with Slingshot's Collider
        bird.collider2d.enabled = false;

        // Make sure the Bird stationary on Slingshot
        bird.rb2d.gravityScale = 0f;
        bird.rb2d.velocity     = Vector2.zero;

        bird.OnSlingshot();

        bird.transform.position = loadPoint.transform.position;

        while (true)
        {
            if (MouseDragged) { slingshotState = SlingshotState.Aim; break; }

            yield return null;
        }
    }

    private IEnumerator Aim()
    {
        MouseReleased = false;

        bird.ReadyToFly();

        while (true)
        {
            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragPos.z = 0f;

            if (Vector3.Distance(dragPos, this.transform.position) > stringRadius)
            {
                dragPos = (dragPos - this.transform.position).normalized * stringRadius + this.transform.position;
            }

            rb2d.MovePosition(dragPos);

            bird.transform.position = this.transform.position;

            // Bird looks at the Aim Point
            bird.transform.right = aimPoint.position - bird.transform.position;

            if (MouseReleased) { slingshotState = SlingshotState.Fire; break; }

            yield return null;
        }  
    }

    private IEnumerator Fire()
    {
        bird.collider2d.enabled = true;

        bird.rb2d.gravityScale  = 1f;

        bird.rb2d.velocity = (aimPoint.position - this.transform.position) * 10;

        bird.Fly();

        BirdLoaded = false;

        slingshotState = SlingshotState.Idle;

        yield return null;
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
