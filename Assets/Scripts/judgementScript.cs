using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class judgementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private TextMeshProUGUI thisText;

    void Awake()
    {
        thisText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        this.GetComponent<RectTransform>().DOAnchorPosY(35f, 0.3f).SetRelative().SetEase(Ease.OutQuad).OnComplete(() =>
        {
            thisText.DOFade(0f, 0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
    
    public void changeText(string text)
    {
        thisText.text = text;
    }


}
