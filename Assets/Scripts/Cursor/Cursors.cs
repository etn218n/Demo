using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCursor", menuName = "Cursor/DefaultCursor", order = 1)]
public class DefaultCursor : CustomCursor
{
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected Sprite holdSprite;

    public override void Hold()    { sprRenderer.sprite = holdSprite;    }
    public override void Release() { sprRenderer.sprite = defaultSprite; }
}

[CreateAssetMenu(fileName = "UICursor", menuName = "Cursor/UICursor", order = 1)]
public class UICursor : CustomCursor
{
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected Sprite clickedSprite;

    public override void Click()   { sprRenderer.sprite = clickedSprite; }
    public override void Release() { sprRenderer.sprite = defaultSprite; }
}