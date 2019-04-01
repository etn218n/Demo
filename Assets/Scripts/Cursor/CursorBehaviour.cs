using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] private CustomCursor[] customCursors;

    private SpriteRenderer sprRenderer;
    private CustomCursor currentCursor;

    private float clickStartTime;

    private void Awake()
    {
        Cursor.visible = false;

        sprRenderer = GetComponent<SpriteRenderer>();

        sprRenderer.sortingOrder = 5;

        foreach (var cursor in customCursors)
        {
            cursor.sprRenderer = sprRenderer;
        }

        SetCursor(customCursors[0]);
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
