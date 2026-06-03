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

    public float currentSongPosition;
    public static float shownBeats = 4f;

    public Queue<Chart> noteSpawns;

    public List<NoteScript> Notes = new List<NoteScript>();

    // EVENTS
    public delegate void OnNoteMissed();
    public delegate void OnNoteHit();

    public static OnNoteMissed onNoteMissed;

    public static OnNoteHit onNoteHit;

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
        //onNoteMissed/subscribes this function to the onNoteMissed event

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

        currentSongPosition = ConductorScript.Instance.songPosition;
        currentBeat = ConductorScript.Instance.songPositionInBeats;

        if (ConductorScript.Instance.songHasStarted)
        {
            spawnNotes();
        }

        // handle inputs
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame || UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame) //change this later
        {
            hitNote();
        }

        removeOldNotes();

    }

    void hitNote()
    {
        Debug.Log("press");
        if (Notes.Count > 0)
        {
            NoteScript upcomingNote = Notes[0];

            float adjSongPosition = currentSongPosition + (ConductorScript.Instance.songOffset / 1000f);

            float targetTime = upcomingNote.targetBeat * ConductorScript.Instance.secPerBeat;

            float diff = Mathf.Abs(targetTime - adjSongPosition);

            float msDiff = diff * 1000f;

            UIScript.diffDebug?.Invoke(msDiff);

            if (msDiff <= 70f)
            {
                Debug.Log("hit!");


                onNoteHit?.Invoke();

                Notes.Remove(upcomingNote);
                upcomingNote.destroyThisNote();
            }

        }
    }

    public void handleNoteMissed(NoteScript note)
    {
        if (Notes.Contains(note))
        {
            Debug.Log("miss!");
            Notes.Remove(note);
            onNoteMissed?.Invoke();
        }
    }

    void removeOldNotes()
    {
        while (Notes.Count > 0)
        {
            NoteScript upcomingNote = Notes[0];

            float adjSongPosition = currentSongPosition + (ConductorScript.Instance.songOffset / 1000f);

            float targetTime = upcomingNote.targetBeat * ConductorScript.Instance.secPerBeat;

            float purgeTime = targetTime + (125f / 1000f);

            if (adjSongPosition > purgeTime)
            {
                handleNoteMissed(upcomingNote);
            }

        }
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
                }
                else
                {
                    Debug.LogError("That note doesn't exist");
                }
            }
        }
    }
}
