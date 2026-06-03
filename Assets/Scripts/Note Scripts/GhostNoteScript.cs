using System;
using UnityEngine;

public class GhostNoteScript : NoteScript
{
    [Tooltip("Multiplier of how fast the ghost note disappears")]
    [SerializeField] private float transparencySpeed = 1.33f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void moveNote()
    {
        base.moveNote();

        Color currCol = sprite.color;
        
        currCol.a = Mathf.Lerp(1f, 0f, threshold * transparencySpeed);

        sprite.color = currCol;
    }
}
