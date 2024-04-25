using UnityEngine;
using UnityEngine.EventSystems;
using signals;

public class HudView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject buttonContainer;
    public CanvasGroup canvasGroup;
    public Signal over = new Signal();
    public Signal off = new Signal();
    public ButtonView hudElement;
    private bool _showing = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        over.Dispatch();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        off.Dispatch();
    }

    public async void show(bool show = true)
    {
        _showing = show;
        if (show)
        {
            await DOTweenUITransitions.fadeIn(canvasGroup, gameObject);
        }
        else
        {
            await DOTweenUITransitions.fadeOut(canvasGroup, gameObject, false);
        }
    }

    public bool showing
    {
        get
        {
            return _showing;
        }
    }
}
