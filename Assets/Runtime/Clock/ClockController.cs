using System;

public class ClockController
{
    private ClockView _view;
    private DateTime _gameTime;
    private int _currentDay;
    private bool _endOfDay;
    private readonly AddressablesAssetService _assetService;

    public ClockController(AddressablesAssetService assetService)
    {
        _assetService = assetService;
    }

    public async void init()
    {
        _gameTime = new DateTime();
        _currentDay = _gameTime.Day;

        _view = await _assetService.InstantiateAsync<ClockView>();
        _view.over.Add(handleOver);
        _view.off.Add(() => show(false));

        _view.SetTime(_gameTime);

        _view.canvasGroup.alpha = 0f;
    }

    public void advanceTime(double minutes)
    {
        _gameTime = _gameTime.AddMinutes(minutes);

        if (_currentDay != _gameTime.Day)
        {
            _endOfDay = true;
        }
    }

    public void beginDay()
    {
        _endOfDay = false;
        _currentDay = _gameTime.Day;
    }

    public void handleOver()
    {
        _view.SetTime(_gameTime);
        show(true);
    }

    public async void show(bool show = true)
    {
        if (show)
        {
            await DOTweenUITransitions.fadeIn(_view.canvasGroup, _view.gameObject);
        }
        else
        {
            await DOTweenUITransitions.fadeOut(_view.canvasGroup, _view.gameObject, false);
        }
    }

    public void showTimeCost(bool show, double costInMinutes = 0)
    {        
        this.show(show);

        if (costInMinutes != 0)
        {
            _view.SetTime(_gameTime.AddMinutes(costInMinutes));
        }
    }

    public DateTime gameTime
    {
        get
        {
            return _gameTime;
        }
    }

    public bool isEndOfDay
    {
        get
        {
            return _endOfDay;
        }
    }
}
