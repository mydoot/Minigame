using Unity.VisualScripting;
using UnityEngine;

public class LoadSong : MonoBehaviour
{

    [SerializeField] public SongObject song;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void loadSongInConductor()
    {
        if (ConductorScript.Instance)
        {
            ConductorScript.Instance.Song = song;
            ConductorScript.Instance.loadCurrentSong();
        }
    }
}
