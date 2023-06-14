using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Inventory
{
    public class Chest : InteractableObject
    {
        public InventoryItemData[] itemsInChest;
        public ChestController controller;
        InventoryController inventory;
        OpenInventory opener;
        public ItemGrid chestGrid;
        [SerializeField]public GameObject itemPrefab;
        GridInventoryItem[] items;
        [HideInInspector] public bool isChestOpen = false;
        [HideInInspector] private bool openFirstTime = true;

        private void Start()
        {
            inventory = controller.inventory;
            items = new GridInventoryItem[itemsInChest.Length];
            //chestGrid = controller.chestGrid;
            opener = controller.opener;
            chestGrid.gameObject.SetActive(false);
        }
        public override void Action()
        {
            base.Action();
            OpenChest();
            Deactivate();
            Invoke(nameof(Recover), 0.1f);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            gameObject.SetActive(false);
        }
        public override void Recover()
        {
            base.Recover();
            gameObject.SetActive(true);
        }
        public void OpenChest()
        {
            opener.Open();
            chestGrid.gameObject.SetActive(true);
            if (openFirstTime)
            {
                if (chestGrid == null)
                {
                    chestGrid = GameObject.FindGameObjectWithTag("ChestGrid").GetComponent<ItemGrid>();
                }
                for (int i = 0; i < itemsInChest.Length; i++)
                {
                    inventory.CreateItem(itemsInChest[i], chestGrid);
                    //inventory.InsertCreatedRandomItem(chestGrid);
                }
            }
            openFirstTime = false;
            isChestOpen = true;
            //for(int x = 0;x < chestGrid.tilesWidthCount;x++)
            //    for(int y = 0; y< chestGrid.tilesHeightCount;y++)
            //    {
            //        if(chestGrid._inventoryItemSlot[x,y] != null)
            //        {
            //            Debug.Log(chestGrid._inventoryItemSlot[x, y].Data);
            //        }
            //    }
        }
        public void CloseChest()
        {
            chestGrid.gameObject.SetActive(false);
            isChestOpen = false;
        }
    }
}

