using System;
using UnityEngine;

public class HudController
{
    public Action<Screens.Name> ShowScreen;
    private HudView _view;
    private readonly ClockController _clockController;
    private readonly AddressablesAssetService _assetService;

    public HudController(ClockController clockController, AddressablesAssetService assetService)
    {
        _clockController = clockController;
        _assetService = assetService;
        init();
    }

    private async void init()
    {
        _clockController.init();

        _view = await _assetService.InstantiateAsync<HudView>();
        _view.Over += () => show(true);
        _view.Off += () => show(false);

        _view.canvasGroup.alpha = 0f;

        addElement("Journal", Screens.Name.Journal);
        addElement("Main Menu", Screens.Name.MainMenu);
    }

    private void addElement(string label, Screens.Name screenName)
    {
        ButtonView hudElement = GameObject.Instantiate(_view.hudElement, _view.buttonContainer.transform);
        hudElement.labelText.text = label;
        hudElement.button.onClick.AddListener(()=>hudElementClicked(screenName));
    }

    public void show(bool show = true)
    {
        _view.show(show);
    }

    public void showClock(bool show, double costInMinutes)
    {
        _clockController.showTimeCost(show, costInMinutes);
    }

    private void hudElementClicked(Screens.Name screenName)
    {
        ShowScreen?.Invoke(screenName);
    }
}
