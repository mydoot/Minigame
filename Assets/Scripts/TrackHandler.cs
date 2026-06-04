using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.IO;

// Handles spawning in the notes and moving them betwen the spawn point and the hit point, a "2nd game manager"

public class TrackHandler : MonoBehaviour
{
    public static TrackHandler Instance { get; private set; }

    [Header("Note Types")]
    // PRIVATE VARIABLES
    [SerializeField] private NoteScript note;
    [SerializeField] private GhostNoteScript ghostNote;
    [SerializeField] private CapsuleNoteScript capsuleNote;

    [Header("Hit Point and Spawn Point Objects")]
    [SerializeField] public Transform hitPoint;
    [SerializeField] public Transform spawnPoint;

    // PRIVATE DATA TYPES
    private Dictionary<noteType, NoteScript> noteDictionary = new Dictionary<noteType, NoteScript>();
    
    // PUBLIC STATIC VARIABLES
    public static float currentBeat;

    public static float currentSongPosition;
    public static float shownBeats = 4f;

    // PUBLIC DATA TYPES
    [Header("Public Data Types")]
    public Queue<Chart> noteSpawns;

    public List<NoteScript> Notes = new List<NoteScript>();

    // EVENTS
    public delegate void OnNoteMissed();
    public delegate void OnNoteHit();

    public static OnNoteMissed onNoteMissed;

    public static OnNoteHit onNoteHit;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI debugText;

    [Header("Score")]
    public int hitCount = 0;
    public int missedCount = 0;


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

        //charting
        noteSpawns = new Queue<Chart>(ConductorScript.Instance.Song.chart);

        //this needs to be updated for every new note type
        noteDictionary.Add(noteType.Note, note);
        noteDictionary.Add(noteType.GhostNote, ghostNote);
        noteDictionary.Add(noteType.CapsuleNote, capsuleNote);
    }


    void Update()
    {
        debugText.text = $"Current Beat: {ConductorScript.Instance.songPositionInBeats:F2} | Song Duration Elapsed: {ConductorScript.Instance.songPosition:F2} | ms per beat {ConductorScript.Instance.secPerBeat * 1000f:F2} | Notes to spawn: {noteSpawns.Count} | Hits: {hitCount} | Misses: {missedCount} |";


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
        Debug.Log($"clicked on beat {ConductorScript.Instance.songPositionInBeats}");
        if (Notes.Count > 0)
        {
            NoteScript upcomingNote = Notes[0];

            float adjSongPosition = currentSongPosition + (ConductorScript.Instance.globalOffset / 1000f);

            float targetTime = upcomingNote.targetBeat * ConductorScript.Instance.secPerBeat;

            Debug.Log($"note target beat: {upcomingNote.targetBeat:F2}");

            float diff = Mathf.Abs(targetTime - adjSongPosition);

            float msDiff = diff * 1000f; //turn diff into millisecond value

            UIScript.diffDebug?.Invoke(msDiff);

            if (upcomingNote.returnCurrentHealth() > 1)
            {
                float largeTargetWindow = ConductorScript.Instance.secPerBeat * 1000f;
                if (msDiff <= largeTargetWindow + 150f) //adding in a temporary 150ms of extra padding for health notes
                {
                    Debug.Log("hit!");
                    hitCount++;
                    onNoteHit?.Invoke();

                    bool noteDead = upcomingNote.takeDamage();

                    if (noteDead)
                    {
                            Notes.Remove(upcomingNote);
                            upcomingNote.destroyThisNote();
                    }
                    
                }
            }
            else
            {
                if (msDiff <= 70f)
                {
                    Debug.Log("hit!");
                    onNoteHit?.Invoke();

                    bool noteDead = upcomingNote.takeDamage();

                    if (noteDead)
                    {
                        Notes.Remove(upcomingNote);
                        upcomingNote.destroyThisNote();
                    }
                }
            }

            /* else if (msDiff <= 125f)
            {
                Debug.Log("bad!");
                onNoteMissed?.Invoke();
                
                handleNoteMissed(upcomingNote);
            } */
        }
    }

    public void handleNoteMissed(NoteScript note)
    {
        if (Notes.Contains(note))
        {
            Debug.Log("miss!");
            missedCount++;
            Notes.Remove(note);
            onNoteMissed?.Invoke();

            //additionally add logic for reducing the PLAYER health
        }
    }

    void removeOldNotes()
    {

        while (Notes.Count > 0)
        {
            NoteScript upcomingNote = Notes[0];

            float adjSongPosition = currentSongPosition + (ConductorScript.Instance.globalOffset / 1000f);

            float targetTime = upcomingNote.targetBeat * ConductorScript.Instance.secPerBeat;

            float purgeTime = targetTime + (125f / 1000f);

            if (adjSongPosition > purgeTime)
            {
                handleNoteMissed(upcomingNote);
            }
            else
            {
                break;
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
