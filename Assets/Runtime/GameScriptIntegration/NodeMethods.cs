public class NodeMethods
{
    private static SaveStateController _saveStateController;
    private static InventoryController _inventoryController;

    public NodeMethods(SaveStateController saveStateController, InventoryController inventoryController)
    {
        _saveStateController = saveStateController;
        _inventoryController = inventoryController;
    }

    public static void SaveEvent(string eventName)
    {
#if DEBUG_VERBOSE
            Debug.Log($"NodeMethods::SaveEvent {eventName}");
#endif
        _saveStateController.SaveOutcome(eventName);
    }

    public static bool CheckEvent(string eventName)
    {
#if DEBUG_VERBOSE
            Debug.Log($"NodeMethods::CheckEvent {eventName} : {_saveStateController.HasOutcome(eventName)}");
#endif
        return _saveStateController.HasOutcome(eventName);
    }

    public static bool HasItem(string itemName)
    {
#if DEBUG_VERBOSE
            Debug.Log($"NodeMethods::HasItem {itemName} : {_inventoryController.Amount(itemName) > 0}");
#endif
        return _inventoryController.Amount(itemName) > 0;
    }

    public static void AddItem(string itemName)
    {
#if DEBUG_VERBOSE
            Debug.Log($"NodeMethods::GiveItem {itemName}");
#endif
        _inventoryController.Add(itemName);
    }

    public static void RemoveItem(string itemName)
    {
#if DEBUG_VERBOSE
            Debug.Log($"NodeMethods::RemoveItem {itemName}");
#endif
        _inventoryController.Remove(itemName);
    }
}