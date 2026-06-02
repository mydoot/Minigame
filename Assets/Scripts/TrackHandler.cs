using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles spawning in the notes and moving them betwen the spawn point and the hit point

public class TrackHandler : MonoBehaviour
{

    [SerializeField] private NoteScript note;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private ConductorScript conductor;

    [SerializeField] private TextMeshProUGUI debugText;

    // beat info
    private float spawnBeat;
    private float targetBeat = 1f; // the beat where the note should hit the line; should be a list but for testing it will be 1 value
    private float currentBeat;
    [SerializeField] private float shownBeats = 4f;

    public Queue<float> noteSpawns = new Queue<float>();

    public List<NoteScript> Notes = new List<NoteScript>();

    //public List

    void Start()
    {
        noteSpawns.Enqueue(1f);
        noteSpawns.Enqueue(2f);
        noteSpawns.Enqueue(4f);

    }

    /* 
        TODO
        - Create a list/other method to contain the "chart" of the track
        - create a function that recalculates the target beat
        - solidify designs

    */
    void Update()
    {
        debugText.text = $"Current Beat: {currentBeat} | songHasStarted:{conductor.songHasStarted}";

        if (conductor.songHasStarted)
        {
            currentBeat = conductor.songPositionInBeats;

            foreach (NoteScript NOTE in Notes)
            {
                NOTE.transform.position = Vector2.Lerp(
                spawnPoint.position,
                hitPoint.position,
                (shownBeats - (NOTE.targetBeat - currentBeat)) / shownBeats
            );
            }


            spawnNotes();
        }



    }

    void spawnNotes()
    {
        if (noteSpawns.Count > 0)
        {
            Debug.Log(noteSpawns.Count);
            float nextTargetBeat = noteSpawns.Peek();

            if ((currentBeat + shownBeats) >= nextTargetBeat)
            {
                NoteScript Note = Instantiate(note, spawnPoint.position, Quaternion.identity);
                Note.targetBeat = nextTargetBeat;
                Notes.Add(Note);

                noteSpawns.Dequeue();
            }
        }
        else
        {
            Debug.LogWarning("No more notes to spawn!");
        }
    }
}
