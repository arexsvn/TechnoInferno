using UnityEngine;
using System.Collections.Generic;

public class SceneController : ISceneController
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

    public SceneController(UIController uiController,
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

        init();
    }

    public async void LoadScene(string sceneId, bool fadeInBlack = false)
    {
        if (fadeInBlack)
        {
            await _uiController.fade(true);
        }

        if (_currentSceneId != null && _loadedScenes.ContainsKey(_currentSceneId))
        {
            GameObject.Destroy(_loadedScenes[_currentSceneId].gameObject);
            _loadedScenes.Remove(_currentSceneId);
        }
        _previousSceneId = _currentSceneId;
        _currentSceneId = sceneId;
        if (!_loadedScenes.ContainsKey(sceneId))
        {
            _loadedScenes[sceneId] = await _addressablesAssetService.InstantiateAsync<Scene>(null, sceneId);
            _loadedScenes[sceneId].view.canvas.worldCamera = _cameraController.camera;
            setupScene(_loadedScenes[sceneId]);
        }
        else
        {
            _loadedScenes[_currentSceneId].gameObject.SetActive(true);
            _currentScene = _loadedScenes[_currentSceneId];
        }

        refreshHotspots();

        await _uiController.fade(false);
    }

    private void StartText(string id)
    {
        _uiController.showClock(false);
        _uiController.showText(false);
        _uiController.fadeComplete.RemoveAll();

        _dialogController.Start(id);
    }

    private void setupScene(Scene scene)
    {
        _currentScene = scene;
        setupHotspots(scene);
    }

    private void init()
    {
        _loadedScenes = new Dictionary<string, Scene>();
        _dialogController.DialogueComplete += handleDialogueComplete;
    }

    private void handleDialogueComplete(string id)
    {
        refreshHotspots();

        if (_currentScene.customSceneController != null)
        {
            _currentScene.customSceneController.handleDialogueComplete("");
        }
    }

    private void setupHotspots(Scene scene)
    {
        if (scene.view.hotspotsContainer == null)
        {
            return;
        }
        foreach (Hotspot hotspot in scene.view.hotspotsContainer.GetComponentsInChildren<Hotspot>())
        {
            hotspot.view.click.Add(() => handleHotspotClick(hotspot));
            hotspot.view.over.Add(() => handleHotspotOver(hotspot));
            hotspot.view.off.Add(() => handleHotspotOff(hotspot));
        }
    }

    private void refreshHotspots()
    {
        Hotspot[] hotspots = _currentScene.view.hotspotsContainer.GetComponentsInChildren<Hotspot>(true);

        if (hotspots != null)
        {
            foreach(Hotspot hotspot in hotspots)
            {
                hotspot.gameObject.SetActive(!shouldHideForOutcomes(hotspot));
            }
        }
    }

    private bool shouldHideForOutcomes(Hotspot hotspot)
    {
        foreach (string outcome in hotspot.hideForOutcomes)
        {
            if (_saveGameManager.HasOutcome(outcome))
            {
                return true;
            }
        }

        bool doHide = false;

        if (hotspot.showForOutcomes.Count > 0)
        {
            doHide = true;

            foreach (string outcome in hotspot.showForOutcomes)
            {
                if (_saveGameManager.HasOutcome(outcome))
                {
                    return false;
                }
            }
        }

        return doHide;
    }

    private bool hotspotsDisabled()
    {
        return _dialogController.ShowingDialog;
    }

    private void handleHotspotClick(Hotspot hotspot)
    {
        if (hotspotsDisabled())
        {
            return;
        }
        
        if (_currentScene.customSceneController != null)
        {
            _currentScene.customSceneController.handleHotspotClick(hotspot);
        }

        switch (hotspot.type)
        {
            case Hotspot.Type.Move:
                _saveGameManager.SaveScene(hotspot.destination.id);
                LoadScene(hotspot.destination.id, true);
                break;
                /*
            case Hotspot.Type.Item:
                if (hotspot.GetTextId() != null)
                {
                    StartText(hotspot.GetTextId());
                }
                else
                {
                    _uiController.setText("You examine the " + hotspot.item.id + ".");
                }
                break;
                */
            case Hotspot.Type.Text:
                StartText(hotspot.GetTextId());
                break;
        }
    }

    private void handleHotspotOver(Hotspot hotspot)
    {
        if (hotspotsDisabled())
        {
            return;
        }

        if (_currentScene.customSceneController != null)
        {
            _currentScene.customSceneController.handleHotspotOver(hotspot);
        }

        string labelText = null;

        switch (hotspot.type)
        {
            case Hotspot.Type.Move:
                labelText = _localeManager.lookup(hotspot.destination.id + "_hotspot_label");

                if (labelText == null)
                {
                    labelText = _localeManager.lookup("hotspot_destination_label", new string[] { hotspot.destination.id });
                }
                break;
                /*
            case Hotspot.Type.Item:
                labelText = _localeManager.lookup("hotspot_item_label", new string[] { hotspot.item.id });
                break;
                */
            case Hotspot.Type.Text:
                labelText = _localeManager.lookup(hotspot.name.ToLower() + "_hotspot_label");
                break;
        }

        if (!string.IsNullOrEmpty(labelText))
        {
            _uiController.setText(labelText);
        }
    }

    private void handleHotspotOff(Hotspot hotspot)
    {
        if (hotspotsDisabled())
        {
            return;
        }

        if (_currentScene.customSceneController != null)
        {
            _currentScene.customSceneController.handleHotspotOff(hotspot);
        }

        _uiController.showText(false);
        _uiController.showClock(false);
    }
}
