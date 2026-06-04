using UnityEngine;

public class CharacterSounds : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip whiffSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrackHandler.onNoteHit += playHitSound;

        audioSource = GetComponent<AudioSource>();
    }

    public void playHitSound()
    {
        audioSource.pitch = Random.Range(0.85f, 1.10f);

        audioSource.PlayOneShot(hitSound, 0.30f);
    }
    public void playWhiffSound()
    {
        audioSource.pitch = Random.Range(1.15f, 1.35f);

        audioSource.PlayOneShot(whiffSound, 0.25f);
    }
}
