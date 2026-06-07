using System;
using UnityEngine;

public class StealthNoteScript : NoteScript
{
    [Tooltip("Multiplier of how fast the ghost note disappears")]
    [SerializeField] private float appearSpeed = 1.33f;
    [SerializeField] private float curveMult = 1f;
    [SerializeField] private float rotationSpeed = 1.33f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void moveNote()
    {
        base.moveNote();

        Color currCol = sprite.color;
        
        float appearCurve = Mathf.Pow(threshold * appearSpeed, curveMult);
        
        currCol.a = Mathf.Lerp(0f, 1f, appearCurve);

        sprite.color = currCol;


        transform.Rotate(0,0, 360 * rotationSpeed * Time.deltaTime);
    }
}
