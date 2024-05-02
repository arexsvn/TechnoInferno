using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotspotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action Over;
    public Action Off;
    public Action Click;

    public void Start()
    {
        // wire click propogation
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick);
        // make hotspot invisible
        Image image = button.gameObject.GetComponentInChildren<Image>();
        image.color = new Color(0, 1, 0, 0);
    }

    public void OnDestroy()
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.RemoveListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Over?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Off?.Invoke();
    }

    private void OnClick()
    {
        Click?.Invoke();
    }
}
