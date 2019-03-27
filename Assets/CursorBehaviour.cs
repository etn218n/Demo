using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    private Animator anim;

    // Adjust tip of finger cursor to match hardware cursor
    [SerializeField] private Vector2 offset = new Vector2(0.2f, -0.3f);

    private void Awake()
    {
        Cursor.visible = false;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position    = mousePosition + offset;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Clicked", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Clicked", false);
        }

    }
}
