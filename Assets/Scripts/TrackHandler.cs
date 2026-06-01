using System;
using UnityEngine;

// Handles spawning in the notes and moving them betwen the spawn point and the hit point

public class TrackHandler : MonoBehaviour
{

    [SerializeField] private GameObject note;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private ConductorScript conductor;

    // beat info
    private float spawnBeat;
    private float targetBeat = 1f; // the beat where the note should hit the line; should be a list but for testing it will be 1 value
    private float currentBeat;
    [SerializeField] private float shownBeats = 4f;

    // might have to create a list/dictionary of note types in the future depending if we want to have multiple note types

    void Start()
    {
        spawnBeat = targetBeat - shownBeats;
    }

    /* 
        TODO
        - Create a list/other method to contain the "chart" of the track
        - create a function that recalculates the target beat
        - solidify designs
    
    */
    void Update()
    {
        currentBeat = conductor.songPositionInBeats;
        
        transform.position = Vector2.Lerp(
        spawnPoint.position,
        hitPoint.position,
        (currentBeat - spawnBeat) / (targetBeat - spawnBeat)
    );
    }
}
