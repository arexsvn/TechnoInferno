using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HudView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject buttonContainer;
    public CanvasGroup canvasGroup;
    public Action Over;
    public Action Off;
    public ButtonView hudElement;
    private bool _showing = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Over?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Off?.Invoke();
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
