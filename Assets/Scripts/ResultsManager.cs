using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;



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
    [SerializeField] private GameObject particleEmitter;
    private Sequence resultsSequence;

    private void Start()
    {
        resultsPanel.SetActive(false);
        gradeText.gameObject.SetActive(false);
        particleEmitter.SetActive(false);

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

            int total = hits + misses;
            float accuracy = total > 0 ? (float)hits / total : 0f;

            float shownAcc = accuracy * 100;

            hitsText.text = "HITS: " + hits;
            missesText.text = "MISSES: " + misses;
            accText.text = $"ACCURACY: {shownAcc:F0}%";

            if (accuracy == 100f && misses == 0)
                gradeText.text = "FC";
            else if (accuracy >= 0.95f)
                gradeText.text = "S";
            else if (accuracy >= 0.90f)
                gradeText.text = "A";
            else if (accuracy >= 0.80f)
                gradeText.text = "B";
            else if (accuracy >= 0.70f)
                gradeText.text = "C";
            else
            {
                gradeText.text = "F";
                resultsText.text = "FAILURE";
            }
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
}