using System;
using UnityEngine;

public class GhostNoteScript : NoteScript
{
    //[SerializeField] private float transparencySpeed;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void moveNote()
    {
        base.moveNote();

        Color currCol = sprite.material.color;
        
        currCol.a = Mathf.Lerp(currCol.a, 0, base.threshold);
    }
}
