using UnityEngine;

public class NoteScript : MonoBehaviour
{
    public float targetBeat;
    public float pointValue;

    [SerializeField] private float removeThreshold = 1.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveNote();
    }

    void moveNote()
    {
        float threshold = (TrackHandler.shownBeats - (targetBeat - TrackHandler.currentBeat)) / TrackHandler.shownBeats;

        transform.position = Vector2.LerpUnclamped(
        TrackHandler.Instance.spawnPoint.position,
        TrackHandler.Instance.hitPoint.position,
        threshold
    );

        if (threshold > removeThreshold)
        {
            TrackHandler.onNoteMissed?.Invoke();
            Destroy(gameObject);
        }
    }
}
