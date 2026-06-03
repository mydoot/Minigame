using UnityEngine;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public enum noteType
{
    Note,
    GhostNote,
    CapsuleNote
}

public class ConductorScript : MonoBehaviour
{
    public static ConductorScript Instance { get; private set; } 

    [SerializeField] public SongObject Song;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    [Tooltip("The number of seconds for each song beat")]
    public float secPerBeat;

    //Current song position, in seconds
    [Tooltip("current song position in seconds, adjusted for possible song offsets")]
    public float songPosition;

     [Tooltip("current song positio in seconds ignoring possible song offsets")]
    public float absoluteSongPosition;

    //Current song position, in beats
    [Tooltip("current song position in beats")]
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    [Tooltip("The amount of seconds that have passed since song started (universal); Rely on songPosition and songPositionInBeats for specific song elapsed time")]
    public float dspSongTime;

    [Tooltip("The amount of delay before the song actually starts. The minimum should be 2s.")]
    [SerializeField] private float delay = 2f;

    [Tooltip("Global offset in milliseconds. Used for better game feel")]
    public float globalOffset;
    
    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    public bool songHasStarted = false;

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
        songHasStarted = false;

        //loading SongObject data
        musicSource.clip = Song.trackData;
        songBpm = Song.BPM;

        //Calculate the number of seconds in each beat
        // duration of a quarter note
        secPerBeat = 60f / songBpm;

        //StartCoroutine(waitForStart(0));

        beginSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (songHasStarted)
        {
            absoluteSongPosition = (float)(AudioSettings.dspTime - dspSongTime);
            
            //determine how many seconds since the song started
            songPosition = absoluteSongPosition - Song.songOffset;

            //determine how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;
        }
    }

    void beginSong()
    {   
        if (Song.songOffset > 0)
        {
            Debug.Log($"adjusted individual song offset due to delayed start by {Song.songOffset}");
        }

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime + delay;

        Debug.Log($"song starts in {delay}");

        //Start the music
        musicSource.PlayScheduled(dspSongTime);
        
        songHasStarted = true;
    }

   

    
}
