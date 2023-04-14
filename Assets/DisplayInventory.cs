using System.Collections.Generic;
using ScriptableObjects.Inventory.Scripts;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItems;
    public int ySpaceBetweenItems;
    public int columnsNumber;
    private readonly Dictionary<InventorySlot, GameObject> _itemsDisplayed = new();

    // private void Start()
    // {
    //     CreateDisplay();
    // }
    //
    // private void Update()
    // {
    //     UpdateDisplay();
    // }
    //
    // private void CreateDisplay()
    // {
    //     for (var index = 0; index < inventory.container.Count; index++)
    //     {
    //         var inventorySlot = inventory.container[index];
    //         DisplayInventorySlot(inventorySlot, index);
    //     }
    // }
    //
    // private void DisplayInventorySlot(InventorySlot inventorySlot, int index)
    // {
    //     var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //     obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = 
    //     obj.GetComponent<RectTransform>().localPosition = GetPositionInGrid(index);
    //     obj.GetComponentInChildren<TextMeshProUGUI>().text = inventorySlot.amount.ToString("n0");
    //     _itemsDisplayed.Add(inventorySlot, obj);
    // }
    //
    // private Vector3 GetPositionInGrid(int itemIndex)
    // {
    //     return new Vector3(
    //         xStart + xSpaceBetweenItems * (itemIndex % columnsNumber),
    //         yStart + -ySpaceBetweenItems * (itemIndex / columnsNumber),
    //         0f);
    // }
    //
    // private void UpdateDisplay()
    // {
    //     for (var index = 0; index < inventory.container.Count; index++)
    //     {
    //         var inventorySlot = inventory.container[index];
    //         if (_itemsDisplayed.ContainsKey(inventorySlot))
    //         {
    //             _itemsDisplayed[inventorySlot].GetComponentInChildren<TextMeshProUGUI>().text =
    //                 inventorySlot.amount.ToString("n0");
    //         }
    //         else
    //         {
    //             DisplayInventorySlot(inventorySlot, index);
    //         }
    //     }
    // }
}