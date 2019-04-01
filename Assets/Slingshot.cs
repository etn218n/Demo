using System.Collections;
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

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform loadPoint;

    [HideInInspector] public bool MouseDragged;
    [HideInInspector] public bool MouseReleased;
    [HideInInspector] public bool BirdLoaded;

    public float stringRadius;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        bird = birdObject.GetComponent<Bird>();

        stringRadius = 0.15f;
    }

    private void Start()
    {
        MachineOn = true;
        slingshotState = SlingshotState.Idle;

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

        bird.rb2d.gravityScale = 0f;
        bird.rb2d.velocity     = Vector2.zero;

        bird.LoadintoSlingshot = true;

        bird.gameObject.transform.position = loadPoint.transform.position;
       

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
            Vector3 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragPos.z = 0f;

            if (Vector3.Distance(dragPos, transform.position) > stringRadius)
            {
                dragPos = (dragPos - transform.position).normalized * stringRadius + transform.position;
            }

            rb2d.MovePosition(dragPos);

            bird.gameObject.transform.position = gameObject.transform.position;

            // Bird looks at the Aim Point
            bird.gameObject.transform.right = aimPoint.position - birdObject.transform.position;

            if (MouseReleased) { slingshotState = SlingshotState.Fire; break; }

            yield return null;
        }  
    }

    private IEnumerator Fire()
    {
        bird.collider2d.enabled = true;

        bird.rb2d.gravityScale = 1f;

        bird.rb2d.velocity = (aimPoint.position - gameObject.transform.position) * bird.flySpeed;

        bird.MouseReleased = true;

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
