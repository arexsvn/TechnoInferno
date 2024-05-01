using System;
using System.Threading.Tasks;

public class MainMenuController
{
    public Action NewGame;
    public Action ResumeGame;
    public Action LoadGame;
    public Action QuitGame;
    private MainMenuView _view;
    private static string BUTTON_PREFAB = "UI/MainMenuButton";
    private ButtonView _resumeButton;
    private ButtonView _loadButton;
    readonly UICreator _uiCreator;
    readonly LocaleManager _localeManager;
    readonly IAssetService _assetService;
    private bool _inited;

    public MainMenuController(UICreator uiCreator, LocaleManager localeManager, IAssetService assetService)
    {
        _uiCreator = uiCreator;
        _localeManager = localeManager;
        _assetService = assetService;
    }

    public async Task Init()
    {
        if (_inited)
        {
            await Task.CompletedTask;
        }


        _view = await _assetService.InstantiateAsync<MainMenuView>();
        _view.title.text = _localeManager.lookup("application_title");
        _resumeButton = _uiCreator.addButton(_view.buttonContainer, new ButtonData(() => ResumeGame?.Invoke(), _localeManager.lookup("application_resume"), null, BUTTON_PREFAB));
        _loadButton = _uiCreator.addButton(_view.buttonContainer, new ButtonData(() => LoadGame?.Invoke(), _localeManager.lookup("application_load"), null, BUTTON_PREFAB));
        _uiCreator.addButton(_view.buttonContainer, new ButtonData(() => NewGame?.Invoke(), _localeManager.lookup("application_new"), null, BUTTON_PREFAB));
        _uiCreator.addButton(_view.buttonContainer, new ButtonData(() => QuitGame?.Invoke(), _localeManager.lookup("application_quit"), null, BUTTON_PREFAB));

        _inited = true;
    }

    public void Show(bool show = true, float fadeTime = -1)
    {
        _view.show(show, fadeTime);
    }

    public void AllowResume(bool allow)
    {
        _resumeButton.gameObject.SetActive(allow);
    }

    public void AllowLoad(bool allow)
    {
        _loadButton.gameObject.SetActive(allow);
    }
}
