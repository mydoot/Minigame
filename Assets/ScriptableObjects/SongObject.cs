using System;
using System.Collections.Generic;
using UnityEngine;

public enum noteType
{
    Note,
    GhostNote
}

[CreateAssetMenu(fileName = "SongObject", menuName = "Scriptable Objects/SongObject")]
public class SongObject : ScriptableObject
{
    [Header("Assets")]
    public AudioClip trackData;

    [Header("Metadata")]
    public string trackName;
    public float BPM;
    public List<Chart> chart = new List<Chart>();
}

[Serializable]
public class Chart
{
    public float targetBeat;

    public noteType type;
}
