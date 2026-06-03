using UnityEngine;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Collections;

/* 
FUTURE CHANGES
 - Change loading the audio to use a scriptable object to easily load both the music and it's BPM (+ other things like song name and jacket art, etc)

*/

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
    [Tooltip("current song position in seconds")]
    public float songPosition;

    //Current song position, in beats
    [Tooltip("current song position in beats")]
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    [Tooltip("The amount of seconds that have passed since song started (universal); Rely on songPosition and songPositionInBeats for specific song elapsed time")]
    public float dspSongTime;

    [Tooltip("The amount of delay before the song actually starts. The minimum should be 2s.")]
    [SerializeField] private float delay = 2f;

    [Tooltip("Global offset in milliseconds. Used for better game feel")]
    public float songOffset;

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
        secPerBeat = 60f / songBpm;

        //StartCoroutine(waitForStart(0));

        beginSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (songHasStarted)
        {
            //determine how many seconds since the song started
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);

            //determine how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;
        }
    }

    /* IEnumerator waitForStart(float offset)
    {
        yield return new WaitForSeconds(2f + offset); //wait 1s + offset of the song before we actually begin
        Debug.Log("songhasstarted");
        songHasStarted = true;
        beginSong();
    } */

    void beginSong()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime + delay;

        Debug.Log($"song starts in {delay}");

        //Start the music
        musicSource.PlayScheduled(dspSongTime);
        
        songHasStarted = true;
    }
}
