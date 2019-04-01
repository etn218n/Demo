using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayCursor : CustomCursor
{
    private readonly SpriteRenderer sprRenderer;

    private readonly Sprite defaultCursor;
    private readonly Sprite clickCursor;
    private readonly Sprite holdCursor;

    public PlayCursor(SpriteRenderer sprRenderer,
                      Sprite defaultCursor, 
                      Sprite clickCursor, 
                      Sprite holdCursor)
    {
        this.sprRenderer = sprRenderer;

        this.defaultCursor = defaultCursor;
        this.clickCursor   = clickCursor;
        this.holdCursor    = holdCursor;
    }

    public override void Click()
    {
        sprRenderer.sprite = clickCursor;
    }

    public override void Hold()
    {
        sprRenderer.sprite = holdCursor;
    }

    public override void Release()
    {
        sprRenderer.sprite = defaultCursor;
    }
}

public class UICursor : CustomCursor
{
    private readonly SpriteRenderer sprRenderer;

    private readonly Sprite defaultCursor;
    private readonly Sprite clickCursor;
    private readonly Sprite holdCursor;

    public UICursor(SpriteRenderer sprRenderer,
                      Sprite defaultCursor,
                      Sprite clickCursor,
                      Sprite holdCursor)
    {
        this.sprRenderer = sprRenderer;

        this.defaultCursor = defaultCursor;
        this.clickCursor = clickCursor;
        this.holdCursor = holdCursor;
    }

    public override void Click()
    {
        sprRenderer.sprite = clickCursor;
    }

    public override void Hold()
    {
        sprRenderer.sprite = holdCursor;
    }

    public override void Release()
    {
        sprRenderer.sprite = defaultCursor;
    }
}
