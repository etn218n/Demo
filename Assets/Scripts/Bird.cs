using UnityEngine;

public enum BirdState { Null, Idle, OnSlingshot, Ready, Flying, Disappearing }

public delegate void BirdEvent();

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class Bird : MonoBehaviour
{
    private BirdState  state;

    public Animator    anim       { get; private set; }
    public Rigidbody2D rb2d       { get; private set; }
    public Collider2D  collider2d { get; private set; }

    public event BirdEvent StateChanged;
    public event BirdEvent Disappeared;

    public BirdState State
    {
        get => this.state;

        protected set
        {
            this.state = value;

            StateChanged?.Invoke();
        }
    }

    private void Awake()
    {
        anim       = GetComponent<Animator>();
        rb2d       = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();  
    }

    protected void Disappear()
    {
        Disappeared?.Invoke();
    }

    public virtual void Idle()           { }
    public virtual void OnSlingshot()    { }
    public virtual void ReadyToFly()     { }
    public virtual void Fly()            { }
    public virtual void SpecialAbility() { }
}
