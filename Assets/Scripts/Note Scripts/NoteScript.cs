using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Tooltip("the target beat of when the note should hit the hitpoint. This value is automatically adjusted by TrackHandler")]
    public float targetBeat;

    [Tooltip("Value of the individual note")]
    public float pointValue;

    [Tooltip("The % threshold for when the note is removed after passing the hitpoint. Base value is 1.2 (20% past the hit point the note is removed)")]
    [SerializeField] protected float removeThreshold = 1.2f;

    protected float threshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveNote();
    }

    protected virtual void moveNote()
    {
        float secOffset = ConductorScript.Instance.songOffset / 1000f;
        float beatOffset = secOffset / ConductorScript.Instance.secPerBeat;
        float fakeCurrentBeat = TrackHandler.currentBeat + beatOffset;
        
        threshold = (TrackHandler.shownBeats - (targetBeat - fakeCurrentBeat)) / TrackHandler.shownBeats; //used for interpolation

        transform.position = Vector2.LerpUnclamped(
        TrackHandler.Instance.spawnPoint.position,
        TrackHandler.Instance.hitPoint.position,
        threshold
    );

        // if the note moves past the hit point by the remove threshold, remove it and invoke all functions subscribes to onNoteMissed
        if (threshold > removeThreshold)
        {   
            TrackHandler.Instance.handleNoteMissed(this);     
            destroyThisNote();
        }
    }
    
    
    public void destroyThisNote()
    {
        Destroy(gameObject);
    }
}
