using System;
using UnityEngine;

public class TriangleNoteScript : NoteScript
{
    [Tooltip("Multiplier of how fast the capsule note rotates")]
    [SerializeField] private float rotationSpeed = 1.33f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        base.healthValue = 3;
    }

    protected override void moveNote()
    {
        base.moveNote();

        transform.Rotate(0,0, 360 * rotationSpeed * Time.deltaTime);
    }
}
