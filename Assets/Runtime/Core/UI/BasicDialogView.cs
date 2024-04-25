using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicDialogView : MonoBehaviour, IUIView
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button closeButton;
    public Button background;
    public RectTransform buttonContainer;
    public BasicButtonView buttonPrefab;
    public CanvasGroup canvasGroup;
    private IUITransitions _uiTransitions;

    public void Set(string title, string message = null, List<DialogButtonData> buttons = null, bool allowClose = true, IUITransitions uiTransitions = null)
    {
        titleText.text = title;

        if (!string.IsNullOrEmpty(message))
        {
            messageText.text = message;
        }

        if (buttons != null)
        {
            UIUtils.RecycleOrCreateItems(buttonContainer, buttonPrefab, buttons, setupButton);
        }
        else
        {
            ClearButtons();
        }

        closeButton.gameObject.SetActive(allowClose);

        if (allowClose)
        {
            closeButton.onClick.AddListener(() => Hide());
            background.onClick.AddListener(() => Hide());
        }

        _uiTransitions = uiTransitions;

        Show();
    }

    public async void Show(bool animate = true)
    {
        gameObject.SetActive(true);
        if (animate && _uiTransitions != null)
        {
            await _uiTransitions.FadeIn(canvasGroup, gameObject);
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }

    public async void Hide(bool animate = true)
    {
        if (animate && _uiTransitions != null)
        {
            await _uiTransitions.FadeOut(canvasGroup, gameObject);
        }
        else
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }

    public void ClearButtons()
    {
        foreach (Transform child in buttonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    protected void setupButton(BasicButtonView view, DialogButtonData data)
    {
        view.button.onClick.RemoveAllListeners();
        view.labelText.text = data.label;

        if (data.action != null)
        {
            view.button.onClick.AddListener(data.action);
        }

        if (data.closeOnAction)
        {
            view.button.onClick.AddListener(() => Hide());
        }
    }

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public bool IsFullScreen { get => false; set { } }
}