using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// Handles spawning in the notes and moving them betwen the spawn point and the hit point, a "2nd game manager"

public class TrackHandler : MonoBehaviour
{
    public static TrackHandler Instance { get; private set; } 

    private Dictionary<noteType, NoteScript> noteDictionary = new Dictionary<noteType, NoteScript>();

    // PRIVATE VARIABLES
    [Tooltip("Note prefab")]
    [SerializeField] private NoteScript note;
    [SerializeField] private GhostNoteScript ghostNote;

    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] public Transform hitPoint;
    [SerializeField] public Transform spawnPoint;
    
    // PUBLIC STATIC VARIABLES
    public static float currentBeat;
    public static float shownBeats = 4f;

    public Queue<Chart> noteSpawns;

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
        onNoteMissed += handleNoteMissed; //subscribes this function to the onNoteMissed event

        //temp charting
        noteSpawns = new Queue<Chart>(ConductorScript.Instance.Song.chart);

        noteDictionary.Add(noteType.Note, note);
        noteDictionary.Add(noteType.GhostNote, ghostNote);
    }

    /* 
        TODO
        - Create a list/other method to contain the "chart" of the track
        - create a function that recalculates the target beat
        - solidify designs

    */
    void Update()
    {
        debugText.text = $"Current Beat: {ConductorScript.Instance.songPositionInBeats} | Song Duration Elapsed: {ConductorScript.Instance.songPosition} | songHasStarted:{ConductorScript.Instance.songHasStarted} | Notes to spawn: {noteSpawns.Count}";

        currentBeat = ConductorScript.Instance.songPositionInBeats;

        if (ConductorScript.Instance.songHasStarted)
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
            // looks at the next target beat in the noteSpawns list
            float nextTargetBeat = noteSpawns.Peek().targetBeat;
            noteType chosenNote = noteSpawns.Peek().type;

            // if the note can be spawned from the current beat and also within the next 4, spawn the note
            if ((currentBeat + shownBeats) >= nextTargetBeat)
            {
                if (noteDictionary.TryGetValue(chosenNote, out NoteScript obj))
                {
                NoteScript Note = Instantiate(obj, spawnPoint.position, Quaternion.identity);
                Note.targetBeat = nextTargetBeat;
                Notes.Add(Note);

                noteSpawns.Dequeue();   
                } else
                {
                    Debug.LogError("That note doesn't exist");
                }
            }
        }
        else
        {
            Debug.LogWarning("No more notes to spawn!");
        }
    }
}
