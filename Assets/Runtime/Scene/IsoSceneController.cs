using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IsoSceneController : ISceneController
{
    private Dictionary<string, Scene> _loadedScenes;
    private string _currentSceneId;
    private string _previousSceneId;
    private Scene _currentScene;
    readonly UIController _uiController;
    readonly IDialogueController _dialogController;
    readonly LocaleManager _localeManager;
    readonly SaveStateController _saveGameManager;
    readonly CameraController _cameraController;
    readonly ClockController _clockController;
    readonly AddressablesAssetService _addressablesAssetService;

    public IsoSceneController(UIController uiController,
                              IDialogueController dialogController,
                              LocaleManager localeManager,
                              SaveStateController saveGameManager,
                              CameraController cameraController,
                              ClockController clockController,
                              AddressablesAssetService addressablesAssetService)
    {
        _uiController = uiController;
        _dialogController = dialogController;
        _localeManager = localeManager;
        _saveGameManager = saveGameManager;
        _cameraController = cameraController;
        _clockController = clockController;
        _addressablesAssetService = addressablesAssetService;

        Init();
    }

    private void Init()
    {

    }

    public async Task LoadScene(string sceneId, bool fadeInBlack = false)
    {
        if (fadeInBlack)
        {
            await _uiController.Fade(true);
        }

        // Load additive scene???

        await _uiController.Fade(false);
    }
}
