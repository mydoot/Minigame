using UnityEngine;

public class SlashCombo : MonoBehaviour
{
    public Animator animator;

    private int clickCount = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 0.5f;

    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            clickCount++;
            comboTimer = comboResetTime;

            if (clickCount == 1)
            {
                animator.Play("Slash1", 0, 0f);
            }
            else if (clickCount == 2)
            {
                animator.Play("Slash2", 0, 0f);
            }
            else if (clickCount >= 3)
            {
                animator.Play("Slash3", 0, 0f);
                clickCount = 0;
            }
        }

        if (clickCount > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                clickCount = 0;
            }
        }
    }
}