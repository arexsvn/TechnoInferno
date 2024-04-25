using UnityEngine;
using VContainer.Unity;

public class GameController : ITickable, ILateTickable, IFixedTickable, IInitializable
{
    private bool _gamePaused = true;
    readonly UIController _uiController;
    readonly ISceneController _sceneController;
    readonly SaveStateController _saveStateController;
    readonly IDialogueController _dialogueController;
    readonly LocaleManager _localeManager;
    readonly ISubscriber<ApplicationMessage> _applicationMessageSubscriber;
    readonly MainMenuController _mainMenuController;
    readonly BasicDialogController _basicDialogController;
    private const string START_SCENE = "office";
    public const string STRINGS_PATH = "data/strings";

    public GameController(UIController uiController, 
                          ISceneController sceneController, 
                          SaveStateController saveStateController,
                          IDialogueController dialogueController, 
                          LocaleManager localeManager,
                          MainMenuController mainMenuController,
                          ISubscriber<ApplicationMessage> applicationMessageSubscriber,
                          BasicDialogController basicDialogController) 
    {
        _uiController = uiController;
        _sceneController = sceneController;
        _saveStateController = saveStateController;
        _dialogueController = dialogueController;
        _localeManager = localeManager;
        _mainMenuController = mainMenuController;
        _applicationMessageSubscriber = applicationMessageSubscriber;
        _basicDialogController = basicDialogController;
    }

    public async void Initialize()
    {
        _uiController.showBlackScreen();

        _applicationMessageSubscriber.Subscribe(OnApplicationMessage);

        loadLocale();
        await _basicDialogController.Init();
        initMainMenu();
    }

    private void loadLocale()
    {
        TextAsset strings = (TextAsset)Resources.Load(STRINGS_PATH);
        _localeManager.addBundle(strings.text);
    }

    private async void initMainMenu()
    {
        bool newGame = _saveStateController.LoadCurrentSave();
        // load latest scene -or- starting scene
        if (_saveStateController.CurrentSave.SceneId == null)
        {
            _saveStateController.CurrentSave.SceneId = START_SCENE;
        }
        _sceneController.LoadScene(_saveStateController.CurrentSave.SceneId);

        await _mainMenuController.init();
        _mainMenuController.newGame.Add(handleNewGame);
        _mainMenuController.resumeGame.Add(handleResumeGame);
        _mainMenuController.loadGame.Add(handleLoadGame);
        _mainMenuController.quitGame.Add(handleQuit);

        _mainMenuController.allowResume(!newGame);
        _mainMenuController.allowLoad(!newGame && _saveStateController.GetTotalSaves() > 0);
        showMainMenu();
    }

    public async void showMainMenu(bool show = true, bool fadeFromBlack = true, float fadeTime = -1)
    {
        if (show && fadeFromBlack)
        {
            fadeTime = 0;
        }
        
        _mainMenuController.show(show, fadeTime);

        if (show)
        {
            handlePause();

            if (fadeFromBlack)
            {
                await _uiController.fade(false, default(Color), -1, 1f);
            }
        }
    }

    private void OnApplicationMessage(ApplicationMessage applicationMessage)
    {
        switch (applicationMessage.ApplicationAction)
        {
            case ApplicationAction.Pause:
                handlePause();
                break;
            case ApplicationAction.UnPause:
                handleUnPause();
                break;
            case ApplicationAction.Quit:
                handleQuit();
                break;
            case ApplicationAction.ShowMainMenu:
                showMainMenu(true, false);
                break;
        }
    }

    private void handlePause()
    {
        _gamePaused = true;
        _dialogueController.Pause();
    }

    private void handleUnPause()
    {
        _gamePaused = false;
        _dialogueController.Resume();
    }

    private void handleQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL
        Application.Quit();
#endif
    }

    private async void handleNewGame()
    {
        _dialogueController.Stop();
        await _uiController.fade(true);
        newGame();
    }

    private void handleResumeGame()
    {
        // Latest saved scene is already loaded on startup so simply hide the main menu.
        showMainMenu(false, false);
    }

    private void handleLoadGame()
    {
        _saveStateController.saveGameSelected.AddOnce(handleSaveGameSelected);
        _saveStateController.closeButtonClicked.AddOnce(handleSaveGameSelectionClosed);
        _saveStateController.Show();
    }

    private void handleSaveGameSelectionClosed()
    {
        _saveStateController.saveGameSelected.RemoveAll();
        _saveStateController.closeButtonClicked.RemoveAll();
        _saveStateController.Show(false);
    }

    private async void handleSaveGameSelected(SaveState saveGame)
    {
        _dialogueController.Stop();
        _uiController.fadeComplete.AddOnce(() => loadGame(saveGame));
        await _uiController.fade(true);
        handleSaveGameSelectionClosed();
    }

    private void loadGame(SaveState saveGame)
    {
        showMainMenu(false, false, 0);
        // load latest scene -or- starting scene
        _sceneController.LoadScene(saveGame.SceneId);
        handleUnPause();
    }

    private void newGame()
    {
        showMainMenu(false, true, 0);
        _mainMenuController.allowResume(true);
        _mainMenuController.allowLoad(true);

        _saveStateController.CreateNewSave();
        _sceneController.LoadScene(START_SCENE);
        _saveStateController.SaveScene(START_SCENE);
        handleUnPause();
    }

    public void Tick()
    {
        if (_gamePaused)
        {
            return;
        }

    }

    public void FixedTick()
    {
        if (_gamePaused)
        {
            return;
        }
    }

    public void LateTick()
    {
        if (_gamePaused)
        {
            return;
        }
    }
}
