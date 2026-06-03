using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles spawning in the notes and moving them betwen the spawn point and the hit point, a "2nd game manager"

public class TrackHandler : MonoBehaviour
{
    public static TrackHandler Instance { get; private set; } //Turns GameManager into a singleton

    // PRIVATE VARIABLES
    [SerializeField] private NoteScript note;

    [SerializeField] private ConductorScript conductor;

    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] public Transform hitPoint;
    [SerializeField] public Transform spawnPoint;
    
    // PUBLIC STATIC VARIABLES
    public static float currentBeat;
    public static float shownBeats = 4f;

    public Queue<float> noteSpawns = new Queue<float>();

    public List<NoteScript> Notes = new List<NoteScript>();

    // EVENTS
    public delegate void OnNoteMissed();

    public static OnNoteMissed onNoteMissed;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        onNoteMissed += handleNoteMissed;

        //temp charting
        noteSpawns.Enqueue(1f);
        noteSpawns.Enqueue(2f);
        noteSpawns.Enqueue(4f);
        noteSpawns.Enqueue(5f);
        noteSpawns.Enqueue(6f);
        noteSpawns.Enqueue(7f);
        noteSpawns.Enqueue(8f);

    }

    /* 
        TODO
        - Create a list/other method to contain the "chart" of the track
        - create a function that recalculates the target beat
        - solidify designs

    */
    void Update()
    {
        debugText.text = $"Current Beat: {currentBeat} | Song Duration Elapsed: {conductor.songPosition} | songHasStarted:{conductor.songHasStarted}";

        currentBeat = conductor.songPositionInBeats;

        if (conductor.songHasStarted)
        {
            spawnNotes();
        }

    }

    void handleNoteMissed()
    {
        Debug.Log("note missed!");
    }

    void spawnNotes()
    {
        if (noteSpawns.Count > 0)
        {
            //Debug.Log(noteSpawns.Count);
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
