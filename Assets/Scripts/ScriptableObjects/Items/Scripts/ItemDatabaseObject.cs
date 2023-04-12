using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] items;
        public Dictionary<ItemObject, int> itemId = new();
        public Dictionary<int, ItemObject> idItem = new();

        public void OnBeforeSerialize()
        {
            // throw new System.NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            itemId = new Dictionary<ItemObject, int>();
            idItem = new Dictionary<int, ItemObject>();
            for (var i = 0; i < items.Length; i++)
            {
                itemId.Add(items[i], i);
                idItem.Add(i, items[i]);
            }
        }
    }
}