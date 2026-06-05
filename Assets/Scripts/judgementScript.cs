using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class judgementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Image thisImage;

    private TextMeshProUGUI thisText;

    void Awake()
    {
        thisImage = GetComponent<Image>();
        thisText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        this.transform.DOMoveY(-10, 0.15f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            thisImage.DOFade(0f, 0.5f).OnComplete(() =>
            {
            });
                Destroy(gameObject);
        });
    }
    
    public void changeText(string text)
    {
        thisText.text = text;
    }


}
