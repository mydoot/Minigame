using UnityEngine;

[CreateAssetMenu(fileName = "SongObject", menuName = "Scriptable Objects/SongObject")]
public class SongObject : ScriptableObject
{
    [Header("Assets")]
    public AudioClip trackData;

    [Header("Metadata")]
    public float BPM;
}
