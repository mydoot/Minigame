using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

[CreateAssetMenu(fileName = "SongObject", menuName = "Scriptable Objects/SongObject")]
public class SongObject : ScriptableObject
{
    [Header("Assets")]
    public AudioClip trackData;
    public TextAsset txtChartData;

    [Header("Metadata")]
    public string trackName;
    public float BPM;
    [Tooltip("Song offset. Adjust according to when the music actually begins playing in a file.")]
    public float songOffset = 0;

    [Header("Chart data as a List object")]
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

    //AI assistance used for the below
    void LoadChartFromText()
    {
        float oldChartCount = chart.Count;
        if (chart.Count >= 1)
        {
            chart.Clear();
        }

        string[] lines = txtChartData.text.Split('\n');

        foreach (string line in lines)
        {
            string cleanLine = line.Trim();

            if (string.IsNullOrWhiteSpace(cleanLine) || cleanLine.StartsWith("|")) continue;

            string[] data = cleanLine.Split(',');

            if (data.Length < 2) continue;

            string beatString = data[0].Trim();
            string typeString = data[1].Trim();

            // (InvariantCulture ensures decimals work correctly in all countries)
            float parsedBeat = float.Parse(beatString, NumberStyles.Float, CultureInfo.InvariantCulture);

            if (Enum.TryParse(typeString, out noteType parsedType))
            {
                chart.Add(new Chart { targetBeat = parsedBeat, type = parsedType });
            }
            else
            {
                Debug.LogWarning($"Invalid note type used in chart data '{typeString}'");
            }
        }

        Debug.Log($"Loaded chart data from {txtChartData.name}.txt, populating {chart.Count} elements from {oldChartCount} ({(oldChartCount < chart.Count ? "+" : "")}{chart.Count-oldChartCount} difference)");
    }

    void OnValidate()
    {
        if (txtChartData)
        {
            LoadChartFromText();
        }
    }
}

[Serializable]
public class Chart
{
    public float targetBeat;

    public noteType type;
}
