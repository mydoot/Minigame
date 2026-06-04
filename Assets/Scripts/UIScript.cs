using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboUI;
    [SerializeField] private TextMeshProUGUI judgementUI;
    [SerializeField] private TextMeshProUGUI realComboUI;
    [SerializeField] private TextMeshProUGUI debuggingUI;


    public delegate void DiffDebug(float num);
    public static DiffDebug diffDebug;

    private float diff;
    public int combo;

    private bool debugMode = false;

    private Tween punchTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combo = 0;

        diffDebug += handleDiff;

        TrackHandler.onNoteHit += increaseCombo;
        TrackHandler.onNoteMissed += decreaseCombo;

        judgementUI.text = "nothing so far";
    }

    // Update is called once per frame
    void Update()
    {
        comboUI.text = $"Combo: {combo} | Note Diff (in ms): {diff:F2}";
        realComboUI.text = $"x{combo}";

        if (UnityEngine.InputSystem.Keyboard.current.lKey.wasPressedThisFrame)
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
        judgementUI.text = "HIT!";
        if (punchTween != null && punchTween.IsActive())
        {
            punchTween.Kill();
            realComboUI.transform.localScale = Vector3.one;
        }

        punchTween = realComboUI.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.25f,0,0.1f);
    }

    void decreaseCombo()
    {
        if (combo != 0)
        {
            combo = 0;
        }
        judgementUI.text = "MISS!";
    }

    void handleDiff(float num)
    {
        diff = num;
    }

    void hideDebuggers()
    {
        comboUI.gameObject.SetActive(false);
        debuggingUI.gameObject.SetActive(false);
    }
    void showDebuggers()
    {
        comboUI.gameObject.SetActive(true);
        debuggingUI.gameObject.SetActive(true);
    }

}
