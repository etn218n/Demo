using UnityEngine;

public abstract class CustomCursor : ScriptableObject
{
    [HideInInspector] public SpriteRenderer sprRenderer;
    [HideInInspector] public Animator anim;

    public virtual void Click() { }
    public virtual void Hold() { }
    public virtual void Release() { }
}
