using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    public TextMeshProUGUI title;
    public GameObject buttonContainer;
    public Image iconImage;
    public CanvasGroup canvasGroup;

    public async void show(bool show = true, float fadeTime = -1)
    {
        float alpha = 1f;
        
        if (!show)
        {
            alpha = 0f;
        }
        
        await DOTweenUITransitions.fade(canvasGroup, alpha, fadeTime, gameObject, !show);
    }
}
