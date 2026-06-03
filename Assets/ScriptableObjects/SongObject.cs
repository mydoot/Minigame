using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    
    /* [ContextMenu("Export Chart to TXT")]
    public void ExportExistingChart()
    {
        // 1. Define exactly where the file will be saved (Your main Assets folder)
        string filePath = Application.dataPath + "/MyExportedChart.txt";

        // 2. Create an empty string to hold all our text
        string textData = "";

        // 3. Loop through your existing manual list
        foreach (Chart note in chart)
        {
            // Format it exactly like our parser expects: "1.5, Tap"
            // \n tells the text file to drop down to the next line
            textData += $"{note.targetBeat}, {note.type}\n"; 
        }

        // 4. Create the physical file on your hard drive!
        File.WriteAllText(filePath, textData);

        Debug.Log($"<color=green>SUCCESS:</color> Chart exported to {filePath}");
    } */
}

[Serializable]
public class Chart
{
    public float targetBeat;

    public noteType type;
}
