using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private ResourceUIElement[] _resourceUIElements = new ResourceUIElement[5];

    public void UpdateUIElement(string resourceName, string value)
    {
        for (int i = 0; i < _resourceUIElements.Length; i++)
        {
            if (_resourceUIElements[i].Name == resourceName)
            {
                _resourceUIElements[i].Text.text = value;
                break;
            }
        }
    }

    [System.Serializable]
    private struct ResourceUIElement
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
    }
}