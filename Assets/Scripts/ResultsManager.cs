using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;



public class ResultsManager : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private RectTransform resultsbottomPanel;
    [SerializeField] private TrackHandler trackHandler;

    [SerializeField] private TextMeshProUGUI hitsText;
    [SerializeField] private TextMeshProUGUI missesText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI accText;
    [SerializeField] private TextMeshProUGUI resultsText;
    [SerializeField] private TextMeshProUGUI remainingHealth;
    [SerializeField] private TextMeshProUGUI songText;
    [SerializeField] private GameObject particleEmitter;
    private Sequence resultsSequence;

    private void Start()
    {
        resultsPanel.SetActive(false);
        gradeText.gameObject.SetActive(false);
        particleEmitter.SetActive(false);
        songText.text = ConductorScript.Instance.Song.trackName;

        TrackHandler.onSongEnd += ShowResults;
    }

    private void OnDisable()
    {
        TrackHandler.onSongEnd -= ShowResults;
        resultsSequence.Kill();
    }

    public void ShowResults()
    {
        if (!resultsPanel.activeSelf)
        {

            resultsSequence = DOTween.Sequence();
            int hits = trackHandler.hitCount;
            int misses = trackHandler.missedCount;

            resultsPanel.SetActive(true);

            resultsSequence.Append(resultsPanel.GetComponent<Image>().DOFade(1f, 2f))
                .Append(resultsbottomPanel.DOAnchorPosX(-7000f, 1.25f).From(true).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    gradeText.gameObject.SetActive(true);
                    particleEmitter.SetActive(true);
                }))
                .Append(gradeText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 0.5f, 0, 1f))
                .SetAutoKill(false);

            int total = calculateTotalHits();
            float accuracy = total > 0 ? (float)hits / total : 0f;

            float shownAcc = accuracy * 100;

            hitsText.text = "HITS: " + hits;
            missesText.text = "MISSES: " + misses;
            accText.text = $"Hit rate: {shownAcc:F2}%";
            remainingHealth.text = $"Remaining Health: {HealthScript.Health}";

            if (HealthScript.Health == 0)
            {
                gradeText.text = "F";
                resultsText.text = "FAILURE";
            } 
            else if (HealthScript.Health == 10 && misses == 0)
                gradeText.text = "FC";
            else if (HealthScript.Health >= 9)
                gradeText.text = "S";
            else if (HealthScript.Health >= 8)
                gradeText.text = "A";
            else if (HealthScript.Health >= 6)
                gradeText.text = "B";
            else if (HealthScript.Health >= 2)
                gradeText.text = "C";
            else
                gradeText.text = "D";
        }
    }

    public void onContinue(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (resultsPanel.activeSelf && resultsSequence.IsComplete())
                SceneManagerMini.Instance.LoadMainMenu();
        }
    }

    private int calculateTotalHits()
    {
        int totalHits = 0;
        Queue<Chart> calc = new Queue<Chart>(ConductorScript.Instance.Song.chart);

        foreach (Chart chartNote in calc)
        {
            noteType note = chartNote.type;

            switch (note)
            {
                case noteType.Note:
                    totalHits += 1;
                    break;

                case noteType.GhostNote:
                    totalHits += 1;
                    break;
                
                case noteType.StealthNote:
                    totalHits += 1;
                    break;

                case noteType.CapsuleNote:
                    totalHits += 2;
                    break;
                case noteType.TriangleNote:
                    totalHits += 3;
                    break;

                default:
                    break;
            }
        }
        Debug.Log($"total hits: {totalHits}");
        return totalHits;
    }
}