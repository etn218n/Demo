using UnityEngine;

[CreateAssetMenu(fileName = "UICursor", menuName = "Cursor/UICursor", order = 1)]
public class UICursor : CustomCursor
{
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected Sprite clickedSprite;

    public override void Click()   { sprRenderer.sprite = clickedSprite; }
    public override void Release() { sprRenderer.sprite = defaultSprite; }
}