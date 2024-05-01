using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Globalization;

public class ClockView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI _timeDisplayText;
    public CanvasGroup canvasGroup;
    public Action over;
    public Action off;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        off.Invoke();
    }

    public void SetTime(DateTime time)
    {
        string timeString = time.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        _timeDisplayText.text = timeString;
    }
}