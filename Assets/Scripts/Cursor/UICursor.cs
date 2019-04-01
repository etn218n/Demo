using UnityEngine;

[CreateAssetMenu(fileName = "UICursor", menuName = "Cursor/UICursor")]
public class UICursor : CustomCursor
{
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected Sprite clickedSprite;

    public override void Click()   { sprRenderer.sprite = clickedSprite; }
    public override void Release() { sprRenderer.sprite = defaultSprite; }
}