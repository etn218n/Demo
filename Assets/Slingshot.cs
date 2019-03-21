﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject birdObject;
    private Bird bird;

    [SerializeField]
    private Transform aimPoint;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        bird = birdObject.GetComponent<Bird>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            birdObject.transform.position = aimPoint.position;

            bird.LoadintoSlingshot = true;
            bird.rb2d.gravityScale = 0f;
        }
    }

    private void OnMouseDrag()
    {
        if (birdObject != null && bird.LoadintoSlingshot == true)
        {
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            rb2d.MovePosition(dragPos);

            bird.transform.position = gameObject.transform.position;

            // Bird looks at the Aim Point
            birdObject.transform.right = aimPoint.position - birdObject.transform.position;

            bird.MouseDragged = true;
        }
    }

    private void OnMouseUp()
    {
        if (birdObject != null && bird.MouseDragged == true)
        {
            bird.MouseReleased = true;
            birdObject = null;

            bird.rb2d.velocity = (aimPoint.position - gameObject.transform.position) * bird.flySpeed;
        }
    }

}
