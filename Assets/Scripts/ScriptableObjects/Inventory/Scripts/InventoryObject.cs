using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Items.Scripts;
using UnityEngine;

namespace ScriptableObjects.Inventory.Scripts
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public List<InventorySlot> container = new();

        public void AddItem(ItemObject item, int amount)
        {
            var hasItem = false;
            foreach (var inventorySlot in container.Where(inventorySlot => inventorySlot.item == item))
            {
                inventorySlot.AddAmount(amount);
                hasItem = true;
                break;
            }

            if (!hasItem)
            {
                container.Add(new InventorySlot(item, amount));
            }
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemObject item;
        public int amount;

        public InventorySlot(ItemObject item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }
    }
}