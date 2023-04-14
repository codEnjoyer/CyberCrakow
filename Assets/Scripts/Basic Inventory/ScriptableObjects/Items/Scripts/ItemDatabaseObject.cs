using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] Items { get; set; }
        public Dictionary<int, ItemObject> idItem = new();

        public void OnBeforeSerialize()
        {
            idItem = new Dictionary<int, ItemObject>();
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < Items.Length; i++)
            {
                Items[i].Id = i;
                idItem.Add(i, Items[i]);
            }
        }
    }
}