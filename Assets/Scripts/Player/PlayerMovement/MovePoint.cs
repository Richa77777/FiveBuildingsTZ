using UnityEngine;
using System.Collections.Generic;

public class MovePoint : MonoBehaviour
{
    private List<GameObject> _allChilds = new List<GameObject>();


    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
            _allChilds.Add(transform.GetChild(i).gameObject);
    }
    public void EnableArrow(Vector3 position)
    {
        ToggleAllChilds(true);
        transform.position = position;
    }

    public void DisableArrow()
    {
        ToggleAllChilds(false);
    }
    
    private void ToggleAllChilds(bool enable)
    {
        foreach (GameObject child in _allChilds)
        {
            child.gameObject.SetActive(enable);
        }
    }
}
