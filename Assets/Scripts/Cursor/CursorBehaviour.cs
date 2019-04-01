using UnityEngine;
using System.Collections;

public class CursorBehaviour : MonoBehaviour
{
    private SpriteRenderer sprRenderer;
    private CustomCursor currentCursor;

    private float clickStartTime;

    [SerializeField] private CustomCursor[] customCursors;

    private void Awake()
    {
        Cursor.visible = false;

        sprRenderer = GetComponent<SpriteRenderer>();
        
        foreach (var cursor in customCursors)
        {
            cursor.sprRenderer = sprRenderer;
        }

        currentCursor = customCursors[0];
        currentCursor.Release();
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position    = mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            clickStartTime = Time.time;
        }
        else if (Input.GetMouseButton(0))
        {
            float clickTime = Time.time - clickStartTime;

            if (clickTime > 0.15f) currentCursor.Hold();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float clickTime = Time.time - clickStartTime;

            if (clickTime < 0.15f) currentCursor.Click();

            StartCoroutine(ReleaseMouse());
        }
    }

    private IEnumerator ReleaseMouse()
    {
        yield return new WaitForSeconds(0.1f);

        currentCursor.Release();
    }

    public void SetCursor(CustomCursor cursor)
    {
        currentCursor = cursor;
        currentCursor.Release();
    }
}

public abstract class CustomCursor : ScriptableObject
{
    [HideInInspector] public SpriteRenderer sprRenderer;
    [HideInInspector] public Animator anim;

    public virtual void Click()   { }
    public virtual void Hold()    { }
    public virtual void Release() { }
}
