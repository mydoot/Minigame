using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboUI;
    [SerializeField] private TextMeshProUGUI judgementUI;

    public delegate void DiffDebug(float num);
    public static DiffDebug diffDebug;

    private float diff;
    public int combo;

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
        comboUI.text = $"Combo: {combo} | Note Diff (in ms): {diff}";
    }

    void increaseCombo()
    {
        combo++;
        judgementUI.text = "HIT!";
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
}
