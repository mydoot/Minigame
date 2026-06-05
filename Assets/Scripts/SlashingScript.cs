using UnityEngine;
using UnityEngine.TextCore.Text;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using Unity.UI;

public class SlashCombo : MonoBehaviour
{
    public Animator animator;

    private int clickCount = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 0.5f;

    [SerializeField] private CharacterSounds charSounds;

    [SerializeField] private RectTransform characterTransform;

    private Tween bounceTween;


    void Start()
    {
        ConductorScript.onTheBeat += bounceCharacter;
    }

    void OnDisable()
    {
        ConductorScript.onTheBeat -= bounceCharacter;
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            clickCount++;
            comboTimer = comboResetTime;
            charSounds.playWhiffSound();

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

    void bounceCharacter()
    {
        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            {
                if (bounceTween != null && bounceTween.IsActive())
                {
                    bounceTween.Kill();
                    characterTransform.localScale = Vector2.one;
                }

                characterTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.1f, 0, 0.15f);
            }
        }
    }
}