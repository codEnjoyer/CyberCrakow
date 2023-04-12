using System;
using ScriptableObjects.Inventory.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item) return;
        inventory.AddItem(item.item, 1);
        Destroy(other.gameObject);
    }

    private void Update()
    {
        // just for testing purpose
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.Clear();
    }
}
