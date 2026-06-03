using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboUI;

    public int combo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combo = 0;

        TrackHandler.onNoteHit += increaseCombo;
        TrackHandler.onNoteMissed += decreaseCombo;
    }

    // Update is called once per frame
    void Update()
    {
        comboUI.text = $"Combo: {combo}";    
    }

    void increaseCombo()
    {
        combo++;

    }

    void decreaseCombo()
    {
        if (combo != 0)
        {
        combo = 0;
        }
    }
}
