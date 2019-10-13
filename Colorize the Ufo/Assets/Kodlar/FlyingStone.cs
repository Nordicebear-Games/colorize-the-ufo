﻿using UnityEngine;

public class FlyingStone : SpaceObject {

    [SerializeField]
    private Color[] colors;

    private new void Start()
    {
        base.Start();
        chooseColor();
    }

    private new void Update()
    {
        base.Update();
        rotateObject();
    }

    void rotateObject()
    {
        transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)) * Time.deltaTime);
    }

    private void chooseColor()
    {
        if (colors.Length != 0)
        {
            spriteRenderer.color = colors[Random.Range(0, colors.Length)];
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "engellerTag" ||
            col.transform.tag == "meteorTag" ||
            col.transform.tag == "ucanCisimTag" ||
            col.transform.tag == "canVerenCisimTag" ||
            col.transform.tag == "enerjiVerenCisimTag" ||
            col.transform.tag == "yavaslatanCisimTag")
        {
            Destroy(transform.gameObject);
        }
    }
}
