using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TrackHandler trackHandler;

    [SerializeField] private TextMeshProUGUI hitsText;
    [SerializeField] private TextMeshProUGUI missesText;
    [SerializeField] private TextMeshProUGUI gradeText;

    private void Start()
    {
        resultsPanel.SetActive(false);

        TrackHandler.onSongEnd += ShowResults;
    }

    private void OnDisable()
    {
        TrackHandler.onSongEnd -= ShowResults;
    }

    public void ShowResults()
    {
        int hits = trackHandler.hitCount;
        int misses = trackHandler.missedCount;

        resultsPanel.SetActive(true);

        hitsText.text = "Hits: " + hits;
        missesText.text = "Misses: " + misses;

        int total = hits + misses;
        float accuracy = total > 0 ? (float)hits / total : 0f;

        if (accuracy >= 0.95f)
            gradeText.text = "Grade: S";
        else if (accuracy >= 0.90f)
            gradeText.text = "Grade: A";
        else if (accuracy >= 0.80f)
            gradeText.text = "Grade: B";
        else if (accuracy >= 0.70f)
            gradeText.text = "Grade: C";
        else
            gradeText.text = "Grade: F";
    }
}