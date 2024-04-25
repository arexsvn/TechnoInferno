using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOverlayView : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Image background;
    public Button button;
    public CanvasGroup canvasGroup;
    private Color _defaultTextColor = Color.white;
    private bool _showing = false;

    public void Start()
    {
        //_defaultTextColor = textField.color;
    }

    public async void show(bool show = true, float fadeTime = -1)
    {
        _showing = show;

        float alpha = 1f;
        if (!show)
        {
            alpha = 0f;
        }

        await DOTweenUITransitions.fade(canvasGroup, alpha, fadeTime, gameObject, !show);
    }

    public void restoreDefaultTextColor()
    {
        textField.color = _defaultTextColor;
    }

    public bool showing
    {
        get
        {
            return _showing;
        }
    }
}
