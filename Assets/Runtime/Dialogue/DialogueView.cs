using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using signals;

public class DialogueView : MonoBehaviour
{
    public Signal backgroundClick = new Signal();
    [SerializeField] public GameObject ChoiceContainer;

    [SerializeField] private PortraitView _portraitView;
    [SerializeField] private TextOverlayView _npcTextDisplay;
    [SerializeField] private TextOverlayView _playerTextDisplay;
    [SerializeField] private TextOverlayView _descriptiveTextDisplay;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _choiceCanvasGroup;
    [SerializeField] private CanvasGroup _backgroundCanvasGroup;
    [SerializeField] private Button _backgroundButton;

    private bool _showingBackground = true;
    private List<TextOverlayView> _textOverlays;
    private Vector3 _playerTextPositionStandard;
    private Vector3 _playerTextPositionQuestion;

    void Awake()
    {
        _backgroundButton.onClick.AddListener(OnBackgroundClick);
        _textOverlays = new List<TextOverlayView> { _npcTextDisplay, _playerTextDisplay, _descriptiveTextDisplay };
        _playerTextPositionStandard = _playerTextDisplay.transform.position;
        _playerTextPositionQuestion = _descriptiveTextDisplay.transform.position;

        HideAll();
    }

    public void SetDescriptiveText(string text)
    {
        _descriptiveTextDisplay.textField.text = text;
        ShowOnlyTextOverlay(_descriptiveTextDisplay);
    }

    public void SetNpcText(string text, string color = null)
    {
        Color characterTextColor;
        if (color != null && ColorUtility.TryParseHtmlString(color, out characterTextColor))
        {
            _npcTextDisplay.textField.color = characterTextColor;
        }
        else
        {
            _npcTextDisplay.restoreDefaultTextColor();
        }

        _npcTextDisplay.textField.text = text;
        ShowOnlyTextOverlay(_npcTextDisplay);
    }

    public void SetPlayerText(string text, bool question = false)
    {
        if (question)
        {
            _playerTextDisplay.transform.position = _playerTextPositionQuestion;
        }
        else
        {
            _playerTextDisplay.transform.position = _playerTextPositionStandard;
        }
        _playerTextDisplay.textField.text = text;
        ShowOnlyTextOverlay(_playerTextDisplay);
    }

    public void SetPortrait(string characterId, string emotion = null)
    {
        _portraitView.display(characterId, emotion);

        if (!_portraitView.imageContainer.showing)
        {
            _portraitView.imageContainer.show(true);
        }
    }

    public void HideAll()
    {
        _portraitView.imageContainer.show(false, 0);
        ShowNpcText(false, 0);
        ShowDescriptiveText(false, 0);

        _choiceCanvasGroup.alpha = 0f;
        ChoiceContainer.SetActive(false);

        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        _backgroundButton.onClick.RemoveListener(OnBackgroundClick);
        backgroundClick.RemoveAll();
    }

    public void ClearChoices()
    {
        foreach (Transform child in ChoiceContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void HidePortrait()
    {
        _portraitView.imageContainer.show(false);
    }

    public async void ShowChoices(bool show = true)
    {
        if (show)
        {
            await DOTweenUITransitions.fadeIn(_choiceCanvasGroup, ChoiceContainer);
        }
        else
        {
            await DOTweenUITransitions.fadeOut(_choiceCanvasGroup, ChoiceContainer, true);
        }
    }

    public async void Show(bool show = true)
    {
        if (show)
        {
            await DOTweenUITransitions.fadeIn(_canvasGroup, gameObject);
        }
        else
        {
            await DOTweenUITransitions.fadeOut(_canvasGroup, gameObject, true);
        }
    }

    public async void ShowBackground(bool show = true)
    {
        if (_showingBackground == show)
        {
            return;
        }

        _showingBackground = show;

        if (show)
        {
            await DOTweenUITransitions.fadeIn(_backgroundCanvasGroup);
        }
        else
        {
            await DOTweenUITransitions.fadeOut(_backgroundCanvasGroup, _backgroundCanvasGroup.gameObject, false);
        }
    }

    private void ShowNpcText(bool show = true, float fadeTime = -1)
    {
        _npcTextDisplay.show(show, fadeTime);
    }

    private void ShowPlayerText(bool show = true, float fadeTime = -1)
    {
        _playerTextDisplay.show(show, fadeTime);
    }

    private void ShowDescriptiveText(bool show = true, float fadeTime = -1)
    {
        _descriptiveTextDisplay.show(show, fadeTime);
    }

    private void ShowOnlyTextOverlay(TextOverlayView textOverlay)
    {
        foreach (TextOverlayView nextTextOverlay in _textOverlays)
        {
            if (textOverlay == nextTextOverlay)
            {
                if (!nextTextOverlay.showing)
                {
                    nextTextOverlay.show(true);
                }
            }
            else
            {
                nextTextOverlay.show(false);
            }
        }
    }

    private void OnBackgroundClick()
    {
        backgroundClick.Dispatch();
    }
}
