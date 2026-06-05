using UnityEngine;

// Notice there is no ": MonoBehaviour" here! 
// This script doesn't even need to be attached to a GameObject in Unity.
public static class SessionData
{
    // The 'static' keyword makes this variable immortal. 
    // It will survive all scene changes.
    public static SongObject nextSongToLoad;
    public static int globalOffsetInMS;
}