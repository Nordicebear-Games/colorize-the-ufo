﻿using UnityEngine;

public class SpaceObject : MonoBehaviour, ISpaceObject
{
    [Header("Object")]
    public float objectSpeed = 2f;
    public float makeKinematicBorder = -4.3f;

    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnBecameInvisible() //screen exited
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        MakeObjectKinematic();
    }

    public void ObjectMovement()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0, -objectSpeed);
    }

    public void MakeObjectKinematic() //iki kinematic obje arasında collision çalışmadığı için böyle bir çözüm buldum.
    {
        if (transform.position.y <= makeKinematicBorder)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
