using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    private InventoryModel _inventory;

    private void Awake()
    {
        _inventory = new InventoryModel();
    }

    public void CollectResource(string name, int value)
    {
        _inventory.AddResource(name, value);
        UpdateUI();
    }

    public void UseResource(string name, int value)
    {
        _inventory.RemoveResource(name, value);
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (KeyValuePair<string, int> resource in _inventory.Inventory)
        {
            _inventoryView.UpdateUIElement(resource.Key, resource.Value.ToString());
        }
    }
}