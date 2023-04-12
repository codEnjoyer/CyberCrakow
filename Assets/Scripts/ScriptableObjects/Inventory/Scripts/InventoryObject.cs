using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ScriptableObjects.Items.Scripts;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Inventory.Scripts
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public string savePath;
        private ItemDatabaseObject _database;
        public List<InventorySlot> container = new();

        public void AddItem(ItemObject item, int amount)
        {
            foreach (var inventorySlot in container.Where(inventorySlot => inventorySlot.item == item))
            {
                inventorySlot.AddAmount(amount);
                return;
            }

            container.Add(new InventorySlot(_database.itemId[item], item, amount));
        }

        public void Save()
        {
            var saveData = JsonUtility.ToJson(this, true);
            var bf = new BinaryFormatter();
            var file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            bf.Serialize(file, saveData);
            file.Close();
        }

        public void Load()
        {
            var path = string.Concat(Application.persistentDataPath, savePath);
            if (File.Exists(path))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(path, FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
                file.Close();
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            _database = (ItemDatabaseObject) AssetDatabase.LoadAssetAtPath(
                "Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
            _database = Resources.Load<ItemDatabaseObject>("Database");
#endif
        }

        public void OnBeforeSerialize()
        {
            // throw new System.NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            foreach (var inventorySlot in container)
            {
                inventorySlot.item = _database.idItem[inventorySlot.id];
            }
        }
    }

    [Serializable]
    public class InventorySlot
    {
        public int id;
        public ItemObject item;
        public int amount;

        public InventorySlot(int id, ItemObject item, int amount)
        {
            this.id = id;
            this.item = item;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }
    }
}