using System;
using System.Collections.Generic;

public class InventoryController
{
    public Inventory Inventory => _saveStateController.CurrentSave.Inventory;
    readonly SaveStateController _saveStateController;

    public InventoryController(SaveStateController saveStateController)
    {
        _saveStateController = saveStateController;
    }

    public long Amount(string type)
    {
        if (Inventory.items.ContainsKey(type))
        {
            return Inventory.items[type].amount;
        }

        return 0;
    }

    public void Set(string type, long amount, bool replaceAmount)
    {
        if (!Inventory.items.ContainsKey(type))
        {
            Inventory.items[type] = new InventoryItem(type);
        }

        if (replaceAmount)
        {
            Inventory.items[type].amount = amount;
        }
        else
        {
            Inventory.items[type].amount += amount;
        }

        _saveStateController.Save();
    }

    public void Add(string itemType)
    {
        Set(itemType, 1, false);
    }

    public void Remove(string itemType)
    {
        Set(itemType, -1, false);
    }

    public void Clear()
    {
        if (Inventory.items != null)
        {
            Inventory.items.Clear();
            _saveStateController.Save();
        }
    }
}

[Serializable]
public class Inventory
{
    public Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>();
}

[Serializable]
public class InventoryItem
{
    public InventoryItem()
    {

    }

    public InventoryItem(string type, long amount = 0)
    {
        this.type = type;
        this.amount = amount;
    }

    public InventoryItem clone()
    {
        InventoryItem newItem = new InventoryItem(this.type, this.amount);
        newItem.label = this.label;
        return newItem;
    }

    public string type;
    public string label;
    public long amount = 0;
}