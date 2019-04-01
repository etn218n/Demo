using UnityEngine;
using System.Collections;

public class CursorBehaviour : MonoBehaviour
{
    private SpriteRenderer sprRenderer;

    private float clickStartTime;

    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite clickCursor;
    [SerializeField] private Sprite holdCursor;
    [SerializeField] private Sprite UICursor;

    public CustomCursor customCursor;

    public CustomCursor playCursor;
    public CustomCursor uiCursor;

    public void ToUICursor()
    {
        customCursor = uiCursor;
        customCursor.Release();
    }

    public void ToGameCursor()
    {
        customCursor = playCursor;
        customCursor.Release();
    }

    private void Awake()
    {
        Cursor.visible = false;

        sprRenderer = GetComponent<SpriteRenderer>();

        playCursor = new PlayCursor(sprRenderer, defaultCursor, defaultCursor, holdCursor);
        uiCursor = new UICursor(sprRenderer, UICursor, clickCursor, UICursor);

        customCursor = playCursor;
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

            if (clickTime > 0.15f) customCursor.Hold();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float clickTime = Time.time - clickStartTime;

            if (clickTime < 0.15f) customCursor.Click();

            StartCoroutine(ReleaseMouse());
        }
    }

    private IEnumerator ReleaseMouse()
    {
        yield return new WaitForSeconds(0.1f);

        customCursor.Release();
    }
}
