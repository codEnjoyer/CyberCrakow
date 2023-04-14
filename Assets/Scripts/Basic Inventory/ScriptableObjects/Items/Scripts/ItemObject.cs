using System;
using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    public enum ItemType
    {
        Default,
        Food,
        Equipment,
    }

    public abstract class ItemObject : ScriptableObject
    {
        public int Id { get; set; }
        public Sprite uiDisplay;
        public ItemType type;

        [TextArea(15, 20)]
        public string description;
    }

    [Serializable]
    public class InventoryItem
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public InventoryItem(ItemObject itemObject)
        {
            Name = itemObject.name;
            Id = itemObject.Id;
        }
    }
}