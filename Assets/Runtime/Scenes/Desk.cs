using VContainer;

class Desk : CustomSceneController
{
    [Inject]
    readonly UIController _uiController;
    [Inject]
    readonly InboxController _inboxController;
    
    override public void handleHotspotClick(Hotspot hotspot)
    {
        if (hotspot.type == Hotspot.Type.Action)
        {
            _inboxController.CloseButtonClicked += handleInboxClose;
            _inboxController.show();
        }
    }

    override public void handleHotspotOver(Hotspot hotspot)
    {
        if (hotspot.type == Hotspot.Type.Action)
        {
            _uiController.SetText("Use PC");
        }
    }

    override public void handleHotspotOff(Hotspot hotspot)
    {

    }

    private void handleInboxClose()
    {
        if (_inboxController.showing)
        {
            _inboxController.CloseButtonClicked -= handleInboxClose;
            _inboxController.show(false);
        }
    }
}
