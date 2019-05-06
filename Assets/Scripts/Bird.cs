using UnityEngine;
using System;

public enum BirdState { Null, Idle, OnSlingshot, Ready, Flying, Disappearing }

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class Bird : MonoBehaviour
{
    private BirdState  state;

    public Animator    anim       { get; private set; }
    public Rigidbody2D rb2d       { get; private set; }
    public Collider2D  collider2d { get; private set; }

    public EventHandler OnStateChanged;
    public EventHandler OnDisappeared;

    public BirdState State
    {
        get => this.state;

        protected set
        {
            this.state = value;

            OnStateChanged?.Invoke(this, null);  
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
        OnDisappeared?.Invoke(this, null);
    }

    public virtual void Idle()           { }
    public virtual void OnSlingshot()    { }
    public virtual void ReadyToFly()     { }
    public virtual void Fly()            { }
    public virtual void SpecialAbility() { }
}
