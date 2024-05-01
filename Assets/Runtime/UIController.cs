using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class UIController
{
    private GameObject _fadeScreen;
    private const float FADE_TIME = 0.5f;
    readonly UICreator _uiCreator;
    readonly HudController _hudController;
    readonly TextOverlayController _textOverlayController;
    readonly JournalController _journalController;
    readonly IPublisher<ApplicationMessage> _applicationMessagePublisher;

    public UIController(UICreator uiCreator, HudController hudController, TextOverlayController textOverlayController, JournalController journalController, IPublisher<ApplicationMessage> applicationMessagePublisher)
    {
        _uiCreator = uiCreator;
        _hudController = hudController;
        _textOverlayController = textOverlayController;
        _journalController = journalController;
        _applicationMessagePublisher = applicationMessagePublisher;

        init();
    }

    public void ShowClock(bool show, double costInMinutes = 0)
    {
        _hudController.showClock(show, costInMinutes);
    }

    public void ShowHud(bool show = true)
    {
        _hudController.show(show);
        _textOverlayController.show(show);
    }

    public void ShowJournal(bool show = true)
    {
        if (show)
        {
            _journalController.CloseButtonClicked += handleJournalClose;
            pauseGame();
        }
        else
        {
            unPauseGame();
        }
        
        _journalController.show(show);
    }

    public void SetText(string text)
    {
        _textOverlayController.show();
        _textOverlayController.setText(text);
    }

    public void ShowText(bool show = true)
    {
        _textOverlayController.show(show);
    }

    public void ShowBlackScreen(bool show = true)
    {
        if (_fadeScreen == null)
        {
            _fadeScreen = _uiCreator.createFadeScreen();
        }

        _fadeScreen.SetActive(show);
    }

    public async Task Fade(bool fadeIn, Color color = default(Color), float fadeTime = -1, float delay = 0f)
    {
        if (fadeTime == -1)
        {
            fadeTime = FADE_TIME;
        }

        if (_fadeScreen == null)
        {
            _fadeScreen = _uiCreator.createFadeScreen();
        }
        else
        {
            _fadeScreen.SetActive(true);
        }

        int initAlpha = 0;
        int finalAlpha = 1;

        if (!fadeIn)
        {
            initAlpha = 1;
            finalAlpha = 0;
        }

        color.a = initAlpha;

        Image image =_fadeScreen.GetComponentInChildren<Image>();
        image.color = color;
		await image.DOFade(finalAlpha, fadeTime).SetDelay(delay).AsyncWaitForCompletion();
        if (finalAlpha == 0) { _fadeScreen.SetActive(false); }
    }

    private async void init()
    {
        _hudController.ShowScreen += handleShowScreen;
        await _textOverlayController.Init();
    }

    private void pauseGame()
    {
        _applicationMessagePublisher.Publish(new ApplicationMessage(ApplicationAction.Pause));
    }

    private void unPauseGame()
    {
        _applicationMessagePublisher.Publish(new ApplicationMessage(ApplicationAction.UnPause));
    }

    private void handleShowScreen(Screens.Name screenName)
    {
        switch(screenName)
        {
            case Screens.Name.MainMenu :
                _applicationMessagePublisher.Publish(new ApplicationMessage(ApplicationAction.ShowMainMenu));
                ShowHud(false);
                break;

            case Screens.Name.Journal:
                ShowJournal(true);
                ShowHud(false);
                break;
        }
    }

    private void handleJournalClose()
    {
        if (_journalController.showing)
        {
            _journalController.show(false);
            unPauseGame();
        }
    }

    private void fitToScreen(GameObject container)
    {
        SpriteRenderer spriteRenderer = container.GetComponent<SpriteRenderer>();

        container.transform.localScale = new Vector3(1, 1, 1);

        float width = spriteRenderer.bounds.size.x;
        float height = spriteRenderer.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        container.transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
    }
}

public class Screens
{
    public enum Name { MainMenu, Journal };
}
 
