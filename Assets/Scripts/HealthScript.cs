using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed1 = 1.33f;
    [SerializeField] private float rotationSpeed2 = 1.33f;
    [SerializeField] private Transform healthBorder1;
    [SerializeField] private Transform healthBorder2;

    [SerializeField] private TextMeshProUGUI healthNum;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private int Health = 10;

    [SerializeField] private PlayerInput playerInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = Health;
        TrackHandler.onTakeDamage += takeDamage;
    }

    void OnDisable()
    {

        TrackHandler.onTakeDamage -= takeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        healthBorder1.Rotate(0, 0, 360 * rotationSpeed1 * Time.deltaTime);
        healthBorder2.Rotate(0, 0, -360 * rotationSpeed2 * Time.deltaTime);

    }

    void takeDamage(int n)
    {
        if (Health > 0)
        {
            Health = Health - n;
            healthSlider.value = Health;
            healthNum.text = Health.ToString();
        }
        
        if (Health <= 0)
        {
            TrackHandler.onSongEnd?.Invoke();
            playerInput.DeactivateInput();
        }
    }
}
