using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Inventory
{
    public class PickUpItem : InteractableObject
    {
        public GameObject itemPrefab;
        public InventoryItemData data;
        InventoryController inventory;
        public ItemGrid grid;
        GridInventoryItem item;

        private void Awake()
        {
            grid = GameObject.FindGameObjectWithTag("MainGrid").GetComponent<ItemGrid>();
        }
        void Start()
        {
            item = itemPrefab.GetComponent<GridInventoryItem>();
            inventory = FindObjectOfType<InventoryController>().GetComponent<InventoryController>();
        }
        public override void Action()
        {
            item.SetItemData(data);
            inventory.CreateItem(item.Data, grid);
            Deactivate();
            Debug.Log("added");
            base.Action();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            Destroy(gameObject);
        }
    }
}