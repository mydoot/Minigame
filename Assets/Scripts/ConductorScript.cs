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

    [SerializeField] private SongObject Song;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    public bool songHasStarted = false;

    void Start()
    {
        songHasStarted = false;

        //loading SongObject data
        musicSource.clip = Song.trackData;
        songBpm = Song.BPM;

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        waitForStart(0);
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

    IEnumerator waitForStart(float offset)
    {
        yield return new WaitForSeconds(2f + offset); //wait 1s + offset of the song before we actually begin
        Debug.Log("songhasstarted");
        songHasStarted = true;
        beginSong();
    }

    void beginSong()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }
}
