using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChoiceView : MonoBehaviour
{
    public TextMeshProUGUI textArea;
    public Action Clicked;

    public void Start()
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnDestroy()
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        Clicked?.Invoke();
    }
}
