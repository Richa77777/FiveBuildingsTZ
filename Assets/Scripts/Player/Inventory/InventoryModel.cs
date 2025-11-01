using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private Dictionary<string, int> _inventory = new Dictionary<string, int>()
    {
        { "Wood", 0 },
        { "Stone", 0 },
        { "Coal", 0 },
        { "Sand", 0 },
        { "Gold", 0 },
    };

    public IReadOnlyDictionary<string, int> Inventory => _inventory;

    public void AddResource(string resource, int value = 1)
    {
        if (_inventory.ContainsKey(resource))
            _inventory[resource] = Mathf.Clamp(_inventory[resource] + value, 0, int.MaxValue);
        else
            ResourceWarning();
    }

    public void RemoveResource(string resource, int value = 1)
    {
        if (_inventory.ContainsKey(resource))
            _inventory[resource] = Mathf.Clamp(_inventory[resource] - value, 0, int.MaxValue);
        else
            ResourceWarning();
    }

    public int GetResourceValue(string resource)
    {
        if (_inventory.ContainsKey(resource))
            return _inventory[resource];
        else
        {
            ResourceWarning();
            return -1;
        }
    }

    private void ResourceWarning()
    {
        Console.WriteLine("Resource not exists");
    }
}