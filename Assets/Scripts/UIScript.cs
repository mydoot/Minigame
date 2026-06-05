using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboUI;
    [SerializeField] private judgementScript judgementUI;
    [SerializeField] private TextMeshProUGUI realComboUI;
    [SerializeField] private TextMeshProUGUI debuggingUI;
    [SerializeField] private Transform judgementSpawnPos;


    public delegate void DiffDebug(float num);
    public static DiffDebug diffDebug;

    private float diff;
    public int combo;

    private bool debugMode = true;

    private Tween punchTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combo = 0;

        diffDebug += handleDiff;

        TrackHandler.onNoteHit += increaseCombo;
        TrackHandler.onNoteMissed += decreaseCombo;

       
    }

    // Update is called once per frame
    void Update()
    {
        comboUI.text = $"Combo: {combo} | Note Diff (in ms): {diff:F2}";
        realComboUI.text = $"x{combo}";

        if (UnityEngine.InputSystem.Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!debugMode)
            {
                showDebuggers();
            }
            else
            {
                hideDebuggers();
            }
        }
    }

    void increaseCombo()
    {
        combo++;
        popUpJudgement("HIT!");
        if (punchTween != null && punchTween.IsActive())
        {
            punchTween.Kill();
            realComboUI.transform.localScale = Vector3.one;
        }

        punchTween = realComboUI.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0), 0.3f,0,0.5f);
    }

    void decreaseCombo()
    {
        if (combo != 0)
        {
            combo = 0;
        }
        popUpJudgement("MISS!");
    }

    void handleDiff(float num)
    {
        diff = num;
    }

    void hideDebuggers()
    {
        debugMode = false;
        comboUI.gameObject.SetActive(false);
        debuggingUI.gameObject.SetActive(false);
    }
    void showDebuggers()
    {
        debugMode = true;
        comboUI.gameObject.SetActive(true);
        debuggingUI.gameObject.SetActive(true);
    }


    void popUpJudgement(string text)
    {
        judgementScript judge = Instantiate(judgementUI, judgementSpawnPos, false);
        judge.changeText(text);
    }
}
